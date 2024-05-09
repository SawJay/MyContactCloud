using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyContactCloud.Client;
using MyContactCloud.Data;
using MyContactCloud.Helpers;
using System.Security.Claims;

namespace MyContactCloud.Components.Account
{
    public class CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> options)
                                                           : UserClaimsPrincipalFactory<ApplicationUser>(userManager, options)
    {

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);

            string profilePictureUrl = user.ImageId.HasValue ? $"/api/uploads/{user.ImageId}" : UploadHelper.DefaultProfilePicture;

            List<Claim> customClaims =
            [
                new Claim(nameof(UserInfo.FirstName), user.FirstName!),
                new Claim(nameof(UserInfo.LastName), user.LastName!),
                new Claim(nameof(UserInfo.ProfilePictureUrl), profilePictureUrl!),
            ];

            identity.AddClaims(customClaims);

            return identity;
        }

    }
}
