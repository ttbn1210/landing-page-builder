using QrCommerce.Web.Services;

namespace QrCommerce.Web.Background;

public class TelecomRetryWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TelecomRetryWorker> _logger;

    public TelecomRetryWorker(IServiceProvider serviceProvider, ILogger<TelecomRetryWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var telecomService = scope.ServiceProvider.GetRequiredService<TelecomService>();
                await telecomService.RetryFailedAsync();

                var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
                await deviceService.MarkOfflineDevicesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TelecomRetryWorker");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
