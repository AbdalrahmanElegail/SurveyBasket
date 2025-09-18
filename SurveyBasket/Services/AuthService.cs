using System.Security.Cryptography;

namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> UserManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = UserManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) {return null; }

        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!IsValidPassword) { return null; }

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);   // generate new Jwt Token

        var refreshToken = GenerateRefreshToken();        // generate new Refresh Token
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
        user.RefreshTokens.Add(new RefreshToken 
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration 
        });
        await _userManager.UpdateAsync(user);

        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
    }

    public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null) return null;

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return null;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
        if (userRefreshToken is null) return null;

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

        return new AuthResponse
            (
                user.Id, user.Email, user.FirstName, user.LastName,
                newJwtToken, jwtTokenexpiresIn,
                newRefreshToken, refreshTokenExpiration
            );
    }
    public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null) return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return false;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
        if (userRefreshToken is null) return false;

        userRefreshToken.RevokeedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return true;
    }



    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

}
