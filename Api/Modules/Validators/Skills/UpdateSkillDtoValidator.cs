using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Skills;

public class UpdateSkillDtoValidator : Validator<UpdateSkillDto>
{
    public UpdateSkillDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2);
    }
}