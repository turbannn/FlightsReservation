using FlightsReservation.BLL.Entities.DataTransferObjects.ReservationDtos;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Interfaces;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;
using AutoMapper;
using FlightsReservation.BLL.Interfaces.Dtos;
using FlightsReservation.BLL.Interfaces.Services;


namespace FlightsReservation.BLL.Services.EntityServices;

public class ReservationsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<IReservationDto> _reservationValidator;
    private readonly IValidator<IPassengerDto> _passengerValidator;
    private readonly IReservationsRepository _reservationsRepository;
    private readonly ISeatsRepository _seatsRepository;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IEmailService _emailService;
    private readonly IPdfService _pdfService;

    public ReservationsService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<IReservationDto> reservationValidator,
        IValidator<IPassengerDto> passengerValidator,
        IReservationsRepository reservationsRepository,
        ISeatsRepository seatsRepository,
        IFlightsRepository flightsRepository,
        IUsersRepository usersRepository,
        IEmailService emailService,
        IPdfService pdfService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _reservationValidator = reservationValidator;
        _passengerValidator = passengerValidator;
        _reservationsRepository = reservationsRepository;
        _seatsRepository = seatsRepository;
        _flightsRepository = flightsRepository;
        _usersRepository = usersRepository;
        _emailService = emailService;
        _pdfService = pdfService;
    }

    //Admin
    public async Task<FlightReservationResult<ReservationAdminReadDto>> GetReservationByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<ReservationAdminReadDto>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var reservation = await _reservationsRepository.GetByIdAsync(id, ct);
        if (reservation is null)
        {
            Console.WriteLine("Reservation not found");
            return FlightReservationResult<ReservationAdminReadDto>.Fail("Reservation not found", ResponseCodes.NotFound);
        }

        var reservationReadDto = _mapper.Map<ReservationAdminReadDto>(reservation);

        return FlightReservationResult<ReservationAdminReadDto>.Success(reservationReadDto, ResponseCodes.Success);
    }

    //Admin
    public async Task<FlightReservationResult<int>> CommitReservationAsync(
            Guid userId,
            ReservationCreateDto createDto,
            CancellationToken ct = default)
    {
        var precheck = await PrecheckAndFetchEntitiesAsync(userId, createDto, ct);
        if (!precheck.IsSuccess)
            return precheck.Result;

        var (user, flight) = (precheck.User!, precheck.Flight!);

        var businessCheck = CheckBusinessRules(user, flight, createDto);
        if (!businessCheck.IsSuccess)
            return businessCheck;

        InitializeReservationIdentifiers(createDto);

        var validation = await ValidateReservationAndPassengersAsync(createDto, ct);
        if (!validation.IsSuccess)
            return validation;

        if (createDto.Passengers.Select(p => p.SeatId).Distinct().Count() != createDto.Passengers.Count)
        {
            Console.WriteLine("ERROR: Duplicate SeatId detected.");
            return FlightReservationResult<int>.Fail("Duplicate SeatId detected.", ResponseCodes.BadRequest);
        }

        Reservation reservation;
        try
        {
            reservation = MapToReservation(createDto, user);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Internal server error");
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        if (reservation.Passengers.Any(p => p.SeatId == Guid.Empty))
        {
            Console.WriteLine("ERROR: Each passenger must have a valid SeatId.");
            return FlightReservationResult<int>.Fail("Each passenger must have a valid SeatId.", ResponseCodes.BadRequest);
        }

        await _unitOfWork.BeginAsync(ct);

        try
        {
            await _usersRepository.SubtractMoneyAsync(user.Id, flight.Price * reservation.Passengers.Count, ct);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            Console.WriteLine(ex.Message);
            Console.WriteLine("INTERNAL SERVER ERROR ADD RESERVATION");
            return FlightReservationResult<int>.Fail("Error subtracting money.", ResponseCodes.InternalServerError);
        }

        var passengerProcessingResult = await ProcessPassengersAsync(reservation.Passengers, flight, ct);
        if (!passengerProcessingResult.IsSuccess)
        {
            return passengerProcessingResult;
        }

        var saved = await _reservationsRepository.AddAsync(reservation, ct);
        if (!saved)
        {
            await _unitOfWork.RollbackAsync(ct);
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        await _unitOfWork.CommitAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

    //Helpers

    private async Task<(bool IsSuccess, FlightReservationResult<int> Result, User? User, Flight? Flight)> PrecheckAndFetchEntitiesAsync(
        Guid userId,
        ReservationCreateDto createDto,
        CancellationToken ct)
    {
        if (userId == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad user id");
            return (false, FlightReservationResult<int>.Fail("Bad user id", ResponseCodes.BadRequest), null, null);
        }

        var user = await _usersRepository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            Console.WriteLine("ERROR: User not found.");
            return (false, FlightReservationResult<int>.Fail("User not found.", ResponseCodes.NotFound), null, null);
        }

        var flight = await _flightsRepository.GetByIdAsync(createDto.FlightId, ct);
        if (flight is null)
        {
            Console.WriteLine("ERROR: Flight not found.");
            return (false, FlightReservationResult<int>.Fail("Flight not found.", ResponseCodes.NotFound), null, null);
        }

        return (true, FlightReservationResult<int>.Success(0, ResponseCodes.Success), user, flight);
    }

    private FlightReservationResult<int> CheckBusinessRules(User user, Flight flight, ReservationCreateDto createDto)
    {
        if (user.Money < flight.Price * createDto.Passengers.Count)
        {
            Console.WriteLine("ERROR: Insufficient funds.");
            return FlightReservationResult<int>.Fail("Insufficient funds.", ResponseCodes.BadRequest);
        }

        if (createDto.Passengers.Count == 0)
        {
            Console.WriteLine("ERROR: Reservation must have at least one passenger.");
            return FlightReservationResult<int>.Fail("Reservation must have at least one passenger.", ResponseCodes.BadRequest);
        }

        return FlightReservationResult<int>.Success(0, ResponseCodes.Success);
    }

    private void InitializeReservationIdentifiers(ReservationCreateDto createDto)
    {
        createDto.Id = Guid.NewGuid();
        createDto.ReservationNumber = $"RE_{DateTime.UtcNow.AddHours(2)}";

        foreach (var p in createDto.Passengers)
        {
            p.ReservationId = createDto.Id;
        }
    }

    private async Task<FlightReservationResult<int>> ValidateReservationAndPassengersAsync(ReservationCreateDto createDto, CancellationToken ct)
    {
        var validationResult = await _reservationValidator.ValidateAsync(createDto, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();
            return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        foreach (var p in createDto.Passengers)
        {
            validationResult = await _passengerValidator.ValidateAsync(p, ct);
            if (!validationResult.IsValid)
            {
                var error = validationResult.Errors.First();
                return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
            }
        }

        return FlightReservationResult<int>.Success(0, ResponseCodes.Success);
    }

    private Reservation MapToReservation(ReservationCreateDto createDto, User user)
    {
        var reservation = _mapper.Map<Reservation>(createDto);
        reservation.UserId = user.Id;
        return reservation;
    }

    private async Task<FlightReservationResult<int>> ProcessPassengersAsync(IEnumerable<Passenger> passengers, Flight flight, CancellationToken ct)
    {
        foreach (var passenger in passengers)
        {
            try
            {
                await _seatsRepository.MarkSeatAsOccupied(passenger.SeatId, ct);

                var pdf = await _pdfService.GenerateTicketPdfAsync(passenger, flight);

                await _emailService.SendEmailAsync(passenger.Email,
                    pdf,
                    $"Ticket_{flight.FlightNumber}_{DateTime.UtcNow}.pdf",
                    ct);
            }
            catch (InvalidOperationException ioex)
            {
                await _unitOfWork.RollbackAsync(ct);
                Console.WriteLine($"{ioex.Message}");
                return FlightReservationResult<int>.Fail(ioex.Message, ResponseCodes.NotFound);
            }
            catch (ArgumentException aex)
            {
                await _unitOfWork.RollbackAsync(ct);
                Console.WriteLine(aex.Message);
                return FlightReservationResult<int>.Fail(aex.Message, ResponseCodes.BadRequest);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(ct);
                Console.WriteLine(ex.Message);
                Console.WriteLine("INTERNAL SERVER ERROR ADD RESERVATION");
                return FlightReservationResult<int>.Fail("Unknown error.", ResponseCodes.InternalServerError);
            }
        }

        return FlightReservationResult<int>.Success(0, ResponseCodes.Success);
    }
    //End helpers

    //Admin
    public async Task<FlightReservationResult<int>> DeleteReservation(Guid id, CancellationToken ct = default)
    {
        //rework for all Guid.TryParse
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<int>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var existingReservation = await _reservationsRepository.GetByIdAsync(id, ct);
        if (existingReservation is null)
        {
            Console.WriteLine("Passenger not found");
            return FlightReservationResult<int>.Fail("Passenger not found", ResponseCodes.NotFound);
        }

        await _unitOfWork.BeginAsync(ct);

        foreach (var passenger in existingReservation.Passengers)
        {
            try
            {
                await _seatsRepository.MarkSeatAsAvailable(passenger.SeatId, ct);
            }
            catch (InvalidOperationException ioex)
            {
                await _unitOfWork.RollbackAsync(ct);
                Console.WriteLine($"{ioex.Message}");
                return FlightReservationResult<int>.Fail(ioex.Message, ResponseCodes.NotFound);
            }
            catch (ArgumentException aex)
            {
                await _unitOfWork.RollbackAsync(ct);
                Console.WriteLine(aex.Message);
                return FlightReservationResult<int>.Fail(aex.Message, ResponseCodes.BadRequest);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(ct);
                Console.WriteLine("INTERNAL SERVER ERROR ADD RESERVATION");
                return FlightReservationResult<int>.Fail("Unknown error marking seat as occupied.", ResponseCodes.InternalServerError);
            }
        }

        var res = await _reservationsRepository.DeleteAsync(existingReservation.Id, ct);
        if (!res)
        {
            await _unitOfWork.RollbackAsync(ct);
            return FlightReservationResult<int>.Fail("Reservation was not deleted", ResponseCodes.NotFound);
        }
        await _unitOfWork.CommitAsync(ct);
        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }
}