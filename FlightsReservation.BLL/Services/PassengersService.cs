using FlightsReservation.BLL.Entities.DataTransferObjects.PassengerDtos;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Interfaces;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;
using AutoMapper;

namespace FlightsReservation.BLL.Services;

public class PassengersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<IPassengerDto> _validator;
    private readonly IPassengersRepository _passengersRepository;
    private readonly ISeatsRepository _seatsRepository;

    public PassengersService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<IPassengerDto> validator,
        IPassengersRepository passengersRepository,
        ISeatsRepository seatsRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _passengersRepository = passengersRepository;
        _seatsRepository = seatsRepository;
    }

    public async Task<FlightReservationResult<PassengerReadDto>> GetPassengerByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<PassengerReadDto>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var passenger = await _passengersRepository.GetByIdAsync(id, ct);
        if (passenger is null)
        {
            Console.WriteLine("Passenger not found");
            return FlightReservationResult<PassengerReadDto>.Fail("Passenger not found", ResponseCodes.NotFound);
        }


        var pasReadDto = _mapper.Map<PassengerReadDto>(passenger);

        return FlightReservationResult<PassengerReadDto>.Success(pasReadDto, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> UpdatePassengerAsync(PassengerUpdateDto updateDto, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(updateDto, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        var existingPassenger = await _passengersRepository.GetByIdAsync(updateDto.Id, ct);
        if (existingPassenger is null)
        {
            return FlightReservationResult<int>.Fail("Passenger not found", ResponseCodes.NotFound);
        }

        try
        {
            _mapper.Map(updateDto, existingPassenger);
        }
        catch
        {
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        var res = await _passengersRepository.UpdateAsync(existingPassenger, ct);
        if (!res)
        {
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }
        await _unitOfWork.SaveChangesAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> DeletePassengerAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<int>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var existingPassenger = await _passengersRepository.GetByIdAsync(id, ct);
        if (existingPassenger is null)
        {
            return FlightReservationResult<int>.Fail("Passenger not found", ResponseCodes.NotFound);
        }

        await _unitOfWork.BeginAsync(ct);
        try
        {
            await _seatsRepository.MarkSeatAsAvailable(existingPassenger.SeatId, ct);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            Console.WriteLine($"Seat with ID {existingPassenger.SeatId} does not exist.");
            return FlightReservationResult<int>.Fail("Seat not found", ResponseCodes.NotFound);
        }

        try
        {
            await _passengersRepository.DeleteAsync(existingPassenger.Id, ct);
            await _unitOfWork.CommitAsync(ct);
            return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            Console.WriteLine($"Error deleting passenger: {ex.Message}");
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }
    }
}