using System;
using FluentValidation;

namespace BackendBlocks.Core.Features.Hashs.Commands.CreateHash;

public class CreateHashCommandValidator : AbstractValidator<CreateHashCommand>
{
    public CreateHashCommandValidator()
    {
        // RuleFor(x => x.ShaHash)
        //     .NotEmpty()
        //     .WithMessage("Hash is required");

        // RuleFor(x => x.Date)
        //     .GreaterThan(DateTime.MinValue)
        //     .LessThan(DateTime.MaxValue)
        //     .WithMessage("Date must be a valid");
    }
}
