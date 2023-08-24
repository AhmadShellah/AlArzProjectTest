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
    public class ConfigsController : AbpController, IConfigService
    {
        private readonly IConfigService _ConfigService;

        public ConfigsController(IConfigService configService)
        {
            _ConfigService = configService;
        }

        [HttpGet]
        public async Task<PagedResultDto<ConfigDto>> GetListPagedAsync(PagedSortedRequestDto input)
        {
            return await _ConfigService.GetListPagedAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ConfigDto> GetAsync(Guid id)
        {
            var result = await _ConfigService.GetAsync(id);
            return result;
        }

        [HttpGet]
        [Route("{name}/{code}")]
        public async Task<ConfigDto> GetByNameAsync(string name, int code)
        {
            var result = await _ConfigService.GetByNameAsync(name, code);
            return result;
        }

        [HttpGet]
        [Route("{code}")]
        public async Task<List<ConfigDto>> GetByLookupCodeAsync(int code)
        {
            var result = await _ConfigService.GetByLookupCodeAsync(code);
            return result;
        }

        [HttpPost]
        public async Task<ConfigDto> CreateAsync(CreateConfigDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _ConfigService.CreateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpPut]
        public async Task<ConfigDto> UpdateAsync(UpdateConfigDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _ConfigService.UpdateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpDelete]
        public async Task DeleteAsync(Guid id)
        {
            await _ConfigService.DeleteAsync(id);
        }


    }
}
