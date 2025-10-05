using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Users;

public class UpdateUserDtoValidator : Validator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PasswordHash)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.JoinDate)
            .NotEmpty();
    }
}