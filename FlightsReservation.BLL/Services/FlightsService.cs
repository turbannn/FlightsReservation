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

    public async Task<FlightReadDto?> GetFlightByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return null;
        }

        var flight = await _flightsRepository.GetByIdAsync(id);
        if (flight is null)
        {
            Console.WriteLine("Flight not found");
            return null;
        }

        return _mapper.Map<FlightReadDto>(flight);
    }

    public async Task AddFlightAsync(FlightCreateDto createDto)
    {
        var validationResult = await _validator.ValidateAsync(createDto);
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
            await _flightsRepository.AddAsync(flight);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding flight: {ex.Message}");
        }
    }

    public async Task UpdateFlightAsync(FlightUpdateDto updateDto)
    {
        var validationResult = await _validator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        var existingFlight = await _flightsRepository.GetByIdAsync(updateDto.Id);
        if (existingFlight is null)
        {
            Console.WriteLine("Flight not found");
            return;
        }

        _mapper.Map(updateDto, existingFlight);

        try
        {
            await _flightsRepository.UpdateAsync(existingFlight);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating flight: {ex.Message}");
        }
    }

    public async Task DeleteFlight(Guid id)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return;
        }

        var existingFlight = await _flightsRepository.GetByIdAsync(id);
        if (existingFlight is null)
        {
            Console.WriteLine("Flight not found");
            return;
        }

        try
        {
            await _flightsRepository.DeleteAsync(existingFlight.Id);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting flight: {ex.Message}");
        }
    }
}