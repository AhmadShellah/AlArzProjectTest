using AdminManagement;
using AlArz.Dtos;
using AlArz.Interface;
using AlArz.Managers;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Application.Service
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IIdentityUserAppService), typeof(IdentityUserAppService), typeof(UserService))]
    public class UserService : IdentityUserAppService, IUserService
    {
        private readonly UserManager _manager;

        public UserService(
             IdentityUserManager userManager, UserManager manager, IIdentityUserRepository userRepository, IIdentityRoleRepository roleRepository

            ) : base(
         userManager, userRepository, roleRepository, null
         )
        {
            _manager = manager;
        }
     
        [Authorize(AdminPermissions.Admins.User.Default)]
        public async Task<IdentityUserInfoDto> GetUser(Guid userId)
        {
            var user = await UserRepository.GetAsync(userId);

            var userRoles = await UserRepository.GetRolesAsync(userId);

            var userDto = ObjectMapper.Map<IdentityUser, IdentityUserInfoDto>(user);

            var userRolesDto = ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(userRoles);

            userDto.Roles = userRolesDto;

            return userDto;
        }

        public async Task<List<RegisterFireBaseDto>> GetActiveUserTokens(Guid userId)
        {
            var lst = await _manager.GetActiveUserTokens(userId);

            return ObjectMapper.Map<List<RegisterFireBase>, List<RegisterFireBaseDto>>(lst);
        }

        [Authorize]
        public async Task LogOut()
        {
            var TokenId = CurrentUser.FindClaimValue("token_id");
            await _manager.CreateBlockedUserTokenAsync(new BlockedUserToken { ToeknId = Guid.Parse(TokenId) });
            //await _manager.UnRegisterFireBaseToken(token);
        }
        public async Task RegisterFireBaseTokenAsync(string token)
        {
            await _manager.RegisterFireBaseTokenAsync(token);
        }

        [Authorize(AdminPermissions.Admins.User.ResetPassword)]
        public async Task ResetPassword(Guid userId, string newPassword)
        {
            var user = await UserRepository.GetAsync(userId);

            var token = await UserManager.GeneratePasswordResetTokenAsync(user);

            await UserManager.ResetPasswordAsync(user, token, newPassword);
        }

        [Authorize(AdminPermissions.Admins.User.ChangePassword)]
        public async Task ChangePassword(string CurrentPassword, string newPassword)
        {
            var user = await UserRepository.GetAsync(CurrentUser.Id.Value);

            var check = await UserManager.CheckPasswordAsync(user, CurrentPassword);
            if (!check)
            {
                throw new BusinessException(DomainErrorCodesAdminManagement.CurrentPasswordsIsIncorrect);
            }

            var result = await UserManager.ChangePasswordAsync(user, CurrentPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.JoinAsString(","));
            }
        }

        [Authorize(AdminPermissions.Admins.User.Create)]
        public async Task<IdentityUserInfoDto> CreateUserAsync(CreateIdentityUserInfoDto input)
        {
            input.IsActive = true;

            var mapperUser = ObjectMapper.Map<CreateIdentityUserInfoDto, IdentityUser>(input);

            var addUser = await UserManager.CreateAsync(mapperUser, input.Password);

            if (addUser.Succeeded)
            {
                await CurrentUnitOfWork.SaveChangesAsync();

                var userBefore = await UserRepository.FindByNormalizedEmailAsync(input.Email);

                await UserManager.SetLockoutEnabledAsync(userBefore, input.LockoutEnabled);

                var user = await UserRepository.FindByNormalizedEmailAsync(input.Email);

                var addRole = await UserManager.AddToRolesAsync(user, input.RoleNames);

                if (!addRole.Succeeded)
                {
                    throw new BusinessException(addRole.Errors.Select(c => c.Code).JoinAsString(","));
                }

                var userRoles = await UserRepository.GetRolesAsync(user.Id);

                var userInfo = ObjectMapper.Map<IdentityUser, IdentityUserInfoDto>(user);

                var userRolesDto = ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(userRoles);

                userInfo.Roles = userRolesDto;

                return userInfo;
            }
            else
            {
                var error = addUser.Errors.Select(c => c.Code).JoinAsString(",");
                throw new BusinessException(error);
            }
        }

        [Authorize(AdminPermissions.Admins.User.Update)]
        public async Task<IdentityUserInfoDto> UpdateUserAsync(UpdateIdentityUserInfoDto input)
        {

            var user = await UserManager.GetByIdAsync(input.UserId);

            if (!string.IsNullOrEmpty(input.UserName) && !user.UserName.ToLower().Equals(input.UserName.ToLower()))
                await UserManager.SetUserNameAsync(user, input.UserName);

            if (!string.IsNullOrEmpty(input.Email) && !user.Email.ToLower().Equals(input.Email.ToLower()))
                await UserManager.SetEmailAsync(user, input.Email);

            if (!string.IsNullOrEmpty(input.PhoneNumber) && !string.IsNullOrEmpty(user.PhoneNumber) && !user.PhoneNumber.ToLower().Equals(input.PhoneNumber.ToLower()))
                await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber);

            if (user.LockoutEnabled != input.LockoutEnabled)
                await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled);

            if (user.IsActive != input.IsActive)
                user.SetIsActive(input.IsActive);

            user.Name = input.Name;
            user.Surname = input.Surname;

            var updateUser = await UserManager.UpdateAsync(user);

            if (updateUser.Succeeded)
            {
                await CurrentUnitOfWork.SaveChangesAsync();

                var roles = await UserManager.SetRolesAsync(user, input.RoleNames);

                if (!roles.Succeeded)
                    throw new BusinessException(roles.Errors.Select(c => c.Code).JoinAsString(","));

                var userRoles = await UserRepository.GetRolesAsync(user.Id);

                var userInfo = ObjectMapper.Map<IdentityUser, IdentityUserInfoDto>(user);

                var userRolesDto = ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(userRoles);

                userInfo.Roles = userRolesDto;

                return userInfo;
            }
            else
            {
                var error = updateUser.Errors.Select(c => c.Code).JoinAsString(",");

                throw new BusinessException(error);
            }

        }

        public async Task<string> GetUserName(Guid userId)
        {
            return (await UserManager.GetByIdAsync(userId)).UserName;
        }

        [Authorize]
        public async Task<List<DDLUsersDto>> GetAllUsers()
        {
            var lstOfUsers = await UserRepository.GetListAsync();

            return lstOfUsers.Select(c => new DDLUsersDto
            {
                Id = c.Id,
                UserName = c.UserName,
                SurName = c.Surname
            }).ToList();
        }


    }
}
