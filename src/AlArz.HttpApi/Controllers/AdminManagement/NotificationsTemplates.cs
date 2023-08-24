using AlArz.Dtos;
using AlArz.Interface;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace AlArz.Controllers.AdminManagement
{

    [Route("api/AdminManagement/[controller]/[action]")]
    public class NotificationsTemplates : AbpController, INotificationTemplate
    {
        private readonly INotificationTemplate _notificationService;
        public NotificationsTemplates(INotificationTemplate notificationService)
        {
            _notificationService = notificationService;

        }

        [HttpPost]

        public async Task<NotificationTemplateDto> CreateAsync(CreateNotificationTemplateDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _notificationService.CreateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _notificationService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<NotificationTemplateDto> GetAsync(Guid id)
        {
            return await _notificationService.GetAsync(id);
        }

        [HttpPut]

        public async Task<NotificationTemplateDto> UpdateAsync(UpdateNotificationTemplateDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _notificationService.UpdateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }
    }
}
