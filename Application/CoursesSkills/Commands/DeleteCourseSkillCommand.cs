using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.CoursesSkills.Exceptions;
using Domain.Courses;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.CoursesSkills.Commands;

public record DeleteCourseSkillCommand : IRequest<Either<CourseSkillException, bool>>
{
    public required CourseId CourseId { get; init; }
    public required SkillId SkillId { get; init; }
}

public class DeleteCourseSkillCommandHandler(
    ICourseSkillQueries courseSkillQueries,
    ICourseSkillRepository courseSkillRepository)
    : IRequestHandler<DeleteCourseSkillCommand, Either<CourseSkillException, bool>>
{
    public async Task<Either<CourseSkillException, bool>> Handle(
        DeleteCourseSkillCommand request,
        CancellationToken cancellationToken)
    {
        var existingCourseSkill = await courseSkillQueries.GetByIdAsync(request.CourseId, request.SkillId, cancellationToken);

        return await existingCourseSkill.MatchAsync(
            cs => DeleteEntity(request, cancellationToken),
            () => Task.FromResult<Either<CourseSkillException, bool>>(
                new CourseSkillNotFoundException(request.CourseId, request.SkillId)));
    }

    private async Task<Either<CourseSkillException, bool>> DeleteEntity(
        DeleteCourseSkillCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await courseSkillRepository.DeleteAsync(request.CourseId, request.SkillId, cancellationToken);
            return true;
        }
        catch (Exception exception)
        {
            return new UnhandledCourseSkillException(request.CourseId, request.SkillId, exception);
        }
    }
}
