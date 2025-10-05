using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Lessons;

public class DeleteLessonDtoValidator : Validator<DeleteLessonDto>
{
    public DeleteLessonDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}