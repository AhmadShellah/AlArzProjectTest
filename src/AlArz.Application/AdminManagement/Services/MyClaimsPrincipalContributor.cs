using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace AlArz.AdminManagement.Services
{
    public class MyClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
    {
        public async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
        {
            var identity = context.ClaimsPrincipal.Identity as ClaimsIdentity;

            var userId = identity?.FindUserId();

            if (userId.HasValue)
            {
                identity.AddClaim(new Claim("token_id", Guid.NewGuid().ToString()));
                identity.AddClaim(new Claim("lang", "ar"));
            }
        }

    }
}
