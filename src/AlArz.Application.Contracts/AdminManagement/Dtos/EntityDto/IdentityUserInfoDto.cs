using System;
using System.Collections.Generic;
using Volo.Abp.Identity;

namespace AlArz.Dtos
{
    public class IdentityUserInfoDto : IdentityUserDto
    {
        public List<IdentityRoleDto> Roles { get; set; } = new List<IdentityRoleDto>();
    }


    public class DDLUsersDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
    }
    public class UserInfo
    {
        public IEnumerable<string> Permissions { get; set; } = new List<string>();
        public IdentityUserInfoDto User { get; set; } = new IdentityUserInfoDto();
        public bool IsAdmin { get; set; }
    }


}
