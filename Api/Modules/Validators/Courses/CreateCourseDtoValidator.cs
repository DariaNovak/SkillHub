using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Courses;

public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.AuthorId)
            .NotNull();

        RuleFor(x => x.CreatedAt)
            .NotEmpty();
    }
}