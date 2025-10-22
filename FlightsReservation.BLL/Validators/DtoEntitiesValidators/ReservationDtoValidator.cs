using FlightsReservation.BLL.Interfaces.Dtos;
using FluentValidation;

namespace FlightsReservation.BLL.Validators.DtoEntitiesValidators;

public class ReservationDtoValidator : AbstractValidator<IReservationDto>
{
    public ReservationDtoValidator()
    {
        //Ids
        RuleFor(reservation => reservation.FlightId)
            .NotEmpty().WithMessage("Flight ID is required.");
    }
}