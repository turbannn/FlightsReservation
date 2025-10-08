using FlightsReservation.BLL.Interfaces;
using FluentValidation;

namespace FlightsReservation.BLL.Validators;

public class PassengerDtoValidator : AbstractValidator<IPassengerDto>
{
    public PassengerDtoValidator()
    {
        //Ids
        RuleFor(passenger => passenger.Id)
            .NotEmpty().WithMessage("Passenger ID is required.");

        //String
        RuleFor(passenger => passenger.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(40).WithMessage("First name cannot exceed 40 characters.");

        RuleFor(passenger => passenger.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(40).WithMessage("Last name cannot exceed 40 characters.");

        RuleFor(passenger => passenger.PassportNumber)
            .NotEmpty().WithMessage("Passport number is required.")
            .MaximumLength(15).WithMessage("Passport number cannot exceed 15 characters.");

        RuleFor(passenger => passenger.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.")
            .MaximumLength(25).WithMessage("Email cannot exceed 25 characters.");

        RuleFor(passenger => passenger.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("A valid phone number is required.")
            .MaximumLength(15).WithMessage("Phone number cannot exceed 15 characters.");
    }
}