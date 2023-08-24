using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Config : BaseEntity
    {
        public string Value { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }
        //Navigation 


        [ForeignKey(nameof(Lookup))]
        public Guid TypeId { get; set; }
        public virtual Lookup Lookup { get; set; }
        public Guid NewCol { get; set; }

    }
}
