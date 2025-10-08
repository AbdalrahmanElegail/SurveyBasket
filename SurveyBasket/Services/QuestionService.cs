using Azure.Core;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Errors;

namespace SurveyBasket.Services;

public class QuestionService(ApplicationDbContext context) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken: cancellationToken);
        if (!pollExists) return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);

        var questions = await _context.Questions
            .Where(q => q.PollId == pollId)
            .Include(q => q.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Succeed<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .Where(q => q.PollId == pollId && q.Id == id)
            .Include(q => q.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (question is null) return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);

        return Result.Succeed(question);
    }

    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var pollExists = await _context.Polls.AnyAsync(q => q.Id == pollId, cancellationToken: cancellationToken);
        if (!pollExists) return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

        var questionIsDuplicated = await _context.Questions.AnyAsync(q => q.Content == request.Content && q.PollId == pollId, cancellationToken: cancellationToken);
        if (questionIsDuplicated) return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

        var question = request.Adapt<Question>();
        question.PollId = pollId;

        await _context.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Succeed(question.Adapt<QuestionResponse>());
    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .Include(q => q.Answers)
            .SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == id, cancellationToken);
        if (question is null) return Result.Failure(QuestionErrors.QuestionNotFound);

        var questionIsDuplicated = await _context.Questions
            .AnyAsync(q => q.Id != id
                && q.PollId == pollId 
                && q.Content == request.Content,
                cancellationToken: cancellationToken
            );
        if (questionIsDuplicated) return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

        question.Content = request.Content; 
        foreach (var answer in question.Answers)  answer.IsActive = false;    // Deactivate old Answers 

        foreach (var answer in request.Answers) 
        {
            var existingAnswer = question.Answers.SingleOrDefault(a => a.Content == answer);

            if(existingAnswer is null)
                question.Answers.Add(new Answer { Content = answer });   // if answer doesn't exist in current answers, add it
            else existingAnswer.IsActive = true;     // if answer exists, activate it
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Succeed();
    }

    public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions.SingleOrDefaultAsync(q => q.Id == id && q.PollId == pollId, cancellationToken);
        if (question is null) return Result.Failure(QuestionErrors.QuestionNotFound);

        question.IsActive = !question.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Succeed();
    }

}
