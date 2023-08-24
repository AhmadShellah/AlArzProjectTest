using Application.Contracts.EntityDto;

namespace AlArz.AdminManagement
{
    public class CustomLog : BaseEntityDto
    {
        public object Response { get; set; } = string.Empty;
        public object Request { get; set; } = string.Empty;
        public string ERPReference { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Result { get; set; } = true;
    }
}
