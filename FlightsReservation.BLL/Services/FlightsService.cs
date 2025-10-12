using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.BLL.Interfaces;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;
using AutoMapper;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.BLL.Entities.DataTransferObjects.SeatDtos;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.DAL.Repositories;

namespace FlightsReservation.BLL.Services;

public class FlightsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<IFlightDto> _validator;
    private readonly IFlightsRepository _flightsRepository;

    public FlightsService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<IFlightDto> validator, IFlightsRepository flightsRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _flightsRepository = flightsRepository;
    }

    public async Task<FlightReservationResult<FlightReadDto>> GetFlightByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return FlightReservationResult<FlightReadDto>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var flight = await _flightsRepository.GetByIdAsync(id, ct);
        if (flight is null)
        {
            Console.WriteLine("Flight not found");
            return FlightReservationResult<FlightReadDto>.Fail("Flight not found", ResponseCodes.NotFound);
        }

        var flightReadDto = _mapper.Map<FlightReadDto>(flight);

        return FlightReservationResult<FlightReadDto>.Success(flightReadDto, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> AddFlightAsync(FlightCreateDto createDto, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(createDto, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        Flight? flight = null;

        try
        {
            flight = _mapper.Map<Flight>(createDto);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Internal server error");
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        var res = await _flightsRepository.AddAsync(flight, ct);
        if (!res)
        {
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }
        await _unitOfWork.SaveChangesAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> UpdateFlightAsync(FlightUpdateDto updateDto, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(updateDto, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        var existingFlight = await _flightsRepository.GetByIdAsync(updateDto.Id, ct);
        if (existingFlight is null)
        {
            Console.WriteLine("Flight not found");
            return FlightReservationResult<int>.Fail("Flight not found", ResponseCodes.NotFound);
        }

        try
        {
            _mapper.Map(updateDto, existingFlight);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Internal server error");
            return FlightReservationResult<int>.Fail("Internal server error", ResponseCodes.InternalServerError);
        }

        var res = await _flightsRepository.UpdateAsync(existingFlight, ct);
        if (!res)
        {
            return FlightReservationResult<int>.Fail("Flight was not updated", ResponseCodes.InternalServerError);
        }
        await _unitOfWork.SaveChangesAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }

    public async Task<FlightReservationResult<int>> DeleteFlight(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return FlightReservationResult<int>.Fail("Bad id", ResponseCodes.BadRequest);
        }

        var existingFlight = await _flightsRepository.GetByIdAsync(id, ct);
        if (existingFlight is null)
        {
            Console.WriteLine("Flight not found");
            return FlightReservationResult<int>.Fail("Flight not found", ResponseCodes.NotFound);
        }


        var res = await _flightsRepository.DeleteAsync(existingFlight.Id, ct);
        if (!res)
        {
            return FlightReservationResult<int>.Fail("Flight was not updated", ResponseCodes.InternalServerError);
        }
        await _unitOfWork.SaveChangesAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }
}