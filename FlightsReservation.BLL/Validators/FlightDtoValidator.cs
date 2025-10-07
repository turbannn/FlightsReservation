using FlightsReservation.BLL.Interfaces;
using FluentValidation;

namespace FlightsReservation.BLL.Validators;

public class FlightDtoValidator : AbstractValidator<IFlightDto>
{
    public FlightDtoValidator()
    {
        //Ids
        RuleFor(flight => flight.Id)
            .NotEmpty().WithMessage("Flight ID is required.");

        //String
        RuleFor(flight => flight.FlightNumber)
            .NotEmpty().WithMessage("Flight number is required.")
            .MaximumLength(20).WithMessage("Flight number cannot exceed 20 characters.");

        RuleFor(flight => flight.Departure)
            .NotEmpty().WithMessage("Departure is required.")
            .MaximumLength(20).WithMessage("Departure cannot exceed 20 characters.");

        RuleFor(flight => flight.Arrival)
            .NotEmpty().WithMessage("Arrival is required.")
            .MaximumLength(20).WithMessage("Arrival cannot exceed 20 characters.");
        
        RuleFor(flight => flight.AirplaneType)
            .NotEmpty().WithMessage("AirplaneType is required.")
            .MaximumLength(20).WithMessage("AirplaneType exceed 20 characters.");

        //Date
        RuleFor(flight => flight.DepartureTime)
            .NotEmpty().WithMessage("DepartureTime is required.");

        RuleFor(flight => flight.ArrivalTime)
            .NotEmpty().WithMessage("ArrivalTime is required.");
    }
}