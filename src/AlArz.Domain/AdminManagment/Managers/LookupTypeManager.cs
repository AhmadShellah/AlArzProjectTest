using AlArz.ALLFilters;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AlArz.Managers
{
    public class LookupTypeManager : DomainService
    {
        private readonly IRepository<LookupType, Guid> _lookupTypeRepository;
        public LookupTypeManager(IRepository<LookupType, Guid> lookupTyperepository)
        {
            _lookupTypeRepository = lookupTyperepository;
        }

        public async Task<LookupType> CreateAsync(string Name, int LookupTypeNumber)
        {
            var existingLookupType = await _lookupTypeRepository.FirstOrDefaultAsync(p => p.LookupTypeNumber == LookupTypeNumber);
            if (existingLookupType != null)
            {
                throw new RecordAlReadyExistException();
            }
            return await _lookupTypeRepository.InsertAsync(new LookupType(GuidGenerator.Create(), LookupTypeNumber, Name));
        }

        public async Task CreateForSeederAsync(IEnumerable<LookupType> list)
        {
            var _inDb = await _lookupTypeRepository.GetListAsync();

            var codesInDb = _inDb.Select(x => x.LookupTypeNumber);

            list = list.Where(x => !codesInDb.Contains(x.LookupTypeNumber));
            await _lookupTypeRepository.InsertManyAsync(list);
        }

        public async Task<LookupType> Create(LookupType input)
        {
            try
            {
                input.Enabled = true;

                GuidGenerator.Create();

                var result = await _lookupTypeRepository.InsertAsync(input);

                return result;
            }
            catch (Exception)
            {
                throw new NotCreatedException();
            }
        }

        public async Task<LookupType> Update(LookupType lookupType)
        {
            try
            {
                var update = await _lookupTypeRepository.UpdateAsync(lookupType);
                return update;
            }
            catch (Exception)
            {
                throw new NotUpdateException();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _lookupTypeRepository.DeleteAsync(id);
        }
        public async Task<LookupType> GetByIdAsync(Guid id)
        {
            return await _lookupTypeRepository.GetAsync(id);
        }

        public async Task<LookupType> GetByLookupTypeNumberAsync(int lookupTypeNumber)
        {
            var result = await _lookupTypeRepository.FirstOrDefaultAsync(x => x.LookupTypeNumber == lookupTypeNumber);
            return result;
        }
        public async Task<List<LookupType>> GetAllAsync()
        {
            Expression<Func<LookupType, bool>> expression = s => true;
            Expression<Func<LookupType, object>> sorting = s => "LookupTypeNumber";

            var resultFromDataBase = (await _lookupTypeRepository.GetQueryableAsync()).Where(expression);

            resultFromDataBase = resultFromDataBase.OrderBy(sorting);

            return resultFromDataBase.ToList();
        }
        public async Task<PagedResultDto<LookupType>> GetListPagedAsync(FilterLookupType filter = null, PagedSortedRequestDto input = null)
        {
            Expression<Func<LookupType, bool>> expression = s => true;
            Expression<Func<LookupType, object>> sorting = s => "";

            var result = new PagedResultDto<LookupType>();

            var resultFromDataBase = (await _lookupTypeRepository.GetQueryableAsync())
                                                                               .Where(expression)
                                                                               .Skip(input.SkipCount)
                                                                               .Take(input.MaxResultCount);
            result.TotalCount = resultFromDataBase.Count();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                resultFromDataBase = resultFromDataBase.Where(x => x.Name.Contains(filter.Name));
            }

            result.TotalCount = resultFromDataBase.Count();

            if (input.SortDescending == false)
            {
                resultFromDataBase = resultFromDataBase.OrderBy(sorting);
            }
            else
            {
                resultFromDataBase = resultFromDataBase.OrderByDescending(sorting);
            }

            result.Items = resultFromDataBase.ToList();
            return result;
        }



    }
}
