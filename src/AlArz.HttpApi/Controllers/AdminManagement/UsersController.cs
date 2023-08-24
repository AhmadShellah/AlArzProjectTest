using AlArz.Dtos;
using AlArz.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace AlArz.Controllers.AdminManagement
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IdentityUserController), IncludeSelf = true)]
    [RemoteService(true, Name = "AbpIdentity")]
    [Area("identity")]
    [ControllerName("User")]
    [Route("api/identity/users")]
    public class UsersController : IdentityUserController
    {
        private readonly IUserService _identityAppService;

        public UsersController(IIdentityUserAppService identityAppService)
    : base(identityAppService)
        {
            _identityAppService = (IUserService)identityAppService;
        }

        [HttpGet("GetUser")]
        public async Task<IdentityUserInfoDto> GetUser(Guid userId)
        {
            return await _identityAppService.GetUser(userId);
        }

        [HttpGet("LogOut")]
        public async Task LogOut()
        {
            await _identityAppService.LogOut();
        }

        [HttpPost("RegisterFireBaseToken")]
        public async Task RegisterFireBaseToken([FromBody] string token)
        {
            await _identityAppService.RegisterFireBaseTokenAsync(token);
        }
        [HttpPatch("ResetPassword")]
        public async Task ResetPassword(Guid userId, string newPassword)
        {
            await _identityAppService.ResetPassword(userId, newPassword);
        }
        [HttpPatch("ChangePassword")]
        public async Task ChangePassword(string CurrnetPassword, string newPassword)
        {
            await _identityAppService.ChangePassword(CurrnetPassword, newPassword);
        }
        [HttpPut("UpdateUser")]
        public async Task<IdentityUserInfoDto> UpdateUserAsync(UpdateIdentityUserInfoDto input)
        {
            return await _identityAppService.UpdateUserAsync(input);
        }
        [HttpPost("CreateUser")]
        public async Task<IdentityUserInfoDto> CreateUserAsync(CreateIdentityUserInfoDto input)
        {
            return await _identityAppService.CreateUserAsync(input);
        }
        [HttpGet("GetAllUsers")]
        public async Task<List<DDLUsersDto>> GetAllUsers()
        {
            return await _identityAppService.GetAllUsers();
        }

    }
}
