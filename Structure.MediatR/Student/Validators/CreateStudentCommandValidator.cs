using FluentValidation;
using Structure.MediatR.Student.Commands;

namespace Structure.MediatR.Validators;

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.Age)
            .InclusiveBetween(18, 100).WithMessage("Age must be between 18 and 100.");
    }
}
