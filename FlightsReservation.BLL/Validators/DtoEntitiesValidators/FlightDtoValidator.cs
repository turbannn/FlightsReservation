using FlightsReservation.BLL.Interfaces.Dtos;
using FluentValidation;

namespace FlightsReservation.BLL.Validators.DtoEntitiesValidators;

public class FlightDtoValidator : AbstractValidator<IFlightDto>
{
    public FlightDtoValidator()
    {
        //Int
        RuleFor(flight => flight.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(flight => flight.Departure)
            .NotEmpty().WithMessage("Departure is required.")
            .MaximumLength(30).WithMessage("Departure cannot exceed 30 characters.");

        RuleFor(flight => flight.Arrival)
            .NotEmpty().WithMessage("Arrival is required.")
            .MaximumLength(30).WithMessage("Arrival cannot exceed 30 characters.");

        RuleFor(flight => flight.AirplaneType)
            .NotEmpty().WithMessage("AirplaneType is required.")
            .MaximumLength(20).WithMessage("AirplaneType exceed 20 characters.");

        RuleFor(flight => flight.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .MaximumLength(4).WithMessage("Currency exceed 4 characters.");

        RuleFor(flight => flight.Company)
            .NotEmpty().WithMessage("Company is required.")
            .MaximumLength(20).WithMessage("Company exceed 20 characters.");

        //Date
        RuleFor(flight => flight.DepartureTime)
            .NotEmpty().WithMessage("DepartureTime is required.");

        RuleFor(flight => flight.ArrivalTime)
            .NotEmpty().WithMessage("ArrivalTime is required.");
    }
}