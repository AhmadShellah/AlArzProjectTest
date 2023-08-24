using System;
using Volo.Abp.Identity;

namespace AlArz.Dtos
{
    public class UpdateIdentityUserInfoDto : IdentityUserUpdateDto
    {
        public Guid UserId { get; set; }
    }
    public class CreateIdentityUserInfoDto : IdentityUserCreateDto
    {
       
    }
}
