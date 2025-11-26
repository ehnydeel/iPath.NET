
using Microsoft.EntityFrameworkCore.Metadata;

namespace iPath.EF.Core.FeatureHandlers.Users;

public class GetRolesQueryHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<GetRolesQuery, Task<IEnumerable<RoleDto>>>
{
    public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await db.Roles.AsNoTracking().ToListAsync(cancellationToken);
        var dtos = roles.Select(r => new RoleDto(r.Id, r.Name)).ToArray();
        return dtos;
    }
}