using Application.Contracts.EntityDto;
using System;

namespace AlArz.Dtos
{
    public class ConfigDto : BaseEntityDto
    {
        public Guid TypeId { get; set; }

        public string Value { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid? ProjectsId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
