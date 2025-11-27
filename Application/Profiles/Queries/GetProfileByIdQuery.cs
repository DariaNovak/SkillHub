using Application.Common.Interfaces.Queries;
using Domain.Profiles;
using LanguageExt;
using MediatR;

namespace Application.Profiles.Queries;

public record GetProfileByIdQuery : IRequest<Option<Profile>>
{
    public required Guid ProfileId { get; init; }
}

public class GetProfileByIdQueryHandler(IProfileQueries profileQueries)
    : IRequestHandler<GetProfileByIdQuery, Option<Profile>>
{
    public async Task<Option<Profile>> Handle(
        GetProfileByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await profileQueries.GetByIdAsync(new ProfileId(request.ProfileId), cancellationToken);
    }
}
