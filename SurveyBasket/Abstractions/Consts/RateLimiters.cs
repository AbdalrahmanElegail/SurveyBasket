namespace SurveyBasket.Abstractions.Consts;

public static class RateLimiters
{
    public const string UserLimiter = "userLimit";
    public const string IpLimiter = "ipLimit";
    public const string ConcurrencyLimiter = "concurrency";
    public const string TokenLimiter = "token";
    public const string FixedWindowLimiter = "fixedWindow";
    public const string SlidingWindowLimiter = "slidingWindow";
}