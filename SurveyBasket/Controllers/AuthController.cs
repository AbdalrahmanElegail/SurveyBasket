using SurveyBasket.Abstractions;

namespace SurveyBasket.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService AuthService) : ControllerBase
{
    private readonly IAuthService _authService = AuthService;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSucceed
            ? Ok(authResult.Value)
            : Problem(statusCode: StatusCodes.Status400BadRequest, title: authResult.Error.Code, detail: authResult.Error.Description);
    }

    //[HttpPost("refresh")]
    //public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    //{
    //    var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

    //    return authResult is not null 
    //        ? Ok(authResult)
    //        : Problem(statusCode: StatusCodes.Status400BadRequest, title: authResult.Error.Code, detail: authResult.Error.Description);
    //}
    //[HttpPost("revoke-refresh-token")]
    //public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    //{
    //    var authResult = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

    //    return authResult ? Ok() : BadRequest("Operation Failed.");
    //}
}
