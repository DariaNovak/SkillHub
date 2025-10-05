using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Lessons;

public class GetLessonByIdDtoValidator : Validator<GetLessonByIdDto>
{
    public GetLessonByIdDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}