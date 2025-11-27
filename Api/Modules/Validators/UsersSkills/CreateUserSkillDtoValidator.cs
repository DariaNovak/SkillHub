using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.UsersSkills;

public class CreateUserSkillDtoValidator : AbstractValidator<CreateUserSkillDto>
{
    public CreateUserSkillDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.SkillId)
            .NotEmpty()
            .WithMessage("SkillId is required");

        RuleFor(x => x.ProficiencyLevel)
            .InclusiveBetween(1, 5)
            .WithMessage("ProficiencyLevel must be between 1 and 5");
    }
}
