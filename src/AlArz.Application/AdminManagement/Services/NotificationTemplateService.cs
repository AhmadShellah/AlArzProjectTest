using AlArz.Dtos;
using AlArz.Interface;
using AlArz.Managers;
using Application.Exceptions;
using Domain.Entity;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AlArz.AdminManagement.Services
{
    //[Authorize(AdminPermissions.Admins.Default)]
    public class NotificationTemplateService : ApplicationService, INotificationTemplate
    {
        private readonly NotificationTemplateManger _notificationsManager;
        public NotificationTemplateService(NotificationTemplateManger notificationsManager)
        {
            _notificationsManager = notificationsManager;
        }

        //[Authorize(AdminPermissions.Admins.Create)]
        public async Task<NotificationTemplateDto> CreateAsync(CreateNotificationTemplateDto input)
        {
            var mapping = ObjectMapper.Map<CreateNotificationTemplateDto, NotificationTemplate>(input);

            var result = await _notificationsManager.Create(mapping);

            var finalResult = ObjectMapper.Map<NotificationTemplate, NotificationTemplateDto>(result);

            return finalResult;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _notificationsManager.DeleteAsync(id);
        }

        public async Task<NotificationTemplateDto> GetAsync(Guid id)
        {
            var result = await _notificationsManager.GetAsync(id);

            return ObjectMapper.Map<NotificationTemplate, NotificationTemplateDto>(result);
        }

        //[Authorize(AdminPermissions.Admins.Update)]
        public async Task<NotificationTemplateDto> UpdateAsync(UpdateNotificationTemplateDto input)
        {
            var result = await _notificationsManager.GetAsync(input.Id);
            if (result == null)
            {
                throw new NotUpdateException();
            }
            ObjectMapper.Map(input, result);

            var final = await _notificationsManager.Update(result);


            return ObjectMapper.Map<NotificationTemplate, NotificationTemplateDto>(final);
        }

    }
}
