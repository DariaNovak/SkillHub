using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Skills;

public class DeleteSkillDtoValidator : Validator<DeleteSkillDto>
{
    public DeleteSkillDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}