using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Domain.Entity
{
    public class BaseEntity : FullAuditedAggregateRoot<Guid>
    {
    }
}
