using AlArz.ALLFilters;
using AlArz.Dtos;
using Application.Contracts.EntityDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AlArz.AdminManagement.Interfaces
{
    public interface ILookupService : IApplicationService
    {
        Task<PagedResultDto<LookupDto>> GetListPagedAsync(PagedSortedRequestDto input, FilterLookup filter);
        Task<LookupDto> GetAsync(Guid id);
        Task<LookupDto> CreateAsync(CreateLookupDto input);
        Task<LookupDto> UpdateAsync(UpdateLookupDto input);
        Task DeleteAsync(Guid id);
        Task<List<LookupDto>> GetLookupTypeNumber(int number);
        Task<List<LookupDto>> GetByLookupTypeId(Guid LookupTypeId);
        Task<Boolean> ChecklookupCodeExcistsAsync(Guid lookupTypeId, int code);
        Task<IEnumerable<LookupTypeGroupDto>> GetLookupTypesByNumberList(List<int> lookupTypeNumbers);
        Task<IEnumerable<LookupDto>> GetLookupListUsingGuid(IEnumerable<Guid> LookupIds);
        Task GetLookupCount(List<Guid> LookupIds);
        Task<LookupDto> GetLookupByCode(int lookupTypeCode, int lookupCode);
    }
}
