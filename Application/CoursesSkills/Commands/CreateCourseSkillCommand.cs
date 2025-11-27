using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.CoursesSkills.Exceptions;
using Domain.CoursesSkills;
using Domain.Courses;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.CoursesSkills.Commands;

public record CreateCourseSkillCommand : IRequest<Either<CourseSkillException, CourseSkill>>
{
    public required CourseId CourseId { get; init; }
    public required SkillId SkillId { get; init; }
}

public class CreateCourseSkillCommandHandler(
    ICourseSkillQueries courseSkillQueries,
    ICourseSkillRepository courseSkillRepository,
    ICourseQueries courseQueries,
    ISkillQueries skillQueries)
    : IRequestHandler<CreateCourseSkillCommand, Either<CourseSkillException, CourseSkill>>
{
    public async Task<Either<CourseSkillException, CourseSkill>> Handle(
        CreateCourseSkillCommand request,
        CancellationToken cancellationToken)
    {
        var existingCourseSkill = await courseSkillQueries.GetByIdAsync(request.CourseId, request.SkillId, cancellationToken);

        return await existingCourseSkill.MatchAsync(
            cs => new CourseSkillAlreadyExistsException(request.CourseId, request.SkillId),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<CourseSkillException, CourseSkill>> CreateEntity(
        CreateCourseSkillCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var course = await courseQueries.GetByIdAsync(request.CourseId, cancellationToken);
            var skill = await skillQueries.GetByIdAsync(request.SkillId, cancellationToken);

            if (course.IsNone || skill.IsNone)
            {
                return new UnhandledCourseSkillException(
                    request.CourseId, 
                    request.SkillId, 
                    new Exception("Course or Skill not found"));
            }

            var courseSkill = await courseSkillRepository.AddAsync(
                CourseSkill.New(course.First(), skill.First()),
                cancellationToken);

            return courseSkill;
        }
        catch (Exception exception)
        {
            return new UnhandledCourseSkillException(request.CourseId, request.SkillId, exception);
        }
    }
}
