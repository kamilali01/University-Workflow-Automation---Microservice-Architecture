using Enrollment.Contracts;
using Enrollment.Domain.Entities;
using Enrollment.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly EnrollmentDbContext _db;
        private readonly IHttpClientFactory _http;

        public EnrollmentsController(EnrollmentDbContext db, IHttpClientFactory http)
        {
            _db = db;
            _http = http;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentRequest req)
        {
            // Validate student exists
            var studentClient = _http.CreateClient("Student");
            var studentResp = await studentClient.GetAsync($"/api/students/{req.StudentId}");
            if (!studentResp.IsSuccessStatusCode)
                return BadRequest("Student not found.");

            // Validate course exists
            var courseClient = _http.CreateClient("Course");
            var courseResp = await courseClient.GetAsync($"/api/courses/{req.CourseId}");
            if (!courseResp.IsSuccessStatusCode)
                return BadRequest("Course not found.");

            // Prevent duplicates
            var already = await _db.Enrollments.AnyAsync(x => x.StudentId == req.StudentId && x.CourseId == req.CourseId);
            if (already) return Conflict("Student already enrolled in this course.");

            var enrollment = new Enrollment.Domain.Entities.Enrollment
            {
                StudentId = req.StudentId,
                CourseId = req.CourseId,
                Status = EnrollmentStatus.Pending
            };

            _db.Enrollments.Add(enrollment);
            _db.AuditLogs.Add(new AuditLog
            {
                Action = "EnrollmentCreated",
                EntityId = enrollment.Id,
                Details = $"StudentId={req.StudentId};CourseId={req.CourseId}"
            });

            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = enrollment.Id }, enrollment);
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateEnrollmentStatusRequest req)
        {
            var enrollment = await _db.Enrollments.FindAsync(id);
            if (enrollment is null) return NotFound();

            enrollment.Status = req.Status;

            _db.AuditLogs.Add(new AuditLog
            {
                Action = "EnrollmentStatusChanged",
                EntityId = enrollment.Id,
                Details = $"NewStatus={enrollment.Status}"
            });

            await _db.SaveChangesAsync();

            // Notify on Approved
            if (enrollment.Status == EnrollmentStatus.Approved)
            {
                var notificationClient = _http.CreateClient("Notification");
                var payload = new
                {
                    enrollmentId = enrollment.Id,
                    message = $"Enrollment {enrollment.Id} approved (simulated)."
                };

                // Fire-and-forget simplified (await to see errors during demo)
                var resp = await notificationClient.PostAsJsonAsync("/api/notifications", payload);

                _db.AuditLogs.Add(new AuditLog
                {
                    Action = "NotificationRequested",
                    EntityId = enrollment.Id,
                    Details = $"NotificationServiceStatus={(int)resp.StatusCode}"
                });
                await _db.SaveChangesAsync();
            }

            return Ok(enrollment);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var e = await _db.Enrollments.FindAsync(id);
            return e is null ? NotFound() : Ok(e);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _db.Enrollments.OrderByDescending(x => x.CreatedAtUtc).ToListAsync());
    }
}
