using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sibers.Core.Entities;
using System.Security.Claims;

namespace Sibers.MVC.Initializers;

public class AvatarUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole<int>>
{
    private const string AVATAR_CLAIM_NAME = "Avatar";
    public AvatarUserClaimsPrincipalFactory(
        UserManager<User> userManager, 
        RoleManager<IdentityRole<int>> roleManager, 
        IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        if (!string.IsNullOrEmpty(user.Avatar))
            identity.AddClaim(new Claim(AVATAR_CLAIM_NAME, user.Avatar));

        return identity;
    }
}
