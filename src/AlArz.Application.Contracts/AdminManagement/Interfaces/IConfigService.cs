using AlArz.Dtos;
using Application.Contracts.EntityDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AlArz.Interface
{
    public interface IConfigService : IApplicationService
    {
        Task<PagedResultDto<ConfigDto>> GetListPagedAsync(PagedSortedRequestDto input);
        Task<ConfigDto> GetAsync(Guid id);
        Task<ConfigDto> GetByNameAsync(string name, int typeCode);
        Task<List<ConfigDto>> GetByLookupCodeAsync(int code);
        Task<ConfigDto> CreateAsync(CreateConfigDto input);
        Task<ConfigDto> UpdateAsync(UpdateConfigDto input);
        Task DeleteAsync(Guid id);
    }
}
