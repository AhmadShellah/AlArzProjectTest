using System;

namespace AlArz.Dtos
{
    public class CreateConfigDto
    {
        public string Value { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        //Navigation 
        public Guid? ProjectsId { get; set; }

        public Guid TypeId { get; set; }
    }
}
