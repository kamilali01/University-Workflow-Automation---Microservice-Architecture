using Enrollment.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enrollment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly EnrollmentDbContext _db;
        public AuditLogsController(EnrollmentDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetLatest()
            => Ok(await _db.AuditLogs.OrderByDescending(x => x.CreatedAtUtc).Take(50).ToListAsync());
    }
}
