using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Courses;

public class UpdateCourseDtoValidator : Validator<UpdateCourseDto>
{
    public UpdateCourseDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(10);
    }
}