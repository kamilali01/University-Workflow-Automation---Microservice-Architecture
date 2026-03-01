namespace Enrollment.Contracts
{
    public record CreateEnrollmentRequest(Guid StudentId, Guid CourseId);
}
