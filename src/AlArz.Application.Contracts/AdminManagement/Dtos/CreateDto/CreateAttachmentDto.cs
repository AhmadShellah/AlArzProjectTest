using System;

namespace AlArz.Dtos
{
    public class CreateBaseAttachmentDto
    {
        public string OriginalFileName { get; set; }
        public string Content { get; set; }
    }

    public class CreateAttachmentDto : CreateBaseAttachmentDto
    {
        public Guid? Id { get; set; }
        public string FileName { get; set; }
        public Guid? ModuleId { get; set; }
        public Guid? AttachmentTypeId { get; set; }
        public Guid? ModuleTypeId { get; set; }
    }
}
