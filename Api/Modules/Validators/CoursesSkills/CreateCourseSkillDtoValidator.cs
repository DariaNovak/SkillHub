using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.CoursesSkills;

public class CreateCourseSkillDtoValidator : AbstractValidator<CreateCourseSkillDto>
{
    public CreateCourseSkillDtoValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("CourseId is required");

        RuleFor(x => x.SkillId)
            .NotEmpty()
            .WithMessage("SkillId is required");
    }
}
