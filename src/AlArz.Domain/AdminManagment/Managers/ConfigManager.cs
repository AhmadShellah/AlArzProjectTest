using AdminManagement;
using AlArz.Shared;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AlArz.Managers
{
    //Ctrl r+g
    public class ConfigManager : DomainService
    {
        private readonly IRepository<Config, Guid> _configRepository;
        private readonly IRepository<Lookup, Guid> _lookupRepository;

        public ConfigManager(IRepository<Config, Guid> configRepository, IRepository<Lookup, Guid> lookupRepository)
        {
            _configRepository = configRepository;
            _lookupRepository = lookupRepository;
        }

        public async Task<Config> GetConfigByName(Guid TypeId, string Name)
        {
            var result = await _configRepository.FirstOrDefaultAsync(x => x.Name == Name && x.TypeId == TypeId);
            return result;
        }

        public async Task<Config> GetByNameAsync(string name, int typeCode)
        {
            var result = await _configRepository.FirstOrDefaultAsync(x => x.Name == name && x.Lookup.Code == typeCode);

            //.Include(x => x.Lookup)
            //.FirstOrDefaultAsync(x => x.Name == name && x.Lookup.Code == typeCode);

            return result;
        }

        public async Task<Config> Create(Config config)
        {
            if (config != null)
            {
                throw new NotCreatedException();
            }

            var resultmethod = await GetConfigByName(config.TypeId, config.Name);
            if (resultmethod != null)
            {
                throw new BusinessException(DomainErrorCodesAdminManagement.configNameAlreadyExists)
                .WithData("Name", config.Name);
            }

            GuidGenerator.Create();

            var result = await _configRepository.InsertAsync(config);
            return result;
        }

        public async Task<Config> CreateForSeeder(Config config)
        {
            if (config != null)
            {
                var resultmethod = await GetConfigByName(config.TypeId, config.Name);
                if (resultmethod != null)
                {
                    return null;
                }

                GuidGenerator.Create();

                var result = await _configRepository.InsertAsync(config);
                return result;
            }

            throw new NotCreatedException();
        }

        public async Task<Config> CreateAsync(int code, string name,string ArName ,  string description, string value)
        {
            var lookup = await _lookupRepository.FirstOrDefaultAsync(x => x.Code == code && x.LookupType.LookupTypeNumber == (int)LookupTypes.Config);
            if (lookup == null)
            {
                return null;
            }

            return await CreateForSeeder(new Config { Name = name, TypeId = lookup.Id, Value = value, Description = description,NameAr = ArName });
        }

        public async Task<Config> Update(Config input)
        {
            var dbItem = await GetConfigByName(input.TypeId, input.Name);
            if (dbItem != null && !Guid.Equals(dbItem.Id, input.Id))
            {
                throw new BusinessException(DomainErrorCodesAdminManagement.configNameAlreadyExists)
                  .WithData("name", input.Name);
            }

            var up = await _configRepository.UpdateAsync(input);

            return up;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _configRepository.DeleteAsync(id);
        }

        public async Task<Config> GetByIdAsync(Guid id)
        {
            return await _configRepository.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Config>> GetListAsync(Guid LookupId)
        {
            return await _configRepository.GetListAsync(x => Guid.Equals(x.TypeId, LookupId));
        }

        public async Task<PagedResultDto<Config>> GetListPagedAsync(PagedSortedRequestDto input)
        {
            Expression<Func<Config, bool>> expression = s => true;

            Expression<Func<Config, object>> sorting = s => "";

            var configs = new PagedResultDto<Config>();
            var resultFromDataBase = (await _configRepository.GetQueryableAsync())
                   .Where(expression);

            if (input.Sorting != null)
            {
                if (input.Sorting.ToLower().Equals("name"))
                {
                    sorting = s => s.Name;
                }
                else if (input.Sorting.ToLower().Equals("description"))
                {
                    sorting = s => s.Name;
                }
                else if (input.Sorting.ToLower().Equals("value"))
                {
                    sorting = s => s.Name;
                }
            }

            if (input.SortDescending == false)
            {
                resultFromDataBase.OrderBy(sorting);
            }
            else
            {
                resultFromDataBase = resultFromDataBase.OrderByDescending(sorting);
            }

            configs.TotalCount = resultFromDataBase.Count();

            configs.Items = resultFromDataBase
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

            return configs;
        }
    }
}
