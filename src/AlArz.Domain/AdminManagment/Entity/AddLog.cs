using Domain.Entity;

namespace AlArz.Entity
{
    public class CustomLog : BaseEntity
    {
        public string Response { get; set; } = string.Empty;
        public string Request { get; set; } = string.Empty;
        public string ERPReference { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Result { get; set; } = true;
    }
}
