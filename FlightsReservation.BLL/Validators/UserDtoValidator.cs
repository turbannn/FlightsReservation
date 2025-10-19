using FlightsReservation.BLL.Interfaces.Dtos;
using FluentValidation;

namespace FlightsReservation.BLL.Validators;

public class UserDtoValidator : AbstractValidator<IUserDto>
{
    public UserDtoValidator()
    {
        //String
        RuleFor(u => u.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(120).WithMessage("Password cannot exceed 120 characters.");
    }
}