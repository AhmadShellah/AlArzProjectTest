using AdminManagement;
using AdminManagement.Settings;
using AlArz.AdminManagement.Interfaces;
using AlArz.Dtos;
using AlArz.Interface;
using AlArz.Managers;
using AlArz.Shared;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Application.Service
{
    public class ConfigService : BasicService, IConfigService
    {
        private readonly ConfigManager _configManager;
        private readonly ILookupService _lookupService;

        public ConfigService(ConfigManager configManager, ILookupService lookupService)
        {
            _lookupService = lookupService;
            _configManager = configManager;
        }

        [Authorize(AdminPermissions.Admins.Config.Create)]
        public async Task<ConfigDto> CreateAsync(CreateConfigDto input)
        {
            var mapping = ObjectMapper.Map<CreateConfigDto, Config>(input);

            var result = await _configManager.Create(mapping);

            var configDto = ObjectMapper.Map<Config, ConfigDto>(result);

            return configDto;
        }


        [Authorize(AdminPermissions.Admins.Config.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _configManager.DeleteAsync(id);
        }

        [Authorize(AdminPermissions.Admins.Config.Default)]
        public async Task<ConfigDto> GetAsync(Guid id)
        {
            var result = await _configManager.GetByIdAsync(id);

            var mapperResult = ObjectMapper.Map<Config, ConfigDto>(result);

            return mapperResult;
        }

        public async Task<ConfigDto> GetByNameAsync(string name, int typeCode)
        {
            var result = await _configManager.GetByNameAsync(name, typeCode);

            var mapperResult = ObjectMapper.Map<Config, ConfigDto>(result);

            return mapperResult;
        }

        [Authorize(AdminPermissions.Admins.Config.Default)]
        public async Task<List<ConfigDto>> GetByLookupCodeAsync(int code)
        {
            var lookup = await _lookupService.GetLookupByCode((int)LookupTypes.Config, code);
            if (lookup == null)
            {
                throw new BusinessException(DomainErrorCodesAdminManagement.lookupNotFound);
            }

            var result = await _configManager.GetListAsync(lookup.Id);

            var mapperResult = ObjectMapper.Map<List<Config>, List<ConfigDto>>(result);

            return mapperResult;
        }


        [Authorize(AdminPermissions.Admins.Config.Default)]
        public async Task<PagedResultDto<ConfigDto>> GetListPagedAsync(PagedSortedRequestDto input)
        {
            await NormalizeMaxResultCountAsync(input);

            var configs = await _configManager.GetListPagedAsync(input);

            var dtos = ObjectMapper.Map<List<Config>, List<ConfigDto>>(configs.Items.ToList());

            return new PagedResultDto<ConfigDto>(configs.TotalCount, dtos.ToList());
        }


        [Authorize(AdminPermissions.Admins.Config.Update)]
        public async Task<ConfigDto> UpdateAsync(UpdateConfigDto input)
        {
            var result = await _configManager.GetByIdAsync(input.Id);
            if (result == null)
            {
                throw new NotFoundException();
            }

            input.Name = result.Name;

            ObjectMapper.Map(input, result);

            var final = await _configManager.Update(result);

            return ObjectMapper.Map<Config, ConfigDto>(final);
        }

    }
}
