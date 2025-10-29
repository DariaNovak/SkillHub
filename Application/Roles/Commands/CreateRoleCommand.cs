using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Roles;
using Domain.Roles.Role;
using LanguageExt;
using MediatR;

namespace Application.Roles.Commands
{
    public record CreateRoleCommand : IRequest<Either<RoleException, Role>>
    {
        public required RoleId Id { get; init; }
        public required string Name { get; init; }
    }

    public class CreateRoleCommandHandler(
        IRoleQueries roleQueries,
        IRoleRepository roleRepository)
        : IRequestHandler<CreateRoleCommand, Either<RoleException, Role>>
    {
        public async Task<Either<RoleException, Role>> Handle(
            CreateRoleCommand request,
            CancellationToken cancellationToken)
        {
            var existingRole = await roleQueries.GetByIdAsync(request.Id, cancellationToken);

            return await existingRole.MatchAsync(
                r => new RoleAlreadyExistsException(r.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<RoleException, Role>> CreateEntity(
            CreateRoleCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var role = await roleRepository.AddAsync(
                    Role.New(request.Name),
                    cancellationToken);

                return role;
            }
            catch (Exception exception)
            {
                return new UnhandledRoleException(request.Id, exception);
            }
        }
    }
}