using Application.Exceptions;
using Domain.Entity;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AlArz.Managers
{
    public class NotificationTemplateManger : DomainService
    {
        private readonly IRepository<NotificationTemplate, Guid> _Notificationsrepository;
        public NotificationTemplateManger(IRepository<NotificationTemplate, Guid> Notificationsrepository)
        {
            _Notificationsrepository = Notificationsrepository;
        }

        public async Task<NotificationTemplate> GetNotificationTemplateByTypeAndEvemtAsync(Guid events, Guid notificaitontypeId)
        {
            var check = await _Notificationsrepository.FirstOrDefaultAsync(x => x.Event.Id == events);
            return check;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _Notificationsrepository.DeleteAsync(id);
        }

        public async Task<NotificationTemplate> GetAsync(Guid id)
        {
            return await _Notificationsrepository.GetAsync(id);
        }

        public async Task<NotificationTemplate> Create(NotificationTemplate notification)
        {
            try
            {
                GuidGenerator.Create();

                var result = await _Notificationsrepository.InsertAsync(notification);

                return result;

            }
            catch (Exception)
            {
                throw new NotCreatedException();
            }
        }

        public async Task<NotificationTemplate> Update(NotificationTemplate notification)
        {
            try
            {
                var up = await _Notificationsrepository.UpdateAsync(notification);
                return up;
            }
            catch (Exception)
            {
                throw new NotUpdateException();
            }
        }
    }
}
