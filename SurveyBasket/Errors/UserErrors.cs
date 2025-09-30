using Microsoft.EntityFrameworkCore.Query.Internal;

namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Incorrect Email or Password");
    public static readonly Error InvalidToken = new("User.InvalidToken", "Token is not valid");
}
