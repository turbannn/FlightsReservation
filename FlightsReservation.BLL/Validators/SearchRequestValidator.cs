using FlightsReservation.BLL.Interfaces.Requests;
using FluentValidation;

namespace FlightsReservation.BLL.Validators;

public class SearchRequestValidator : AbstractValidator<ISearchRequest>
{
    public SearchRequestValidator()
    {
        RuleFor(x => x.DepartureCity)
            .NotEmpty().WithMessage("Departure city is required.")
            .MaximumLength(30).WithMessage("Departure city cannot exceed 100 characters.");

        RuleFor(x => x.ArrivalCity)
            .NotEmpty().WithMessage("Arrival city is required.")
            .MaximumLength(30).WithMessage("Arrival city cannot exceed 100 characters.");

        RuleFor(x => x.DepartureDate)
            .GreaterThan(DateTime.Now).WithMessage("Departure date must be in the future.");
    }
}