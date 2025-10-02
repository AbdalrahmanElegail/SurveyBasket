namespace SurveyBasket.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound = 
        new("Poll.NotFound", "There is no poll with this ID", StatusCodes.Status404NotFound);
}