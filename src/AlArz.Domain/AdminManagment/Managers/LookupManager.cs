using AdminManagement;
using AlArz.ALLFilters;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AlArz.Managers
{
    public class LookupManager : DomainService
    {
        private readonly IRepository<Lookup, Guid> _lookupRepository;
        private readonly IRepository<LookupType, Guid> _lookupTypeRepository;

        public LookupManager(IRepository<Lookup, Guid> lookupRepository, IRepository<LookupType, Guid> lookupTypeRepository)
        {
            _lookupRepository = lookupRepository;
            _lookupTypeRepository = lookupTypeRepository;
        }

        public async Task<Lookup> GetAsync(Guid id)
        {
            var qResult = await _lookupRepository.FirstOrDefaultAsync(x => x.Id == id);
            //.Include(x => x.ParentLookup)

            if (qResult == null)
            {
                throw new NotFoundException();
            }

            return qResult;
        }

        public async Task<Lookup> GetByIdAsync(Guid id)
        {
            var resultFromDataBase = await _lookupRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (resultFromDataBase == null)
            {
                throw new NotFoundException();
            }

            return resultFromDataBase;
        }

        public async Task<Lookup> GetByTypeAndCode(int code, Guid Typeid)
        {
            var existingCode = await _lookupRepository
                .FirstOrDefaultAsync(p => p.Code == code && p.LookupTypeId == Typeid);

            return existingCode;
        }

        public async Task<Lookup> CreateAsync(bool Enabled, string Name, string NameAr, int Code, Guid lookupTypId)
        {
            var existingLookup = await _lookupRepository.FirstOrDefaultAsync(p => p.Code == Code && p.LookupTypeId == lookupTypId);
            if (existingLookup != null)
            {
                return null;
            }
            return await _lookupRepository.InsertAsync(new Lookup(GuidGenerator.Create(), Code, Name, NameAr, lookupTypId, Enabled), autoSave: true);
        }

        public async Task<Lookup> CreateAsync(bool Enabled, string Name, string NameAr, int Code, int lookupType)
        {
            var lookupTypId = (await _lookupTypeRepository.FirstOrDefaultAsync(x => x.LookupTypeNumber == lookupType))?.Id;
            if (lookupTypId == null)
            {
                throw new NotFoundException();
            }
            return await CreateAsync(Enabled, Name, NameAr, Code, lookupTypId.Value);
        }

        public async Task<Lookup> Create(Lookup lookup)
        {
            try
            {
                lookup.Enabled = true;

                var code = await GetByTypeAndCode(lookup.Code, lookup.LookupTypeId);
                if (code != null)
                {
                    throw new BusinessException(DomainErrorCodesAdminManagement.lookupAlreadyExists);
                }

                GuidGenerator.Create();

                var result = await _lookupRepository.InsertAsync(lookup, autoSave: true);

                return result;
            }
            catch (Exception)
            {
                throw new NotCreatedException();
            }

        }

        public async Task CreateForSeederAsync(IEnumerable<Lookup> list)
        {
            var _inDb = await _lookupRepository.GetListAsync();

            var codesInDb = _inDb.Select(x => x.Code);

            list = list.Where(x => !codesInDb.Contains(x.Code));

            await _lookupRepository.InsertManyAsync(list);
        }

        public async Task<Lookup> Update(Lookup input)
        {
            var result = await GetByTypeAndCode(input.Code, input.LookupTypeId);
            if (result != null && result.Id != input.Id)
            {
                throw new BusinessException(DomainErrorCodesAdminManagement.lookupAlreadyExists)
                                .WithData("name", input.Code);
            }

            var up = await _lookupRepository.UpdateAsync(input, autoSave: true);

            return up;
        }

        public async Task<Lookup> GetLookupByCode(int lookupTypeCode, int lookupCode)
        {
            var result = await _lookupRepository.FirstOrDefaultAsync(x => x.Code == lookupCode && x.LookupType.LookupTypeNumber == lookupTypeCode);
            if (result == null)
            {
                throw new NotFoundException();
            }

            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _lookupRepository.DeleteAsync(id);
        }

        public async Task<Lookup> GetLookupTypeIdAsync(Guid lookUpTypeId)
        {
            var result = await _lookupRepository.FirstOrDefaultAsync(x => x.LookupTypeId == lookUpTypeId);

            return result;
        }

        public async Task<List<Lookup>> GetLookupTypeNumber(int number)
        {
            var resultFromDataBase = await (await _lookupRepository.GetQueryableAsync())
                .Where(x => x.LookupType.LookupTypeNumber == number)
                .ToListAsync();

            return resultFromDataBase;
        }

        public async Task<List<Lookup>> GetByLookupTypeId(Guid LookupTypeId)
        {
            var resultFromDataBase = await (await _lookupRepository.GetQueryableAsync())
                .Where(x => x.LookupTypeId == LookupTypeId)
                .ToListAsync();

            return resultFromDataBase;
        }

        public async Task<PagedResultDto<Lookup>> GetListPagedAsync(PagedSortedRequestDto input, FilterLookup filter)
        {
            Expression<Func<Lookup, bool>> expression = s => true;
            Expression<Func<Lookup, object>> sorting = s => "";

            if (filter.LookupTypeId != null && filter.LookupTypeId != Guid.Empty)
            {
                expression = expression.And(X => X.LookupTypeId == filter.LookupTypeId);
            }

            var resultFromDataBase = (await _lookupRepository.GetQueryableAsync())
                    .Include(x => x.ParentLookup)
                    .Where(expression)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);

            var Lookups = new PagedResultDto<Lookup>();

            if (input != null)
            {
                if (input.Sorting != null)
                {
                    if (input.Sorting.ToLower().Equals("code"))
                    {
                        sorting = s => s.Code;
                    }
                }
                if (input.SortDescending == false)
                {
                    resultFromDataBase.OrderBy(sorting);
                }
                else
                {
                    resultFromDataBase = resultFromDataBase.OrderByDescending(sorting);
                }
            }

            Lookups.TotalCount = await resultFromDataBase.CountAsync();
            Lookups.Items = await resultFromDataBase.ToListAsync();

            return Lookups;
        }

        public async Task<IEnumerable<Lookup>> GetLookupListUsingGuid(IEnumerable<Guid> LookupIds)
        {
            var listLookupsFromDataBase = await (await _lookupRepository.GetQueryableAsync())
                .Where(c => LookupIds.Contains(c.Id))
                .ToListAsync();

            return listLookupsFromDataBase;
        }


        public async Task<int> GetLookupCount(List<Guid> LookupIds)
        {
            var resultFromDataBaseCount = await _lookupRepository.CountAsync(c => LookupIds.Contains(c.Id));
            if (resultFromDataBaseCount != LookupIds.Count())
            {
                throw new BusinessException(DomainErrorCodesAdminManagement.OneOrMoreNotFound);
            }

            return resultFromDataBaseCount;
        }


    }
}
