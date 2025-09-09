namespace SurveyBasket.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService AuthService) : ControllerBase
{
    private readonly IAuthService _authService = AuthService;

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult is null? BadRequest("Incorrect Email or Password") : Ok(authResult);
    }
}
