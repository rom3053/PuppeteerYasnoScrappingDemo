using WebScrappingDemo.Services;

namespace WebScrappingDemo.Background;

public class BrowserSessionCleanerJob : BackgroundService
{
    private readonly ILogger<BrowserSessionCleanerJob> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _periodicTimer;

    public BrowserSessionCleanerJob(IServiceProvider serviceProvider,
        ILogger<BrowserSessionCleanerJob> logger)
    {
        _periodicTimer = new(TimeSpan.FromSeconds(90));
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (await _periodicTimer.WaitForNextTickAsync(cancellationToken) &&
            !cancellationToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Background {JobName} service executed", nameof(BrowserSessionCleanerJob));
                await using var scope = _serviceProvider.CreateAsyncScope();
                var browserSessionStorage = scope.ServiceProvider.GetRequiredService<BrowserSessionStorage>();
                var sessions = browserSessionStorage.Sessions;

                var dateTimeNow = DateTime.UtcNow;
                var expiredKeys = sessions.Where(x => x.Value.CreatedAt + TimeSpan.FromMinutes(5) < dateTimeNow)
                    .Select(x => x.Key)
                    .ToArray();

                foreach (var expiredKey in expiredKeys)
                {
                    sessions.TryRemove(expiredKey, out var session);
                    session.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
