using System.ComponentModel.DataAnnotations;

namespace AlArz.Dtos
{
    public class CreateLookupTypeDto
    {
        [Required]
        public string Name { get; set; }
        public string NameAr { get; set; }
        public bool Enabled { get; set; }
    }
}
