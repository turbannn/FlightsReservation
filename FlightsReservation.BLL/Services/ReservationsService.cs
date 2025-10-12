using FlightsReservation.BLL.DtoEntities.ReservationDtos;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Interfaces;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;
using AutoMapper;


namespace FlightsReservation.BLL.Services;

public class ReservationsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<IReservationDto> _reservationValidator;
    private readonly IValidator<IPassengerDto> _passengerValidator;
    private readonly IReservationsRepository _reservationsRepository;
    private readonly ISeatsRepository _seatsRepository;

    public ReservationsService(IUnitOfWork unitOfWork, 
        IMapper mapper,
        IValidator<IReservationDto> reservationValidator,
        IValidator<IPassengerDto> passengerValidator,
        IReservationsRepository reservationsRepository,
        ISeatsRepository seatsRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _reservationValidator = reservationValidator;
        _passengerValidator = passengerValidator;
        _reservationsRepository = reservationsRepository;
        _seatsRepository = seatsRepository;
    }

    //ct
    public async Task<FlightReservationResult<ReservationReadDto>> GetReservationByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<ReservationReadDto>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var reservation = await _reservationsRepository.GetByIdAsync(id, ct);
        if (reservation is null)
        {
            Console.WriteLine("Reservation not found");
            return FlightReservationResult<ReservationReadDto>.Fail("Reservation not found", ResponseCodes.NotFound);
        }

        var reservationReadDto = _mapper.Map<ReservationReadDto>(reservation);

        return FlightReservationResult<ReservationReadDto>.Success(reservationReadDto, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> AddReservationAsync(ReservationCreateDto createDto, CancellationToken ct = default)
    {
        if (createDto.Passengers.Count == 0)
        {
            Console.WriteLine("ERROR: Reservation must have at least one passenger.");
            return FlightReservationResult<int>.Fail("Reservation must have at least one passenger.", ResponseCodes.BadRequest);
        }
        
        createDto.Id = Guid.NewGuid();

        foreach (var p in createDto.Passengers)
        {
            p.ReservationId = createDto.Id;
        }

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

        if (createDto.Passengers.Select(p => p.SeatId).Distinct().Count() != createDto.Passengers.Count)
        {
            Console.WriteLine("ERROR: Duplicate SeatId detected.");
            return FlightReservationResult<int>.Fail("Duplicate SeatId detected.", ResponseCodes.BadRequest);
        }

        Reservation reservation;

        try
        {
            reservation = _mapper.Map<Reservation>(createDto);
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
        foreach (var passenger in reservation.Passengers)
        {
            try
            {
                await _seatsRepository.MarkSeatAsOccupied(passenger.SeatId, ct); //test
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
        
        var res = await _reservationsRepository.AddAsync(reservation, ct);

        if (!res)
        {
            await _unitOfWork.RollbackAsync(ct);
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        await _unitOfWork.CommitAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

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