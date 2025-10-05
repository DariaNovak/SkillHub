using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Roles;

public class DeleteRoleDtoValidator : Validator<DeleteRoleDto>
{
    public DeleteRoleDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}