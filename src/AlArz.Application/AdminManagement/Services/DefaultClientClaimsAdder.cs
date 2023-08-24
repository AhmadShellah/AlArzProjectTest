using AlArz.Interfaces;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
namespace AlArz.AdminManagement.Services
{
    public class DefaultClientClaimsAdder : ApplicationService, ICustomTokenRequestValidator
    {
        public async Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            var userId = context.Result.ValidatedRequest.Subject.FindFirst("Sub")?.Value;

            var service1 = LazyServiceProvider.LazyGetRequiredService<IPermissionService>();

            var userInfo = await service1.GetAllForCurrentUserAsync(userId!);

            context.Result.CustomResponse = new Dictionary<string, object>
            {
                {"UserInfo", userInfo.User  },
                {"Permissions",userInfo.Permissions } ,
                { "IsAdmin", userInfo.IsAdmin}

            };


        }
    }
}
