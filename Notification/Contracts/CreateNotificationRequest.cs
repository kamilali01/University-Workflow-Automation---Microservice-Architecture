namespace Notification.Contracts
{
    public record CreateNotificationRequest(Guid EnrollmentId, string Message);
}
