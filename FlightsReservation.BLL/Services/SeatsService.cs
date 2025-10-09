using FlightsReservation.BLL.DtoEntities.SeatDtos;
using FlightsReservation.BLL.Interfaces;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;
using AutoMapper;

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

    public async Task<SeatReadDto?> GetSeatByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return null;
        }

        var seat = await _seatsRepository.GetByIdAsync(id, ct);
        if (seat is null)
        {
            Console.WriteLine("Seat not found");
            return null;
        }

        return _mapper.Map<SeatReadDto>(seat);
    }

    public async Task AddSeatAsync(SeatCreateDto createDto, CancellationToken ct = default)
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

        var seat = _mapper.Map<Seat>(createDto);
        try
        {
            await _seatsRepository.AddAsync(seat, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Internal server error");
        }
    }

    public async Task UpdateSeatAsync(SeatUpdateDto updateDto, CancellationToken ct = default)
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

        var seat = await _seatsRepository.GetByIdAsync(updateDto.Id, ct);
        if (seat is null)
        {
            Console.WriteLine("Seat not found");
            return;
        }

        _mapper.Map(updateDto, seat);
        try
        {
            await _seatsRepository.UpdateAsync(seat, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Internal server error");
        }
    }

    public async Task DeleteSeat(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
        {
            Console.WriteLine("Bad id");
            return;
        }

        await _seatsRepository.DeleteAsync(id, ct); //Add deleted count check
    }
}