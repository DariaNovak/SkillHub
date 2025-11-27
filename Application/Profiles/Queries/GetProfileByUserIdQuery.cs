using Application.Common.Interfaces.Queries;
using Domain.Profiles;
using LanguageExt;
using MediatR;

namespace Application.Profiles.Queries;

public record GetProfileByUserIdQuery : IRequest<Option<Profile>>
{
    public required Guid UserId { get; init; }
}

public class GetProfileByUserIdQueryHandler(IProfileQueries profileQueries)
    : IRequestHandler<GetProfileByUserIdQuery, Option<Profile>>
{
    public async Task<Option<Profile>> Handle(
        GetProfileByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        return await profileQueries.GetByUserIdAsync(request.UserId, cancellationToken);
    }
}
