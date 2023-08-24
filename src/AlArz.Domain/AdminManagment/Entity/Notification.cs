using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Notification : BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }

        public string Body { get; set; }

        public string Url { get; set; }

        public string Image { get; set; }

        public bool IsSent { get; set; }

        public int TryCount { get; set; }

        public Guid? User { get; set; }

        public Guid? Role { get; set; }
        public int? Event { get; set; }

        public string RecordId { get; set; }

        public bool IsRead { get; set; }

        //Navigation
        [ForeignKey(nameof(Type))]
        public Guid TypeId { get; set; }
        public Lookup Type { get; set; }

    }
    public class BaseNotification
    {
        public string UpdateMood { get; set; }
    }
    public class Root
    {
        public List<Notification> notifications { get; set; }
    }
}
