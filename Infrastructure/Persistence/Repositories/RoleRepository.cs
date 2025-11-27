using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Domain.Roles;
using Domain.Roles.Role;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository, IRoleQueries
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;
        _context = context;
    }

    public async Task<Role> AddAsync(Role entity, CancellationToken cancellationToken)
    {
        await _context.Roles.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Option<IReadOnlyList<Role>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var roles = await _context.Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return roles ?? Option<IReadOnlyList<Role>>.None;
    }

    public async Task<Option<Role?>> GetByIdAsync(RoleId id, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return role ?? Option<Role?>.None;
    }

    public async Task UpdateAsync(Role entity, CancellationToken cancellationToken)
    {
        _context.Roles.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(id);
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);

        if (role is not null)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}