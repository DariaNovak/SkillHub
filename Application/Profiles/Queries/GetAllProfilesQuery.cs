using Application.Common.Interfaces.Queries;
using Domain.Profiles;
using MediatR;

namespace Application.Profiles.Queries;

public record GetAllProfilesQuery : IRequest<IReadOnlyList<Profile>>;

public class GetAllProfilesQueryHandler(IProfileQueries profileQueries)
    : IRequestHandler<GetAllProfilesQuery, IReadOnlyList<Profile>>
{
    public async Task<IReadOnlyList<Profile>> Handle(
        GetAllProfilesQuery request,
        CancellationToken cancellationToken)
    {
        return await profileQueries.GetAllAsync(cancellationToken);
    }
}
