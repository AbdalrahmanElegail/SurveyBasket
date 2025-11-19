using SurveyBasket.Contracts.Roles;

namespace SurveyBasket.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? IncludeDisabled = false, CancellationToken cancellationToken = default)=>
        await _roleManager.Roles
            .Where(role => !role.IsDefault && (!role.IsDeleted || IncludeDisabled.HasValue && IncludeDisabled.Value))
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);
}
