using AlArz.Entity;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AlArz.EntityFrameworkCore
{
    public partial class AlArzDbContext
    {
        public DbSet<Config> Configs { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<LookupType> LookupTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationTemplate> NotificationsTemplates { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<BlockedUserToken> BlockedUserTokens { get; set; }
        public DbSet<RegisterFireBase> RegisterFireBases { get; set; }
        public DbSet<CustomLog> CustomLogs { get; set; }

    }
}