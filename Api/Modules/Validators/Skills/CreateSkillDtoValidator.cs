using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Skills;

public class CreateSkillDtoValidator : AbstractValidator<CreateSkillDto>
{
    public CreateSkillDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2);
    }
}