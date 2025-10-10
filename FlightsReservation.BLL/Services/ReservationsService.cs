using FlightsReservation.BLL.DtoEntities.ReservationDtos;
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
    public async Task<ReservationReadDto?> GetReservationByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return null;
        }

        var reservation = await _reservationsRepository.GetByIdAsync(id, ct);
        if (reservation is null)
        {
            Console.WriteLine("Reservation not found");
            return null;
        }

        return _mapper.Map<ReservationReadDto>(reservation);
    }

    public async Task AddReservationAsync(ReservationCreateDto createDto, CancellationToken ct = default)
    {
        if (createDto.Passengers.Count == 0)
        {
            Console.WriteLine("Reservation must have at least one passenger.");
            return;
        }
        
        createDto.Id = Guid.NewGuid();

        foreach (var p in createDto.Passengers)
        {
            p.ReservationId = createDto.Id;
        }

        var validationResult = await _reservationValidator.ValidateAsync(createDto, ct);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        foreach (var p in createDto.Passengers)
        {
            validationResult = await _passengerValidator.ValidateAsync(p, ct);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return;
            }
        }

        if (createDto.Passengers.Select(p => p.SeatId).Distinct().Count() != createDto.Passengers.Count)
        {
            Console.WriteLine("Duplicate SeatId detected.");
            return;
        }

        var reservation = _mapper.Map<Reservation>(createDto);

        if (reservation.Passengers.Any(p => p.SeatId == Guid.Empty))
        {
            Console.WriteLine("Each passenger must have a valid SeatId.");
            return;
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
            }
            catch (ArgumentException aex)
            {
                await _unitOfWork.RollbackAsync(ct);
                Console.WriteLine(aex.Message);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(ct);
                Console.WriteLine("Unknown error marking seat as occupied.");
            }
        }
        
        try
        {
            await _reservationsRepository.AddAsync(reservation, ct);
            await _unitOfWork.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            Console.WriteLine($"Error adding reservation: {ex.Message}");
        }
    }

    public async Task DeleteReservation(Guid id, CancellationToken ct = default)
    {
        //rework for all Guid.TryParse
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return;
        }

        var existingReservation = await _reservationsRepository.GetByIdAsync(id, ct);
        if (existingReservation is null)
        {
            Console.WriteLine("Passenger not found");
            return;
        }

        await _unitOfWork.BeginAsync(ct);

        foreach (var passenger in existingReservation.Passengers)
        {
            try
            {
                await _seatsRepository.MarkSeatAsAvailable(passenger.SeatId, ct);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(ct);
            }
        }

        try
        {
            await _reservationsRepository.DeleteAsync(existingReservation.Id, ct);
            await _unitOfWork.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            Console.WriteLine($"Error deleting passenger: {ex.Message}");
        }
    }
}