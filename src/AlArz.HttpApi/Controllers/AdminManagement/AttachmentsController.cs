using AlArz.Dtos;
using AlArz.Interface;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;


namespace AlArz.Controllers.AdminManagement
{
    [Route("api/AdminManagement/[controller]/[action]")]
    public class AttachmentsController : AbpController, IAttachmentService
    {
        private readonly IAttachmentByteService _service;
        public AttachmentsController(IAttachmentService AttachmentService)
        {
            _service = (IAttachmentByteService)AttachmentService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<AttachmentDto> GetAsync(Guid id)
        {
            var result = await _service.GetAsync(id);
            return result;
        }

        [HttpGet]
        [Route("{moduleId}")]
        public async Task<AttachmentDto> GetLastByModuleAsync(Guid moduleId)
        {
            var result = await _service.GetLastByModuleAsync(moduleId);
            return result;
        }

        [HttpGet]
        public async Task<PagedResultDto<AttachmentDto>> GetPublishedListAsync(PagedSortedRequestDto input, bool img)
        {
            return await _service.GetPublishedListAsync(input, img);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetByteAsync(Guid id)
        {
            var result = await _service.GetFileWithBytesAsync(id);
            if (result == null)
            {
                return NoContent();
            }

            var fileResult = new FileStreamResult(new MemoryStream(result.ByteContent), result.MimeType);
            fileResult.FileDownloadName = result.OriginalFileName;
            return fileResult;
        }

        [HttpGet]
        [Route("{moduleId}")]
        public async Task<ActionResult> GetByteByModuleId(Guid moduleId)
        {
            var result = await _service.GetFileWithBytesByModuleIdAsync(moduleId);
            if (result == null)
            {
                return NoContent();
            }

            var fileResult = new FileStreamResult(new MemoryStream(result.ByteContent), result.MimeType);
            fileResult.FileDownloadName = result.OriginalFileName;
            return fileResult;
        }

        [HttpGet]
        [Route("{moduleId}")]
        public async Task<List<AttachmentDto>> GetByModuleIdAsync(Guid moduleId)
        {
            var result = await _service.GetByModuleIdAsync(moduleId);
            return result;

        }


        [HttpGet]
        public async Task<List<AttachmentDto>> GetByModuleIdsAsync(List<Guid> moduleIds)
        {
            var result = await _service.GetByModuleIdsAsync(moduleIds);
            return result;

        }


        [HttpPost]
        public async Task<AttachmentDto> CreateAsync(CreateAttachmentDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpPost]
        public async Task<List<AttachmentDto>> CreateManyAsync(List<CreateAttachmentDto> inputs)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateManyAsync(inputs);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpPut]

        public async Task<AttachmentDto> UpdateAsync([FromBody] CreateAttachmentDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateAsync(input);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpPut]
        public async Task<List<AttachmentDto>> updateAttachment(List<CreateAttachmentDto> attachment, Guid? attachmentId, Guid moduleId, Guid moduleTypeId)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.updateAttachment(attachment, attachmentId, moduleId, moduleTypeId);
                return result;
            }
            throw new BadRequestException();
        }

        [HttpDelete]
        public async Task DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);
        }

        [HttpDelete]
        public async Task DeleteManyAsync(List<Guid> ids)
        {
            await _service.DeleteManyAsync(ids);
        }
    }
}
