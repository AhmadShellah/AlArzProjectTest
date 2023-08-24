using Application.Contracts.EntityDto;
using System;

namespace AlArz.Dtos
{
    public class AttachmentDto : BaseEntityDto
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string Content { get; set; }
        public Guid ModuleId { get; set; }
        public Guid? AttachmentTypeId { get; set; }
        public Guid ModuleTypeId { get; set; }
        public AttachmentDto Attachment { get; set; }

    }

    public class AttachmentFileDto : BaseEntityDto
    {
        public string OriginalFileName { get; set; }
        public string MimeType { get; set; }
        public byte[] ByteContent { get; set; }
    }
}
