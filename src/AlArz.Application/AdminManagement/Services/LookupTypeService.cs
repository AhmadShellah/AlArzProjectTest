using AdminManagement.Settings;
using AlArz.ALLFilters;
using AlArz.Dtos;
using AlArz.Interface;
using AlArz.Managers;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Application.Service
{

    public class LookupTypeService : BasicService, ILookupTypeService
    {
        private readonly LookupTypeManager _LookupTypeManager;
        public LookupTypeService(LookupTypeManager LookupTypeManager)
        {
            _LookupTypeManager = LookupTypeManager;
        }

        [Authorize(AdminPermissions.Admins.LookupType.Create)]
        public async Task<LookupTypeDto> CreateAsync(CreateLookupTypeDto input)
        {
            var mapping = ObjectMapper.Map<CreateLookupTypeDto, LookupType>(input);

            var result = await _LookupTypeManager.Create(mapping);

            var lookupTypeDto = ObjectMapper.Map<LookupType, LookupTypeDto>(result);

            return lookupTypeDto;
        }

        [Authorize(AdminPermissions.Admins.LookupType.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _LookupTypeManager.DeleteAsync(id);
        }

        public async Task<LookupTypeDto> GetAsync(Guid id)
        {
            var result = await _LookupTypeManager.GetByIdAsync(id);

            var mapperResult = ObjectMapper.Map<LookupType, LookupTypeDto>(result);

            return mapperResult;
        }

        public async Task<LookupTypeDto> GetByLookupTypeNumberAsync(int lookupTypeNumber)
        {
            var result = await _LookupTypeManager.GetByLookupTypeNumberAsync(lookupTypeNumber);

            var mapperResult = ObjectMapper.Map<LookupType, LookupTypeDto>(result);

            return mapperResult;
        }

        public async Task<List<LookupTypeDto>> GetAllAsync()
        {
            var result = await _LookupTypeManager.GetAllAsync();

            return ObjectMapper.Map<List<LookupType>, List<LookupTypeDto>>(result);
        }

        [Authorize(AdminPermissions.Admins.LookupType.Default)]
        public async Task<PagedResultDto<LookupTypeDto>> GetListPagedAsync(PagedSortedRequestDto input, FilterLookupType filter)
        {
            await NormalizeMaxResultCountAsync(input);

            var resultFromManager = await _LookupTypeManager.GetListPagedAsync(filter, input);

            var dtos = ObjectMapper.Map<List<LookupType>, List<LookupTypeDto>>(resultFromManager.Items.ToList());

            return new PagedResultDto<LookupTypeDto>(resultFromManager.TotalCount, dtos);
        }

        [Authorize(AdminPermissions.Admins.LookupType.Update)]
        public async Task<LookupTypeDto> UpdateAsync(UpdateLookupTypeDto input)
        {
            var result = await _LookupTypeManager.GetByIdAsync(input.Id);
            if (result == null)
            {
                throw new NotFoundException();
            }

            ObjectMapper.Map(input, result);

            await _LookupTypeManager.Update(result);

            var mapperResult = ObjectMapper.Map<LookupType, LookupTypeDto>(result);

            return mapperResult;
        }

    }
}
