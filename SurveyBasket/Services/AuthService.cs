using System.Security.Cryptography;

namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> UserManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = UserManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials); 

        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!IsValidPassword) return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials); 

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);   // generate new Jwt Token

        var refreshToken = GenerateRefreshToken();        // generate new Refresh Token
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
        user.RefreshTokens.Add(new RefreshToken 
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration 
        });
        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
        return Result.Succeed(response);
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null) return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
        if (userRefreshToken is null) return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokeedOn = DateTime.UtcNow;

        var (newJwtToken, jwtTokenexpiresIn) = _jwtProvider.GenerateToken(user);   // generate new Jwt Token

        var newRefreshToken = GenerateRefreshToken();    // generate new Refresh Token
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);

        var result = new AuthResponse
            (
                user.Id, user.Email, user.FirstName, user.LastName,
                newJwtToken, jwtTokenexpiresIn,
                newRefreshToken, refreshTokenExpiration
            );
        return Result.Succeed(result);
    }
    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null) return Result.Failure(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
        if (userRefreshToken is null) return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokeedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Succeed();
    }



    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

}
