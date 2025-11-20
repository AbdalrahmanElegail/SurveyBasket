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

    public async Task<Result<RoleDetailsResponse>> GetAsync(string id)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleNotFound);

        var permissions = await _roleManager.GetClaimsAsync(role);

        var response = new RoleDetailsResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(p => p.Value));

        return Result.Succeed(response);
    }
}
