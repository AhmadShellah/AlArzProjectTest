using AlArz.AdminManagement.Interfaces;
using AlArz.ALLFilters;
using AlArz.Dtos;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace AlArz.Controllers.AdminManagement
{


    [Route("api/AdminManagement/[controller]/[action]")]
    public class LookupsController : AbpController, ILookupService
    {
        private readonly ILookupService _LookupService;

        public LookupsController(ILookupService LookupService)
        {
            _LookupService = LookupService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<LookupDto> GetAsync(Guid id)
        {

            var result = await _LookupService.GetAsync(id);
            return result;
        }

        [HttpGet]
        [Route("{LookupTypeId}")]
        public async Task<List<LookupDto>> GetlookupTypeIdAsync(int lookupTypeId)
        {
            var result = await _LookupService.GetLookupTypeNumber(lookupTypeId);
            return result;
        }

        [HttpGet]
        [Route("{LookupTypeId}")]
        public async Task<List<LookupDto>> GetByLookupTypeId(Guid lookupTypeId)
        {
            var result = await _LookupService.GetByLookupTypeId(lookupTypeId);
            return result;
        }

        [HttpGet]
        [Route("{LookupTypeId}/{code}")]
        public async Task<Boolean> ChecklookupCodeExcistsAsync(Guid lookupTypeId, int code)
        {
            var result = await _LookupService.ChecklookupCodeExcistsAsync(lookupTypeId, code);
            return result;
        }

        [HttpGet]
        public async Task<PagedResultDto<LookupDto>> GetListPagedAsync(PagedSortedRequestDto input, FilterLookup filter)
        {
            try
            {
                return await _LookupService.GetListPagedAsync(input, filter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task DeleteAsync(Guid id)
        {

            await _LookupService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<LookupDto> CreateAsync(CreateLookupDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _LookupService.CreateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpPut]

        public async Task<LookupDto> UpdateAsync([FromBody] UpdateLookupDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _LookupService.UpdateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpGet]
        [Route("Number")]
        public async Task<List<LookupDto>> GetLookupTypeNumber(int number)
        {
            var result = await _LookupService.GetLookupTypeNumber(number);
            return result;
        }

        [HttpGet]

        public async Task<IEnumerable<LookupTypeGroupDto>> GetLookupTypesByNumberList(List<int> lookupTypeNumbers)
        {
            var result = await _LookupService.GetLookupTypesByNumberList(lookupTypeNumbers);
            return result;
        }

        [HttpGet]

        public async Task<IEnumerable<LookupDto>> GetLookupListUsingGuid(IEnumerable<Guid> LookupIds)
        {
            var result = await _LookupService.GetLookupListUsingGuid(LookupIds);
            return result;
        }
        [HttpGet]
        [Route("{LookupIds}")]
        public async Task GetLookupCount(List<Guid> LookupIds)
        {
            if (ModelState.IsValid)
            {
                await _LookupService.GetLookupCount(LookupIds);
            }
            throw new BusinessException();
        }

        [HttpGet]
        [Route("{lookupTypeCode}/{lookupCode}")]
        public async Task<LookupDto> GetLookupByCode(int lookupTypeCode, int lookupCode)
        {
            var result = await _LookupService.GetLookupByCode(lookupTypeCode, lookupCode);
            return result;
        }
    }
}
