using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Lessons;

public class UpdateLessonDtoValidator : Validator<UpdateLessonDto>
{
    public UpdateLessonDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Content)
            .NotEmpty();

        RuleFor(x => x.Order)
            .GreaterThan(0);
    }
}