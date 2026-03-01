using Microsoft.EntityFrameworkCore;

namespace Notification.Infrastructure.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

        public DbSet<Notification.Domain.Entities.NotificationLog> Notifications
            => Set<Notification.Domain.Entities.NotificationLog>();
    }
}
