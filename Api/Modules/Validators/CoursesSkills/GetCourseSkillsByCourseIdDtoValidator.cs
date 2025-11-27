using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.CoursesSkills;

public class GetCourseSkillsByCourseIdDtoValidator : AbstractValidator<GetCourseSkillsByCourseIdDto>
{
    public GetCourseSkillsByCourseIdDtoValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("CourseId is required");
    }
}
