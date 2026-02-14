using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;

namespace OnlineCoursesPlatform.Services.BackgroundServices
{
    public class LiveSessionCleanerWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LiveSessionCleanerWorker> _logger;

        public LiveSessionCleanerWorker(IServiceProvider serviceProvider, ILogger<LiveSessionCleanerWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Cleaning Ghost Sessions at: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediaService = scope.ServiceProvider.GetRequiredService<MediaService>();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var realActiveKeys = await mediaService.GetActiveStreamKeysAsync();

                    var dbActiveSessions = await db.LiveSessions
                        .Where(s => s.IsActive && !s.IsDeleted)
                        .ToListAsync();

                    foreach (var session in dbActiveSessions)
                    {
                        if (!realActiveKeys.Contains(session.StreamKey))
                        {
                            session.IsActive = false;
                            session.EndTime = DateTime.Now;
                            _logger.LogWarning($"Session {session.StreamKey} was ghosted and has been closed.");
                        }
                    }

                    await db.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}
