using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Roles;

public class GetRoleByIdDtoValidator : Validator<GetRoleByIdDto>
{
    public GetRoleByIdDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}