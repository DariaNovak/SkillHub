using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Users;

public class GetUserByIdDtoValidator : Validator<GetUserByIdDto>
{
    public GetUserByIdDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}