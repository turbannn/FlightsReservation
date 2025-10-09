using FlightsReservation.BLL.DtoEntities.FlightDtos;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.BLL.Interfaces;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;
using AutoMapper;

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

    public async Task<FlightReadDto?> GetFlightByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return null;
        }

        var flight = await _flightsRepository.GetByIdAsync(id, ct);
        if (flight is null)
        {
            Console.WriteLine("Flight not found");
            return null;
        }

        return _mapper.Map<FlightReadDto>(flight);
    }

    public async Task AddFlightAsync(FlightCreateDto createDto, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(createDto, ct);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        var flight = _mapper.Map<Flight>(createDto);
        try
        {
            await _flightsRepository.AddAsync(flight, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding flight: {ex.Message}");
        }
    }

    public async Task UpdateFlightAsync(FlightUpdateDto updateDto, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(updateDto, ct);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        var existingFlight = await _flightsRepository.GetByIdAsync(updateDto.Id, ct);
        if (existingFlight is null)
        {
            Console.WriteLine("Flight not found");
            return;
        }

        _mapper.Map(updateDto, existingFlight);

        try
        {
            await _flightsRepository.UpdateAsync(existingFlight, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating flight: {ex.Message}");
        }
    }

    public async Task DeleteFlight(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return;
        }

        var existingFlight = await _flightsRepository.GetByIdAsync(id, ct);
        if (existingFlight is null)
        {
            Console.WriteLine("Flight not found");
            return;
        }

        try
        {
            await _flightsRepository.DeleteAsync(existingFlight.Id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting flight: {ex.Message}");
        }
    }
}