
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AlArz.Managers
{
    public class AttachmentManager : DomainService
    {
        private readonly IRepository<Attachment, Guid> _repository;

        public AttachmentManager()
        {
        }
        public AttachmentManager(IRepository<Attachment, Guid> attachmentRepository)
        {
            _repository = attachmentRepository;
        }

        public async Task<PagedResultDto<Attachment>> GetPublishedListAsync(PagedSortedRequestDto input, bool img)
        {
            List<string> imgExts = new List<string>() { ".png", ".svg", ".gif", ".jpg", ".jpeg" };
            List<string> videoExts = new List<string>() { ".mov", ".avi", ".wmv", ".flv", ".webm" };
            Expression<Func<Attachment, bool>> expression = s => true;
            if (img)
            {
                expression = expression.And(s => imgExts.Any(x => s.FileName.EndsWith(x)));

            }
            else
            {
                expression = expression.And(s => videoExts.Any(x => s.FileName.EndsWith(x)));

            }
            var response = new PagedResultDto<Attachment>();

            var queryableResult = await _repository.GetQueryableAsync();

            queryableResult = queryableResult//.Include(x => x.ModuleType)
                                             .Where(expression);

            response.TotalCount = queryableResult.Count();

            queryableResult = queryableResult
                .OrderByDescending(x => x.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);
            response.Items = queryableResult.ToList();
            return response;
        }


        public async Task<Attachment> Create(Attachment attachment, bool autoSave = false)
        {
            if (attachment != null)
            {
                GuidGenerator.Create();

                var result = await _repository.InsertAsync(attachment, autoSave: autoSave);

                return result;

            }
            throw new NotCreatedException();
        }

        public async Task<Attachment> Update(Attachment input)
        {
            if (input != null)
            {
                return await _repository.UpdateAsync(input);

            }
            throw new NotUpdateException();

        }
    }
}
