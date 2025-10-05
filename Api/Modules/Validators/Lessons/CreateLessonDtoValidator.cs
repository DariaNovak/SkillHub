using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Lessons;

public class CreateLessonDtoValidator : AbstractValidator<CreateLessonDto>
{
    public CreateLessonDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.CourseId)
            .NotNull();

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0);
    }
}