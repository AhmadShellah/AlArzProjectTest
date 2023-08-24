using Application.Contracts.EntityDto;

namespace AlArz.Dtos
{
    public class NotificationTemplateDto : BaseEntityDto
    {
        public string Title { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public string NameSpace { get; set; } = string.Empty;

        //Navigation 
        public LookupDto NotificationType { get; set; }

        public LookupDto Event { get; set; }

    }
}
