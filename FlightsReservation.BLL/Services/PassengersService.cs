using FlightsReservation.BLL.DtoEntities.PassengerDtos;
using FlightsReservation.BLL.Interfaces;
using FlightsReservation.DAL.Entities.Model;
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

    public PassengersService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<IPassengerDto> validator, IPassengersRepository passengersRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _passengersRepository = passengersRepository;
    }

    public async Task<PassengerReadDto?> GetPassengerByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return null;
        }

        var passenger = await _passengersRepository.GetByIdAsync(id);
        if (passenger is null)
        {
            Console.WriteLine("Passenger not found");
            return null;
        }

        return _mapper.Map<PassengerReadDto>(passenger);
    }

    public async Task AddPassengerAsync(PassengerCreateDto createDto)
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

        var passenger = _mapper.Map<Passenger>(createDto);
        try
        {
            await _passengersRepository.AddAsync(passenger);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding passenger: {ex.Message}");
        }
    }


}