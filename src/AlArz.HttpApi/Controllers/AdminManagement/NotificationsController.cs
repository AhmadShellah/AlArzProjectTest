using AlArz.Dtos;
using AlArz.Interface;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace AlArz.Controllers.AdminManagement
{
    [Route("api/AdminManagement/[controller]/[action]")]
    public class NotificationsController : AbpController, INotificationsService
    {
        private readonly INotificationsService _NotificationsService;
        public NotificationsController(INotificationsService NotificationsService)
        {
            _NotificationsService = NotificationsService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<NotificationsDto> GetAsync(Guid id)
        {
            var result = await _NotificationsService.GetAsync(id);
            return result;
        }

        [HttpPut]
        public async Task<bool> UpdateIsReadToNotification(Guid id)
        {
            var result = await _NotificationsService.UpdateIsReadToNotification(id);
            return result;
        }

        [HttpDelete]
        public async Task DeleteAsync(Guid id)
        {
            await _NotificationsService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<NotificationsDto> CreateAsync(CreateNotificationDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _NotificationsService.CreateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpGet]
        public async Task<CustomPagedResultDto> GetByFilter(PagedSortedRequestDto input)
        {
            return await _NotificationsService.GetByFilter(input);
        }

    }
}
