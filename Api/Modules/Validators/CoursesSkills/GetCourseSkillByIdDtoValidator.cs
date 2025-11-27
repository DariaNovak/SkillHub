using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.CoursesSkills;

public class GetCourseSkillByIdDtoValidator : AbstractValidator<GetCourseSkillByIdDto>
{
    public GetCourseSkillByIdDtoValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("CourseId is required");

        RuleFor(x => x.SkillId)
            .NotEmpty()
            .WithMessage("SkillId is required");
    }
}
