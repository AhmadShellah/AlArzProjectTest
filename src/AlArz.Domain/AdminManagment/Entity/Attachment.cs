using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Attachment : BaseEntity
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public Guid ModuleId { get; set; }
        public Guid? AttachmentTypeId { get; set; }
        public Guid ModuleTypeId { get; set; }

        //Navigation

        [ForeignKey(nameof(ModuleTypeId))]
        public Lookup ModuleType { get; set; }

        [ForeignKey(nameof(AttachmentTypeId))]
        public Lookup AttachmentType { get; set; }

    }
}
