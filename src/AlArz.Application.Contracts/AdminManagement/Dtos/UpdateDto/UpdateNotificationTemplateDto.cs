using System;

namespace AlArz.Dtos
{
    public class UpdateNotificationTemplateDto : CreateNotificationTemplateDto
    {
        public Guid Id { get; set; }
    }
}
