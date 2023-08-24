using System;

namespace AlArz.Dtos
{
    public class CreateLookupDto
    {
        public string Name { get; set; }

        public string NameAr { get; set; }

        public int Code { get; set; }

        public Guid? Parent { get; set; }

        public Guid LookupTypeId { get; set; }

        public bool Enabled { get; set; }


    }
}
