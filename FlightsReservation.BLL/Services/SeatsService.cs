using AutoMapper;
using FlightsReservation.BLL.Interfaces;
using FlightsReservation.DAL.Interfaces;
using FluentValidation;

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


}