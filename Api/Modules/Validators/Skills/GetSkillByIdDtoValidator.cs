using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Skills;

public class GetSkillByIdDtoValidator : Validator<GetSkillByIdDto>
{
    public GetSkillByIdDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}