using FlightsReservation.BLL.Entities.Utilities.Requests;
using FluentValidation;

namespace FlightsReservation.BLL.Validators.RequestValidators;

public class PayuRequestValidator : AbstractValidator<PayuOrderRequest>
{
    public PayuRequestValidator()
    {
        //String
        RuleFor(request => request.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");

        RuleFor(request => request.BuyerEmail)
            .NotEmpty().WithMessage("Buyer email is required.")
            .EmailAddress().WithMessage("Buyer email must be a valid email address.")
            .MaximumLength(100).WithMessage("Buyer email cannot exceed 100 characters.");

        //Numeric
        RuleFor(request => request.TotalAmount)
            .GreaterThan(0).WithMessage("Total amount must be greater than zero.");
    }
}