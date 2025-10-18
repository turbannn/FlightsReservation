using FlightsReservation.BLL.Interfaces.Dtos;
using FluentValidation;

namespace FlightsReservation.BLL.Validators;

public class ReservationDtoValidator : AbstractValidator<IReservationDto>
{
    public ReservationDtoValidator()
    {
        //Ids
        RuleFor(reservation => reservation.FlightId)
            .NotEmpty().WithMessage("Flight ID is required.");

        //String
        RuleFor(reservation => reservation.ReservationNumber)
            .NotEmpty().WithMessage("Reservation number is required.")
            .MaximumLength(30).WithMessage("Reservation number cannot exceed 30 characters.");
    }
}