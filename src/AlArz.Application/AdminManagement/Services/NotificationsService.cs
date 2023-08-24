using AdminManagement.Settings;
using AlArz.Dtos;
using AlArz.Interface;
using AlArz.Managers;
using Application.Contracts.EntityDto;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Application.Service
{
    public class NotificationsService : BasicService, INotificationsService
    {
        private readonly NotificationManager _NotificationManager;
        public NotificationsService(NotificationManager notificationManager)
        {
            _NotificationManager = notificationManager;
        }

        public async Task<NotificationsDto> CreateAsync(CreateNotificationDto input)
        {
            var mapping = ObjectMapper.Map<CreateNotificationDto, Notification>(input);

            var result = await _NotificationManager.Create(mapping);

            var NotificationsDto = ObjectMapper.Map<Notification, NotificationsDto>(result);

            return NotificationsDto;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _NotificationManager.DeleteAsync(id);
        }

        public async Task<List<NotificationsDto>> GetNotificationsToBeSent()
        {
            return ObjectMapper.Map<List<Notification>, List<NotificationsDto>>(await _NotificationManager.GetNotificationsToBeSent());
        }

        //public async Task SendNotification(string name, Guid? role, List<Guid> users, Guid user, object obj)
        //{
        //    await _NotificationManager.SendNotifiaction(name, role, users, user, obj);
        //}



        public async Task<List<NotificationsDto>> GetUnSentNotifications(Guid userId)
        {
            var lst = await _NotificationManager.GetNotificationByUserId(userId);
            return ObjectMapper.Map<List<Notification>, List<NotificationsDto>>(lst);
        }

        public async Task UpdateSendNotification(NotificationsDto notifications)
        {
            var obj = await _NotificationManager.GetAsync(notifications.Id);
            ObjectMapper.Map(notifications, obj);
            await _NotificationManager.UpdateSendNotification(obj);
        }

        public async Task<NotificationsDto> GetAsync(Guid id)
        {
            var result = await _NotificationManager.GetAsync(id);

            var mapperResult = ObjectMapper.Map<Notification, NotificationsDto>(result);

            return mapperResult;
        }

        public async Task<bool> UpdateIsReadToNotification(Guid id)
        {
            var result = await _NotificationManager.UpdateIsReadToNotification(id);
            return result;
        }

        public async Task<CustomPagedResultDto> GetByFilter(PagedSortedRequestDto input)
        {
            await NormalizeMaxResultCountAsync(input);

            var result = await _NotificationManager.GetByFilter(input);
            var countIsNotRead = await _NotificationManager.GetIsNotReadCount();
            var mapper = ObjectMapper.Map<List<Notification>, List<NotificationsDto>>(result.Items.ToList());

            return new CustomPagedResultDto { TotalCount = result.TotalCount, Items = mapper, CountIsNotRead = countIsNotRead };
        }
    }
}
