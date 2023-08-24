using AlArz.ALLFilters;
using AlArz.Dtos;
using AlArz.Interface;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace AlArz.Controllers.AdminManagement
{
    [Route("api/AdminManagement/[controller]/[action]")]
    public class LookupTypesController : AbpController, ILookupTypeService
    {
        private readonly ILookupTypeService _lookupTypeService;
        public LookupTypesController(ILookupTypeService lookupTypeService)
        {
            _lookupTypeService = lookupTypeService;
        }

        [HttpGet]
        [Route("{id}")]
        public Task<LookupTypeDto> GetAsync(Guid id)
        {
            var result = _lookupTypeService.GetAsync(id);
            return result;
        }


        [HttpGet]
        [Route("{lookupTypeNumber}")]
        public Task<LookupTypeDto> GetByLookupTypeNumberAsync(int lookupTypeNumber)
        {
            var result = _lookupTypeService.GetByLookupTypeNumberAsync(lookupTypeNumber);
            return result;
        }

        [HttpGet]

        public async Task<PagedResultDto<LookupTypeDto>> GetListPagedAsync(PagedSortedRequestDto input, FilterLookupType filter)
        {
            return await _lookupTypeService.GetListPagedAsync(input, filter);
        }
        [HttpGet]

        public async Task<List<LookupTypeDto>> GetAllAsync()
        {
            return await _lookupTypeService.GetAllAsync();

        }
        [HttpDelete]
        public async Task DeleteAsync(Guid id)
        {
            await _lookupTypeService.DeleteAsync(id);
        }


        [HttpPost]
        public async Task<LookupTypeDto> CreateAsync(CreateLookupTypeDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _lookupTypeService.CreateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpPut]
        public async Task<LookupTypeDto> UpdateAsync(UpdateLookupTypeDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _lookupTypeService.UpdateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }


    }
}
