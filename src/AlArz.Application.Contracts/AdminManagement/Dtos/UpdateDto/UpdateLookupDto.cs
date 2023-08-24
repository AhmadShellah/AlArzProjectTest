using System;

namespace AlArz.Dtos
{
    public class UpdateLookupDto : CreateLookupDto
    {
        public Guid Id { get; set; }
    }
}
