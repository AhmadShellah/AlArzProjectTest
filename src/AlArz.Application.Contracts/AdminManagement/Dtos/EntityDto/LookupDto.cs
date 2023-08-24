using Application.Contracts.EntityDto;
using System;

namespace AlArz.Dtos
{
    public class LookupDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public Guid? Parent { get; set; }
        public LookupDto ParentLookup { get; set; }

        public int Code { get; set; }

        public bool Enabled { get; set; }

        //Navigation 
        public Guid LookupTypeId { get; set; }

        public virtual LookupTypeDto LookupType { get; set; }

    }
}
