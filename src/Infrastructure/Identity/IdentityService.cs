using RendezVous.Application.Common.Interfaces;
using RendezVous.Application.Common.Models;

namespace RendezVous.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    public async Task<string> GetUserNameAsync(Guid userId)
    {
        throw new NotImplementedException();

        // var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        // return user.UserName;
    }

    Task<(Result Result, Guid UserId)> IIdentityService.CreateUserAsync(string userName, string password)
    {
        throw new NotImplementedException();

        // var user = new ApplicationUser
        // {
        //     UserName = userName,
        //     Email = userName,
        // };

        // var result = await _userManager.CreateAsync(user, password);

        // return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        throw new NotImplementedException();

        // var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        // return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
        throw new NotImplementedException();

        // var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        // if (user == null)
        // {
        //     return false;
        // }

        // var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        // var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        // return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(Guid userId)
    {
        throw new NotImplementedException();

        // var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        // return user != null ? await DeleteUserAsync(user) : Result.Success();
    }
}
