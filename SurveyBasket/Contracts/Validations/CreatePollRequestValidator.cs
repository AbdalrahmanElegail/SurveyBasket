
namespace SurveyBasket.Contracts.Validations;

public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator() 
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Please Add a {PropertyName}.")
            .Length(3,50).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters, You Entered: '{PropertyValue}'.");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Please Add a {PropertyName}.")
            .Length(3, 1000).WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters, You Entered: '{PropertyValue}'.");
    }
}
