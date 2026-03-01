using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notification.Contracts;
using Notification.Infrastructure.Data;

namespace Notification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationDbContext _db;
        public NotificationsController(NotificationDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationRequest req)
        {
            var log = new Notification.Domain.Entities.NotificationLog
            {
                EnrollmentId = req.EnrollmentId,
                Message = req.Message
            };

            _db.Notifications.Add(log);
            await _db.SaveChangesAsync();
            return Ok(log);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _db.Notifications.OrderByDescending(x => x.CreatedAtUtc).ToListAsync());
    }
}
