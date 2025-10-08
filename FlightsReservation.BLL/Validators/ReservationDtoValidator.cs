using FlightsReservation.BLL.Interfaces;
using FluentValidation;

namespace FlightsReservation.BLL.Validators;

public class ReservationDtoValidator : AbstractValidator<IReservationDto>
{
    public ReservationDtoValidator()
    {
        //Ids
        RuleFor(reservation => reservation.Id)
            .NotEmpty().WithMessage("Reservation ID is required.");

        //String
        RuleFor(reservation => reservation.ReservationNumber)
            .NotEmpty().WithMessage("Reservation number is required.")
            .MaximumLength(30).WithMessage("Reservation number cannot exceed 30 characters.");

        //Date
        RuleFor(reservation => reservation.ReservationDate)
            .NotEmpty().WithMessage("Reservation date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Reservation date cannot be in the future.");
    }
}