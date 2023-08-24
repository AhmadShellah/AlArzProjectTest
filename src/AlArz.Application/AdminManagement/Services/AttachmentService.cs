using AdminManagement.Settings;
using AlArz.Dtos;
using AlArz.Interface;
using AlArz.Managers;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace Application.Service
{
    public class AttachmentService : BasicService, IAttachmentByteService
    {
        private readonly IRepository<Attachment, Guid> _AttachmentRepository;
        private readonly AttachmentManager _manager;
        private readonly IBlobContainer<AttachmentContainer> _attachmentContenair;
        public AttachmentService(IRepository<Attachment, Guid> AttachmentRepository, AttachmentManager AttachmentManager, IBlobContainer<AttachmentContainer> attachmentContenair)
        {
            _manager = AttachmentManager;
            _AttachmentRepository = AttachmentRepository;
            _attachmentContenair = attachmentContenair;
        }


        // for mainPage no  auth
        public async Task<PagedResultDto<AttachmentDto>> GetPublishedListAsync(PagedSortedRequestDto input, bool img)
        {
            var result = await _manager.GetPublishedListAsync(input, img);

            var dtos = ObjectMapper.Map<List<Attachment>, List<AttachmentDto>>(result.Items.ToList());
            return new PagedResultDto<AttachmentDto>(result.TotalCount, dtos);
        }

        //    [Authorize(BaladiyatPermissions.Portals.Create)]
        [AllowAnonymous]
        public async Task<AttachmentDto> CreateAsync(CreateAttachmentDto input)
        {
            var mapping = ObjectMapper.Map<CreateAttachmentDto, Attachment>(input);
            input.Content = input.Content.Substring(input.Content.IndexOf(',') + 1);

            var contects = Convert.FromBase64String(input.Content);
            var key = GuidGenerator.Create();
            var type = input.OriginalFileName.Substring(input.OriginalFileName.IndexOf('.') + 1);
            await _attachmentContenair.SaveAsync(key.ToString() + "." + type, contects);
            mapping.FileName = key.ToString() + "." + type;
            var result = await _manager.Create(mapping, true);

            return ObjectMapper.Map<Attachment, AttachmentDto>(result);
        }


        public async Task<List<AttachmentDto>> CreateManyAsync(List<CreateAttachmentDto> inputs)
        {
            var results = new List<AttachmentDto>();
            foreach (var input in inputs)
            {
                if (input.Content != null)
                {
                    var mapping = ObjectMapper.Map<CreateAttachmentDto, Attachment>(input);
                    input.Content = input.Content.Substring(input.Content.IndexOf(',') + 1);

                    var contects = Convert.FromBase64String(input.Content);
                    var key = GuidGenerator.Create();
                    var type = input.OriginalFileName.Substring(input.OriginalFileName.IndexOf('.') + 1);
                    await _attachmentContenair.SaveAsync(key.ToString() + "." + type, contects);
                    mapping.FileName = key.ToString() + "." + type;
                    var result = ObjectMapper.Map<Attachment, AttachmentDto>(await _manager.Create(mapping));
                    result.FileName = mapping.FileName;
                    results.Add(result);
                }
                else
                {
                    throw new BadRequestException();
                }
            }


            return results;
        }

        public async Task<List<AttachmentDto>> updateAttachment(List<CreateAttachmentDto> attachments, Guid? attachmentId, Guid moduleId, Guid moduleTypeId)
        {
            var savedAttachments = (await GetByModuleIdAsync(moduleId)).Where(x => x.Id != attachmentId).ToList();
            var newAttachment = attachments.Where(x => x.Id.Equals(Guid.Empty) || x.Id == null).ToList();
            newAttachment.ForEach(newAttachment =>
            {
                newAttachment.ModuleId = moduleId;
                newAttachment.ModuleTypeId = moduleTypeId;
            });
            var createdNewAttachment = await CreateManyAsync(newAttachment);
            var inputAttachment = ObjectMapper.Map<List<CreateAttachmentDto>, List<AttachmentDto>>(attachments);
            inputAttachment.RemoveAll(x => x.Id == null || x.Id.Equals(Guid.Empty));
            inputAttachment.AddRange(createdNewAttachment);
            var deletedItem = savedAttachments.Select(x => x.Id).Except(inputAttachment.Select(y => y.Id)).ToList(); //var test = savedAttachments.Where(x =>! inputAttachment.Select(y=>y.Id).Contains(x.Id));
            await DeleteManyAsync(deletedItem);

            return inputAttachment;
        }

        public async Task<AttachmentDto> UpdateAsync(CreateAttachmentDto input)
        {
            if ((input != null && Guid.Empty.Equals(input.Id)) || input.Id == null)
            {
                var oldAttachment = await _AttachmentRepository.FirstOrDefaultAsync(x => x.ModuleId == input.ModuleId
                && x.ModuleTypeId == input.ModuleTypeId
                && x.AttachmentTypeId == input.AttachmentTypeId
                && x.IsDeleted != true);
                if (oldAttachment != null)
                    await DeleteAsync(oldAttachment.Id);
                if (!string.IsNullOrEmpty(input.Content))
                {
                    var attachment = await CreateAsync(input);
                    attachment.Content = null;
                    return attachment;
                }
            }
            else
            {
                var result = await _AttachmentRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (result != null)
                {
                    ObjectMapper.Map(input, result);
                    var final = ObjectMapper.Map<Attachment, AttachmentDto>(await _manager.Update(result));
                }
            }
            return ObjectMapper.Map<CreateAttachmentDto, AttachmentDto>(input);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _AttachmentRepository.DeleteAsync(id);
        }
        public async Task DeleteManyAsync(List<Guid> ids)
        {
            await _AttachmentRepository.DeleteManyAsync(ids);
        }
        public async Task<AttachmentDto> GetAsync(Guid id)
        {
            var result = await _AttachmentRepository.FindAsync(id);
            if (result != null)
            {
                var co = ObjectMapper.Map<Attachment, AttachmentDto>(result);
                var byt = await _attachmentContenair.GetAllBytesAsync(co.FileName);

                co.Content = Convert.ToBase64String(byt);
                return co;
            }
            throw new NotFoundException();
        }

        public async Task<AttachmentDto> GetLastByModuleAsync(Guid moduleId)
        {
            var result = await _AttachmentRepository.LastOrDefaultAsync(x => x.ModuleId == moduleId);
            //.OrderByDescending(x => x.CreationTime)
            //.FirstOrDefaultAsync(x => x.ModuleId == moduleId);

            return ObjectMapper.Map<Attachment, AttachmentDto>(result);
        }

        public async Task<AttachmentFileDto> GetFileWithBytesAsync(Guid id)
        {
            var result = await _AttachmentRepository.FindAsync(id);
            if (result != null)
            {

                var ext = Path.GetExtension(result.FileName);
                if (!".png,.svg,.gif,.jpg,.jpeg".Contains(ext.ToLower()) || result.FileName.Contains("\\") || result.FileName.Contains("/") || result.FileName.Contains(",") || result.FileName.Length > 100)
                {
                    return null;
                }
                if (string.IsNullOrWhiteSpace(result.FileName))
                {
                    return null;
                }
                //var byt = await _attachmentContenair.GetAllBytesAsync(result.FileName);
                //if (byt != null)
                //{
                //    var _originalFileName = result.OriginalFileName ?? result.FileName;
                //    return new AttachmentFileDto { ByteContent = byt, OriginalFileName = _originalFileName, MimeType = MimeMapping.MimeUtility.GetMimeMapping(_originalFileName) };
                //}
            }
            throw new NotFoundException();
        }

        public async Task<AttachmentFileDto> GetFileWithBytesByModuleIdAsync(Guid moduleId)
        {
            var result = await _AttachmentRepository.FirstOrDefaultAsync(x => x.ModuleId == moduleId);
            if (result != null)
            {

                var ext = Path.GetExtension(result.FileName);
                if (!".png,.svg,.gif,.jpg,.jpeg".Contains(ext.ToLower()) || result.FileName.Contains("\\") || result.FileName.Contains("/") || result.FileName.Contains(",") || result.FileName.Length > 100)
                {
                    return null;
                }
                if (string.IsNullOrWhiteSpace(result.FileName))
                {
                    return null;
                }

                var byt = await _attachmentContenair.GetAllBytesAsync(result.FileName);
                //if (byt != null)
                //{
                //    var _originalFileName = result.OriginalFileName ?? result.FileName;
                //    return new AttachmentFileDto { ByteContent = byt, OriginalFileName = _originalFileName, MimeType = MimeMapping.MimeUtility.GetMimeMapping(_originalFileName) };
                //}
            }
            throw new NotFoundException();
        }


        public async Task<List<AttachmentDto>> GetByModuleIdAsync(Guid moduleId)
        {
            var result = await _AttachmentRepository.GetListAsync(x => x.ModuleId == moduleId);
            var attachments = ObjectMapper.Map<List<Attachment>, List<AttachmentDto>>(result);
            return attachments;
        }

        public async Task<List<AttachmentDto>> GetByModuleIdsAsync(List<Guid> moduleIds)
        {
            var result = await _AttachmentRepository.GetListAsync(x => moduleIds.Contains(x.ModuleId));
            var attachments = ObjectMapper.Map<List<Attachment>, List<AttachmentDto>>(result);
            return attachments;
        }


    }
}
