namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) 
        => Ok(await _userService.GetAllAsync(cancellationToken));

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> Get([FromRoute]string id)
    {
        var result = await _userService.GetAsync(id);
        return result.IsSucceed ? Ok(result.Value) : result.ToProblem();
    }
}