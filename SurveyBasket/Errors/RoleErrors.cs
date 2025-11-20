namespace SurveyBasket.Errors;

public class RoleErrors
{
      public static readonly Error RoleNotFound =
        new("Role.NotFound", "Role is not found", StatusCodes.Status404NotFound);
}
