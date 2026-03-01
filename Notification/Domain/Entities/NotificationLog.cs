namespace Notification.Domain.Entities
{
    public class NotificationLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = "EnrollmentApproved";
        public Guid EnrollmentId { get; set; }
        public string Message { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
