using FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;
using FlightsReservation.BLL.Interfaces;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;
using AutoMapper;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Entities.Utilities.Results;

namespace FlightsReservation.BLL.Services;

public class SeatsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<ISeatDto> _validator;
    private readonly ISeatsRepository _seatsRepository; 

    public SeatsService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<ISeatDto> validator, ISeatsRepository seatsRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _seatsRepository = seatsRepository;
    }

    public async Task<FlightReservationResult<SeatReadDto>> GetSeatByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<SeatReadDto>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var seat = await _seatsRepository.GetByIdAsync(id, ct);
        if (seat is null)
        {
            Console.WriteLine("Seat not found");
            return FlightReservationResult<SeatReadDto>.Fail("Seat not found", ResponseCodes.NotFound);
        }

        var seatReadDto = _mapper.Map<SeatReadDto>(seat);

        return FlightReservationResult<SeatReadDto>.Success(seatReadDto, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> AddSeatAsync(SeatCreateDto createDto, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(createDto, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        Seat seat;
        try
        {
            seat = _mapper.Map<Seat>(createDto);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Internal server error");
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        var res = await _seatsRepository.AddAsync(seat, ct);

        if (!res)
        {
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        await _unitOfWork.SaveChangesAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> UpdateSeatAsync(SeatUpdateDto updateDto, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(updateDto, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        var seat = await _seatsRepository.GetByIdAsync(updateDto.Id, ct);
        if (seat is null)
        {
            return FlightReservationResult<int>.Fail("Seat not found", ResponseCodes.NotFound);
        }

        try
        {
            _mapper.Map(updateDto, seat);
        }
        catch
        {
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        var res = await _seatsRepository.UpdateAsync(seat, ct);
        if (!res)
        {
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }
        await _unitOfWork.SaveChangesAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> DeleteSeat(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("ERROR: Bad id");
            return FlightReservationResult<int>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var res = await _seatsRepository.DeleteAsync(id, ct);
        if (!res)
        {
            return FlightReservationResult<int>.Fail("Seat not found", ResponseCodes.NotFound);
        }
        await _unitOfWork.SaveChangesAsync(ct);
        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }
}