using FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;
using AutoMapper;
using FlightsReservation.BLL.Entities.Utilities.Requests;
using FlightsReservation.BLL.Interfaces.Dtos;
using FlightsReservation.BLL.Interfaces.Requests;

namespace FlightsReservation.BLL.Services;

public class FlightsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<IFlightDto> _flightValidator;
    private readonly IValidator<ISearchRequest> _requestValidator;
    private readonly IFlightsRepository _flightsRepository;

    public FlightsService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<IFlightDto> flightValidator,
        IValidator<ISearchRequest> requestValidator,
        IFlightsRepository flightsRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _flightValidator = flightValidator;
        _requestValidator = requestValidator;
        _flightsRepository = flightsRepository;
    }

    public async Task<FlightReservationPagedResult<List<FlightReadDto>>> GetFlightPageFromRequestWithReturnAsync(int page,
        int size,
        FlightSearchWithReturnRequest request,
        CancellationToken ct = default)
    {
        if (page <= 0)
        {
            Console.WriteLine("Bad page");
            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail("Bad page", ResponseCodes.BadRequest);
        }

        if (size <= 0)
        {
            Console.WriteLine("Bad size");
            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail("Bad size", ResponseCodes.BadRequest);
        }

        var validationResult = await _requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail(error.ToString(), ResponseCodes.BadRequest);
        }

        var flights = await _flightsRepository.GetFilteredPageWithReturnAsync(page,
            size,
            request.DepartureCity,
            request.ArrivalCity,
            request.DepartureDate,
            request.ReturnDate,
            ct);

        if (flights.Outbound.Count == 0)
        {
            Console.WriteLine("Flights weren't found");
            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail("Flights weren't found", ResponseCodes.NotFound);
        }

        if (flights.Return.Count == 0)
        {
            Console.WriteLine("Flights back weren't found");
            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail("Flights back weren't found", ResponseCodes.NotFound);
        }

        var concated = flights.Outbound.Concat(flights.Return).ToList();

        var count = await _flightsRepository.GetTotalCountAsync(ct);

        var flightReadDto = _mapper.Map<List<FlightReadDto>>(concated);

        return FlightReservationPagedResult<List<FlightReadDto>>.PagedSuccess(flightReadDto, count);
    }


    public async Task<FlightReservationPagedResult<List<FlightReadDto>>> GetFlightPageFromRequestAsync(int page,
        int size,
        FlightSearchRequest request,
        CancellationToken ct = default)
    {
        if (page <= 0)
        {
            Console.WriteLine("Bad page");
            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail("Bad page", ResponseCodes.BadRequest);
        }

        if (size <= 0)
        {
            Console.WriteLine("Bad size");
            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail("Bad size", ResponseCodes.BadRequest);
        }

        var validationResult = await _requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail(error.ToString(), ResponseCodes.BadRequest);
        }

        var flights = await _flightsRepository.GetFilteredPageAsync(page,
            size,
            request.DepartureCity,
            request.ArrivalCity,
            request.DepartureDate,
            ct);

        var count = await _flightsRepository.GetTotalCountAsync(ct);

        var flightReadDto = _mapper.Map<List<FlightReadDto>>(flights);

        return FlightReservationPagedResult<List<FlightReadDto>>.PagedSuccess(flightReadDto, count);
    }

    //Admin
    public async Task<FlightReservationPagedResult<List<FlightReadDto>>> GetFlightPageAsync(int page, int size, CancellationToken ct = default)
    {
        if (page <= 0)
        {
            Console.WriteLine("Bad page");
            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail("Bad page", ResponseCodes.BadRequest);
        }

        if (size <= 0)
        {
            Console.WriteLine("Bad size");
            return FlightReservationPagedResult<List<FlightReadDto>>.PagedFail("Bad size", ResponseCodes.BadRequest);
        }

        var flights = await _flightsRepository.GetPageAsync(page, size, ct);

        var count = await _flightsRepository.GetTotalCountAsync(ct);

        var flightReadDto = _mapper.Map<List<FlightReadDto>>(flights);

        return FlightReservationPagedResult<List<FlightReadDto>>.PagedSuccess(flightReadDto, count);
    }

    //Admin
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

    //Admin
    public async Task<FlightReservationResult<int>> AddFlightAsync(FlightCreateDto createDto, CancellationToken ct = default)
    {
        var validationResult = await _flightValidator.ValidateAsync(createDto, ct);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First();

            return FlightReservationResult<int>.Fail(error.ToString(), ResponseCodes.BadRequest);
        }

        createDto.FlightNumber = $"FN-{createDto.DepartureTime.Year}{createDto.DepartureTime.Month}{createDto.DepartureTime.Day}{createDto.DepartureTime.Hour}{createDto.DepartureTime.Minute}{createDto.DepartureTime.Second}";

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

    //Admin
    public async Task<FlightReservationResult<int>> UpdateFlightAsync(FlightUpdateDto updateDto, CancellationToken ct = default)
    {
        var validationResult = await _flightValidator.ValidateAsync(updateDto, ct);
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

    //Admin
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

    //Admin
    public async Task<FlightReservationResult<int>> DeleteAllFlights(CancellationToken ct = default)
    {
        var res = await _flightsRepository.DeleteAllAsync(ct);
        if (!res)
        {
            return FlightReservationResult<int>.Fail("Flight was not updated", ResponseCodes.InternalServerError);
        }
        await _unitOfWork.SaveChangesAsync(ct);

        return FlightReservationResult<int>.Success(1, ResponseCodes.Success);
    }
}