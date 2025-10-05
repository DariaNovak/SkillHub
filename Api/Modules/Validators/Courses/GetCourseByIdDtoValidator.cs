using Api.Dtos;
using FastEndpoints;
using FluentValidation;

namespace Api.Modules.Validators.Courses;

public class GetCourseByIdDtoValidator : Validator<GetCourseByIdDto>
{
    public GetCourseByIdDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}