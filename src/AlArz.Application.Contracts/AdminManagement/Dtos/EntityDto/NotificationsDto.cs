using Application.Contracts.EntityDto;
using System;
using Volo.Abp.Application.Dtos;

namespace AlArz.Dtos
{
    public class NotificationsDto : BaseEntityDto
    {
        public string Title { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public bool IsSent { get; set; } = false;

        public int TryCount { get; set; }

        public Guid? User { get; set; }

        public Guid? Role { get; set; }

        public Guid ProjectId { get; set; }
        public Guid TypeId { get; set; }
        public LookupDto Type { get; set; }
    }

    public class CustomPagedResultDto : PagedResultDto<NotificationsDto>
    {
        public int CountIsNotRead { get; set; }
    }
}
