using System;

namespace AlArz.Dtos
{
    public class CreateNotificationTemplateDto
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public Guid? EventId { get; set; }

        public string NameSpace { get; set; }

        public Guid? ProjectId { get; set; }

        public Guid? NotificationTypeId { get; set; }



    }
}
