using AlArz.Dtos;
using AlArz.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace AlArz.AdminManagement
{

    [Route("api/AdminManagement/[controller]/[action]")]
    public class PermissionController : AbpController, IPermissionService
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        [HttpGet]
        public async Task<UserInfo> GetAllForCurrentUserAsync(string userId)
        {
            return await _permissionService.GetAllForCurrentUserAsync(userId);
        }


    }
}
