using AlArz.Managers;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace AlArz.Data
{
    public class AdminServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly LookupTypeManager _lookupTypeManager;
        private readonly ConfigManager _configManager;
        private readonly IRepository<LookupType, Guid> _lookupTypeRepository;
        private readonly LookupManager _lookupManager;
        private readonly IUnitOfWorkAccessor _unitOfWorkAccessor;

        public AdminServiceDataSeeder(
            IRepository<LookupType, Guid> lookupTypeRepository,

            LookupTypeManager lookupTypeManager, IRepository<Lookup, Guid> lookupRepository,
            LookupManager lookupManager,
            ConfigManager configManager,
            IUnitOfWorkAccessor unitOfWorkAccessor
            )
        {
            _lookupTypeRepository = lookupTypeRepository;
            _lookupTypeManager = lookupTypeManager;
            _lookupManager = lookupManager;
            _unitOfWorkAccessor = unitOfWorkAccessor;
            _configManager = configManager;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            (await _lookupTypeRepository.GetDbContextAsync()).Database.ExecuteSqlRaw("SET IDEntity_INSERT AL_LookupTypes ON");

            await AddLookupTypesAsync();
            await AddLookupsAsync();
            await AddConfigAsync();
        }



        public async Task AddLookupTypesAsync()
        {
            //var list = new[]
            // {
            //    new LookupType { Enabled =false,NameAr="انواع الاسئلة", Name = "CheckListQuestionTypes", LookupTypeNumber=(int)LookupTypes.CheckListQuestionTypes},
            //   };

           // await _lookupTypeManager.CreateForSeederAsync(list);

            await _unitOfWorkAccessor.UnitOfWork.SaveChangesAsync();
        }

        private async Task AddConfigAsync()
        {
            // await _configManager.CreateAsync((int)ConfigLookup.AllowPagination, ConfigLookup.AllowPagination.ToString(),"تمكين متعدد الصفحات", "Allow paginatioon", "false");
            await _unitOfWorkAccessor.UnitOfWork.SaveChangesAsync();
        }
        private async Task AddLookupsAsync()
        {
            // await _lookupManager.CreateAsync(false, "Started", "Started", (int)OrderStatusLookup.Started, (int)LookupTypes.OrderStatus);

            await _unitOfWorkAccessor.UnitOfWork.SaveChangesAsync();
        }
    }
}
