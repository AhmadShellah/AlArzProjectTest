using AdminManagement.Settings;
using AlArz.AdminManagement;
using AlArz.Dtos;
using AlArz.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace AlArz.Services
{
    public class PermissionService : BasicService, IPermissionService, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IPermissionGrantRepository _permissionGrantRepository;
        public PermissionService(IPermissionManager permissionManager, IIdentityUserRepository userRepository
            , IPermissionGrantRepository permissionGrantRepository
)
        {
            _permissionManager = permissionManager;
            _userRepository = userRepository;
            _permissionGrantRepository = permissionGrantRepository;
        }

        public async Task<IEnumerable<string>> GetAllForUserAsync(Guid userId, List<string> Role)
        {
            var listOfPermission = new List<string>();

            foreach (var item in Role)
            {
                var permission = await _permissionGrantRepository.GetListAsync("R", item);

                listOfPermission.AddRange(permission.Select(c => c.Name).ToList());
            }

            return listOfPermission;
        }

        public async Task<UserInfo> GetAllForCurrentUserAsync(string userId)
        {
            Guid _userId = Guid.Parse(userId);

            var userRole = await _userRepository.GetRoleNamesAsync(_userId);

            var permissions = await GetAllForUserAsync(_userId, userRole);

            var userInfo = await _userRepository.GetAsync(_userId);

            var userInfoDto = ObjectMapper.Map<IdentityUser, IdentityUserInfoDto>(userInfo);

            var isAdmin = userRole.Contains(Admin.admin);

            return new UserInfo { Permissions = permissions, User = userInfoDto, IsAdmin = isAdmin };
        }

        
    }
}
