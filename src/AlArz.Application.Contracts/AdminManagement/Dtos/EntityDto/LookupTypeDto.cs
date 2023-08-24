using Application.Contracts.EntityDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlArz.Dtos
{
    public class LookupTypeDto : BaseEntityDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public int LookupTypeNumber { get; set; } 
    }

    public class LookupTypeGroupDto : BaseEntityDto
    {
        public Guid LookupTypeId { get; set; }
        public int LookupTypeNumber { get; set; }

        public IEnumerable<LookupDto> Lookups { get; set; } = new List<LookupDto>();
    }
}
