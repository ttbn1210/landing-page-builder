using QrCommerce.Android.Services;

namespace QrCommerce.Android.Workers;

public class PollingWorker
{
    private readonly ApiService _apiService;
    private readonly SmsService _smsService;
    private readonly StorageService _storage;
    private CancellationTokenSource? _cts;
    private bool _isRunning;

    public event Action<string>? OnLog;
    public int TotalSmsSent { get; private set; }
    public int PendingJobs { get; private set; }
    public DateTime? LastSyncTime { get; private set; }
    public bool IsRunning => _isRunning;

    public PollingWorker(ApiService apiService, SmsService smsService, StorageService storage)
    {
        _apiService = apiService;
        _smsService = smsService;
        _storage = storage;
    }

    public void Start()
    {
        if (_isRunning) return;
        _isRunning = true;
        _cts = new CancellationTokenSource();
        _ = RunAsync(_cts.Token);
    }

    public void Stop()
    {
        _isRunning = false;
        _cts?.Cancel();
    }

    private async Task RunAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                await CheckJobsAsync();
                await HeartbeatAsync();
                LastSyncTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Error: {ex.Message}");
            }

            try { await Task.Delay(5000, token); }
            catch (TaskCanceledException) { break; }
        }
    }

    private async Task CheckJobsAsync()
    {
        var jobs = await _apiService.GetPendingJobsAsync();
        PendingJobs = jobs.Count;

        foreach (var job in jobs)
        {
            OnLog?.Invoke($"Sending SMS to {job.PhoneNumber}...");
            var success = await SendSmsAsync(job.PhoneNumber, job.Message);

            if (success)
            {
                await _apiService.ReportJobAsync(job.Id, "Sent", "OK");
                TotalSmsSent++;
                OnLog?.Invoke($"SMS sent to {job.PhoneNumber}");
            }
            else
            {
                await _apiService.ReportJobAsync(job.Id, "Failed", "SMS_SEND_ERROR");
                OnLog?.Invoke($"Failed to send SMS to {job.PhoneNumber}");
            }
        }
    }

    private async Task<bool> SendSmsAsync(string phone, string message)
    {
        return await _smsService.SendSmsAsync(phone, message);
    }

    private async Task HeartbeatAsync()
    {
        var battery = GetBatteryLevel();
        var network = GetNetworkType();
        await _apiService.SendHeartbeatAsync(battery, network);
    }

    private int GetBatteryLevel()
    {
        try
        {
            return (int)(Battery.Default.ChargeLevel * 100);
        }
        catch
        {
            return 50;
        }
    }

    private string GetNetworkType()
    {
        try
        {
            var profiles = Connectivity.Current.ConnectionProfiles;
            if (profiles.Contains(ConnectionProfile.WiFi)) return "WiFi";
            if (profiles.Contains(ConnectionProfile.Cellular)) return "4G";
            return "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }
}
