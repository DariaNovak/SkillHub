using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.UsersSkills;

public class DeleteUserSkillDtoValidator : AbstractValidator<DeleteUserSkillDto>
{
    public DeleteUserSkillDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.SkillId)
            .NotEmpty()
            .WithMessage("SkillId is required");
    }
}
