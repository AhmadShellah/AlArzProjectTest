
using AlArz.Dtos;
using AutoMapper;
using Domain.Entity;
using Volo.Abp.Identity;

namespace AlArz.AdminManagement.MappingProfiles
{
    public class AdminMapper : Profile
    {
        public AdminMapper()
        {
            CreateMap<Config, ConfigDto>().ReverseMap();
            CreateMap<Config, CreateConfigDto>().ReverseMap();
            CreateMap<Config, UpdateConfigDto>().ReverseMap();


            CreateMap<Lookup, LookupDto>().ReverseMap();
            CreateMap<Lookup, CreateLookupDto>().ReverseMap();
            CreateMap<Lookup, UpdateLookupDto>().ReverseMap();

            CreateMap<LookupType, LookupTypeDto>().ReverseMap();
            CreateMap<CreateLookupTypeDto, LookupType>().ForMember(x => x.LookupTypeNumber, x => x.Ignore()).ReverseMap();
            CreateMap<LookupType, UpdateLookupTypeDto>().ReverseMap();

            CreateMap<Notification, NotificationsDto>().ReverseMap();
            CreateMap<Notification, CreateNotificationDto>().ReverseMap();

            CreateMap<NotificationTemplate, NotificationTemplateDto>().ReverseMap();
            CreateMap<NotificationTemplate, CreateNotificationTemplateDto>().ReverseMap();

            CreateMap<Attachment, AttachmentDto>().ReverseMap();
            CreateMap<Attachment, CreateAttachmentDto>().ReverseMap();
            CreateMap<AttachmentDto, CreateAttachmentDto>().ReverseMap();
            CreateMap<Attachment, UpdateAttachmentDto>().ReverseMap();
            CreateMap<AttachmentDto, UpdateAttachmentDto>().ReverseMap();


            CreateMap<CreateIdentityUserInfoDto, IdentityUser>();
            CreateMap<IdentityUser, CreateIdentityUserInfoDto>()
                .ForMember(x => x.RoleNames, x => x.Ignore());

            CreateMap<IdentityUserUpdateDto, UpdateIdentityUserInfoDto>().ReverseMap();
            CreateMap<IdentityUserUpdateDto, IdentityUserDto>().ReverseMap();
            CreateMap<IdentityUser, IdentityUserInfoDto>().ForMember(c=>c.Roles, s=>s.Ignore()).ReverseMap();

            CreateMap<IdentityUser, IdentityUserDto>().ReverseMap();
            CreateMap<IdentityUser, IdentityUserUpdateDto>().ReverseMap();
            CreateMap<RegisterFireBase, RegisterFireBaseDto>().ReverseMap();


        }
    }
}
