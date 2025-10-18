using FlightsReservation.BLL.Interfaces.Dtos;
using FluentValidation;

namespace FlightsReservation.BLL.Validators;

public class SeatDtoValidator : AbstractValidator<ISeatDto>
{
    public SeatDtoValidator()
    {
        //Ids
        RuleFor(seat => seat.FlightId)
            .NotEmpty().WithMessage("Flight ID is required.");
        
        //String
        RuleFor(seat => seat.SeatNumber)
            .NotEmpty().WithMessage("Seat number is required.")
            .MaximumLength(4).WithMessage("Seat number cannot exceed 4 characters.");
    }
}

