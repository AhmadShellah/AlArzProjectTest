using AdminManagement.Settings;
using AlArz.AdminManagement.Interfaces;
using AlArz.ALLFilters;
using AlArz.Dtos;
using AlArz.Managers;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace AlArz.AdminManagement.Services
{
    //[Authorize(AdminPermissions.Admins.Default)]
    public class LookupService : BasicService, ILookupService
    {
        private readonly IRepository<Lookup, Guid> _LookupRepository;
        private readonly LookupManager _LookupManager;

        public LookupService(IRepository<Lookup, Guid> LookupRepository, LookupManager LookupManager)
        {
            _LookupManager = LookupManager;
            _LookupRepository = LookupRepository;
        }

        [Authorize(AdminPermissions.Admins.Lookup.Create)]
        public async Task<LookupDto> CreateAsync(CreateLookupDto input)
        {
            var mapping = ObjectMapper.Map<CreateLookupDto, Lookup>(input);

            var result = await _LookupManager.Create(mapping);

            return await GetAsync(result.Id);
        }

        [Authorize(AdminPermissions.Admins.Lookup.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _LookupManager.DeleteAsync(id);
        }

        [Authorize]
        public async Task<LookupDto> GetAsync(Guid id)
        {
            var result = await _LookupManager.GetAsync(id);

            var mapperResult = ObjectMapper.Map<Lookup, LookupDto>(result);

            return mapperResult;
        }

        public async Task<Boolean> ChecklookupCodeExcistsAsync(Guid lookupTypeId, int code)
        {
            var result = await _LookupManager.GetByTypeAndCode(code, lookupTypeId);
            return result != null;
        }

        //[Authorize] pls note that this api used in home page 
        public async Task<LookupDto> GetLookupByCode(int lookupTypeCode, int lookupCode)
        {
            var result = await _LookupManager.GetLookupByCode(lookupTypeCode, lookupCode);
            var mapperResult = ObjectMapper.Map<Lookup, LookupDto>(result);
            return mapperResult;
        }

        [Authorize(AdminPermissions.Admins.Lookup.Default)]
        public async Task<PagedResultDto<LookupDto>> GetListPagedAsync(PagedSortedRequestDto input, FilterLookup filter)
        {
            await NormalizeMaxResultCountAsync(input);

            var resultFromManager = await _LookupManager.GetListPagedAsync(input, filter);

            var dtos = ObjectMapper.Map<List<Lookup>, List<LookupDto>>(resultFromManager.Items.ToList());

            return new PagedResultDto<LookupDto>(resultFromManager.TotalCount, dtos);
        }

        [Authorize]
        public async Task<LookupDto> GetLookupTypeIdAsync(Guid lookUpTypeId)
        {
            var result = await _LookupManager.GetLookupTypeIdAsync(lookUpTypeId);

            var mapperResult = ObjectMapper.Map<Lookup, LookupDto>(result);

            return mapperResult;
        }

        [Authorize(AdminPermissions.Admins.Lookup.Update)]
        public async Task<LookupDto> UpdateAsync(UpdateLookupDto input)
        {
            var result = await _LookupManager.GetByIdAsync(input.Id);
            if (result == null)
            {
                throw new NotUpdateException();
            }
            ObjectMapper.Map(input, result);

            var final = await _LookupManager.Update(result);

            return await GetAsync(final.Id);

        }

        [Authorize]
        public async Task<List<LookupDto>> GetLookupTypeNumber(int number)
        {
            var resultFromManager = await _LookupManager.GetLookupTypeNumber(number);

            return ObjectMapper.Map<List<Lookup>, List<LookupDto>>(resultFromManager);
        }

        [Authorize]
        public async Task<List<LookupDto>> GetByLookupTypeId(Guid LookupTypeId)
        {
            var resultFromManager = await _LookupManager.GetByLookupTypeId(LookupTypeId);

            return ObjectMapper.Map<List<Lookup>, List<LookupDto>>(resultFromManager);
        }

        [Authorize]
        public async Task<IEnumerable<LookupTypeGroupDto>> GetLookupTypesByNumberList(List<int> numbers)
        {
            var lstLookupTypes = (await _LookupRepository.GetQueryableAsync())
                .Include(x => x.LookupType)
                .Where(c => numbers.Contains(c.LookupType.LookupTypeNumber))
                .ToList();

            if (lstLookupTypes != null)
            {
                var mapper = ObjectMapper.Map<List<Lookup>, List<LookupDto>>(lstLookupTypes)
                    .GroupBy(x => new { x.LookupTypeId, x.LookupType.LookupTypeNumber })
                    .Select(y => new LookupTypeGroupDto { LookupTypeId = y.Key.LookupTypeId, LookupTypeNumber = y.Key.LookupTypeNumber, Lookups = y.AsEnumerable() });
                return mapper;
            }
            throw new NotFoundException();
        }

        [Authorize]
        public async Task<IEnumerable<LookupDto>> GetLookupListUsingGuid(IEnumerable<Guid> LookupIds)
        {
            var listlookups = await _LookupManager.GetLookupListUsingGuid(LookupIds);

            var mapper = ObjectMapper.Map<List<Lookup>, List<LookupDto>>(listlookups.ToList());

            return mapper;

        }

        [Authorize]
        //This method to check Count if contains list lookup or not 
        public async Task GetLookupCount(List<Guid> LookupIds)
        {
            var resultFromManagerCount = await _LookupManager.GetLookupCount(LookupIds);
        }


    }
}
