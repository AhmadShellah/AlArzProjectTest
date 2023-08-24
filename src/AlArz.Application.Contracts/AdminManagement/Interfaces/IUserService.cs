using AlArz.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace AlArz.Interface
{
    public interface IUserService : IApplicationService
    {
        Task LogOut();
        Task RegisterFireBaseTokenAsync(string token);
        Task ResetPassword(Guid userId, string newPassword);
        Task ChangePassword(string CurrnetPassword, string newPassword);
        Task<IdentityUserInfoDto> CreateUserAsync(CreateIdentityUserInfoDto input);
        Task<IdentityUserInfoDto> UpdateUserAsync(UpdateIdentityUserInfoDto input);
        Task<IdentityUserInfoDto> GetUser(Guid userId);
        Task<List<DDLUsersDto>> GetAllUsers();
    }
}
