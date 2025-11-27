using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.UsersSkills;

public class GetUserSkillsByUserIdDtoValidator : AbstractValidator<GetUserSkillsByUserIdDto>
{
    public GetUserSkillsByUserIdDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}
