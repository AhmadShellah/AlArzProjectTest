using AlArz.ALLFilters;
using AlArz.Dtos;
using Application.Contracts.EntityDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AlArz.Interface
{
    public interface ILookupTypeService : IApplicationService
    {

        Task<PagedResultDto<LookupTypeDto>> GetListPagedAsync(PagedSortedRequestDto input, FilterLookupType filter);

        Task<List<LookupTypeDto>> GetAllAsync();

        Task<LookupTypeDto> GetAsync(Guid id);

        Task<LookupTypeDto> GetByLookupTypeNumberAsync(int lookupTypeNumber);

        Task<LookupTypeDto> CreateAsync(CreateLookupTypeDto input);

        Task<LookupTypeDto> UpdateAsync(UpdateLookupTypeDto input);

        Task DeleteAsync(Guid id);
    }
}
