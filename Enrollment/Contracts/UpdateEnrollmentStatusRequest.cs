using Enrollment.Domain.Entities;

namespace Enrollment.Contracts
{
    public record UpdateEnrollmentStatusRequest(EnrollmentStatus Status);
}
