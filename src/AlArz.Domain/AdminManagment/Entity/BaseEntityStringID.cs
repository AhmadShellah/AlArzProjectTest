using Volo.Abp.Domain.Entities.Auditing;

namespace Domain.Entity
{
    public class BaseEntityStringID : FullAuditedAggregateRoot<string>
    {
    }
}
