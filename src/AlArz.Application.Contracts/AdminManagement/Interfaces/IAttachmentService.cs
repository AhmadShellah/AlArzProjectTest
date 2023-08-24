using AlArz.Dtos;
using Application.Contracts.EntityDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AlArz.Interface
{
    public interface IAttachmentService : IApplicationService
    {
        Task<PagedResultDto<AttachmentDto>> GetPublishedListAsync(PagedSortedRequestDto input, bool img);
        Task<AttachmentDto> GetAsync(Guid id);
        Task<AttachmentDto> GetLastByModuleAsync(Guid moduleId);
        Task<List<AttachmentDto>> GetByModuleIdAsync(Guid moduleId);
        Task<List<AttachmentDto>> GetByModuleIdsAsync(List<Guid> moduleIds);
        Task<AttachmentDto> CreateAsync(CreateAttachmentDto input);
        Task<List<AttachmentDto>> CreateManyAsync(List<CreateAttachmentDto> inputs);
        Task<List<AttachmentDto>> updateAttachment(List<CreateAttachmentDto> attachment, Guid? attachmentId, Guid moduleId, Guid moduleTypeId);
        Task<AttachmentDto> UpdateAsync(CreateAttachmentDto input);
        Task DeleteAsync(Guid id);
        Task DeleteManyAsync(List<Guid> ids);
    }

    public interface IAttachmentByteService : IAttachmentService
    {
        Task<AttachmentFileDto> GetFileWithBytesAsync(Guid id);
        Task<AttachmentFileDto> GetFileWithBytesByModuleIdAsync(Guid moduleId);
    }
}
