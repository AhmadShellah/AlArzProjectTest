using AlArz.Entity;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;


namespace AlArz.Managers
{
    public class CustomLogManager : DomainService
    {
        private readonly IRepository<CustomLog, Guid> _customLogRepository;
        public CustomLogManager(IRepository<CustomLog, Guid> customLogRepository)
        {
            _customLogRepository = customLogRepository;
        }

        public async Task CustomLog(CustomLog log)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            log.Request = JsonConvert.SerializeObject(log.Request);

            log.Response = JsonConvert.SerializeObject(log.Response);

            GuidGenerator.Create();

            await _customLogRepository.InsertAsync(log);
        }

    }
}
