using FlightsReservation.BLL.Interfaces;
using FluentValidation;

namespace FlightsReservation.BLL.Validators;

public class ReservationValidator : AbstractValidator<IReservationDto>
{
    public ReservationValidator()
    {
        //Ids
        RuleFor(reservation => reservation.Id)
            .NotEmpty().WithMessage("Reservation ID is required.");

        //String
        RuleFor(reservation => reservation.ReservationNumber)
            .NotEmpty().WithMessage("Reservation number is required.")
            .MaximumLength(20).WithMessage("Reservation number cannot exceed 20 characters.");

        //Date
        RuleFor(reservation => reservation.ReservationDate)
            .NotEmpty().WithMessage("Reservation date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Reservation date cannot be in the future.");
    }
}