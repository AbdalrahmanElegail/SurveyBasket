using SurveyBasket.Contracts.Users;

namespace SurveyBasket.Services;

public class UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => await (from u in _context.Users
                  join ur in _context.UserRoles on u.Id equals ur.UserId
                  join r in _context.Roles on ur.RoleId equals r.Id
                  into userRoles
                  where !userRoles.Any(ur => ur.Name == DefaultRoles.Member)
                  select new
                  {
                      u.Id,
                      u.FirstName,
                      u.LastName,
                      u.Email,
                      u.IsDisabled,
                      Roles = userRoles.Select(ur => ur.Name!).ToList()
                  })
                  .GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.IsDisabled })
                  .Select(u => new UserResponse
                  (
                      u.Key.Id,
                      u.Key.FirstName,
                      u.Key.LastName,
                      u.Key.Email,
                      u.Key.IsDisabled,
                      u.SelectMany(u => u.Roles)
                  )).ToListAsync(cancellationToken);

    public async Task<Result<UserResponse>> GetAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var userRoles = await _userManager.GetRolesAsync(user);

        var response = (user, userRoles).Adapt<UserResponse>();

        return Result.Succeed(response);
    }

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId)
            .ProjectToType<UserProfileResponse>()
            .SingleAsync();

        return Result.Succeed(user);
    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        await _userManager.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.FirstName, request.FirstName)
                .SetProperty(u => u.LastName, request.LastName)
            );

        return Result.Succeed();
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return Result.Succeed();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
}
