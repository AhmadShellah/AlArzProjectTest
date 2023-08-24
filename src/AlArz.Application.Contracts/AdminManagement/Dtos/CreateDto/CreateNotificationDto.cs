using System;

namespace AlArz.Dtos
{
    public class CreateNotificationDto
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public string Url { get; set; }

        public string Image { get; set; }

        public bool IsSent { get; set; }

        public int TryCount { get; set; }

        public Guid? User { get; set; }

        public Guid? Role { get; set; }

        public int? Event { get; set; }

        public Guid TypeId { get; set; }
    }
}
