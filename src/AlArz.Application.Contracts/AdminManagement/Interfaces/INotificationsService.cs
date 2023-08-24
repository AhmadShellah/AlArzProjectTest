using AlArz.Dtos;
using Application.Contracts.EntityDto;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AlArz.Interface
{
    public interface INotificationsService : IApplicationService
    {
        Task<NotificationsDto> GetAsync(Guid id);

        Task<bool> UpdateIsReadToNotification(Guid id);

        Task<NotificationsDto> CreateAsync(CreateNotificationDto input);

        Task DeleteAsync(Guid id);

        Task<CustomPagedResultDto> GetByFilter(PagedSortedRequestDto input);
    }
}
