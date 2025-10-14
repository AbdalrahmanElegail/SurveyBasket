namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials = 
        new("User.InvalidCredentials", "Incorrect Email or Password", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken = 
        new("User.InvalidJwtToken", "Jwt token is not valid", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken = 
        new("User.InvalidRefreshToken", "Refresh token is not valid", StatusCodes.Status401Unauthorized);
}
