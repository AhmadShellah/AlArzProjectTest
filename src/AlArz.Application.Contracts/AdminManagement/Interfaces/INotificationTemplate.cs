using AlArz.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AlArz.Interface
{
    public interface INotificationTemplate : IApplicationService
    {
        Task<NotificationTemplateDto> GetAsync(Guid id);
        Task<NotificationTemplateDto> UpdateAsync(UpdateNotificationTemplateDto input);
        Task<NotificationTemplateDto> CreateAsync(CreateNotificationTemplateDto input);

        Task DeleteAsync(Guid id);
    }
}
