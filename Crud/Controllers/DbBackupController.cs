using Crud.Contracts;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    public class DbBackupController : Controller
    {
        private readonly IJobService _jobService;

        public DbBackupController(IJobService jobService)
        {
            _jobService = jobService;
        }

        // Immediate backup (fire-and-forget job)
        [HttpPost("run-now")]
        public async Task<IActionResult> RunBackupNow()
        {
            await _jobService.BackupDatabaseAsync();
            return Ok("Backup job has been completed.");
        }


        // Setup recurring backup (e.g., daily at midnight)
        [HttpPost("schedule")]
        public IActionResult ScheduleBackup()
        {
            RecurringJob.AddOrUpdate("daily-db-backup",
                () => _jobService.BackupDatabaseAsync(),
                Cron.Daily); // runs once a day at 00:00
            return Ok("Recurring backup scheduled.");
        }
    }
}
