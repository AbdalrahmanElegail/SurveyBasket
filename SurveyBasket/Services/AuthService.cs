﻿namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> UserManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = UserManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) {return null; }

        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!IsValidPassword) { return null; }

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);
    }
}
