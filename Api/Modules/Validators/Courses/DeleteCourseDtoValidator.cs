using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Courses;

public class DeleteCourseDtoValidator : Validator<DeleteCourseDto>
{
    public DeleteCourseDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}