using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class NotificationTemplate : BaseEntity
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public string NameSpace { get; set; }

        //Navigation 

        [ForeignKey(nameof(Event))]
        public Guid EventId { get; set; }
        public virtual Lookup Event { get; set; }

    }
}
