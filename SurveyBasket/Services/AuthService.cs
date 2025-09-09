using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> UserManager) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = UserManager;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) {return null; }

        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!IsValidPassword) { return null; }

        // Generate JWT token here (implementation depends on your JWT setup)
        

        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, "0k1l23k4l", 3600 );
    }
}
