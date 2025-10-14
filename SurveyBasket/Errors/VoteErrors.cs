namespace SurveyBasket.Errors;

public static class VoteErrors
{
    //public static readonly Error VoteNotFound =
    //    new("Vote.NotFound", "There is no vote with this ID", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedVote =
        new("Vote.Duplicated", "This user Has Voted on the same poll before", StatusCodes.Status409Conflict);

    public static readonly Error InvalidQuestions =
        new("Vote.InvalidQuestions", "Invalid Questions", StatusCodes.Status400BadRequest);
}