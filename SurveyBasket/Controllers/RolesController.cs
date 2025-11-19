namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet("")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> GetRoles([FromQuery] bool IncludeDisabled, CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetAllAsync(IncludeDisabled, cancellationToken);

        return Ok(roles);
    }
}