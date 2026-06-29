using QrCommerce.Android.Workers;

namespace QrCommerce.Android.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly PollingWorker _worker;
    private IDispatcherTimer? _timer;

    public DashboardPage(PollingWorker worker)
    {
        InitializeComponent();
        _worker = worker;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(2);
        _timer.Tick += (s, e) => UpdateUI();
        _timer.Start();
        UpdateUI();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
    }

    private void UpdateUI()
    {
        StatusLabel.Text = _worker.IsRunning ? "Running" : "Stopped";
        StatusLabel.TextColor = _worker.IsRunning ? Colors.LightGreen : Colors.White;
        BatteryLabel.Text = $"{(int)(Battery.Default.ChargeLevel * 100)}%";
        SmsSentLabel.Text = _worker.TotalSmsSent.ToString();
        PendingLabel.Text = _worker.PendingJobs.ToString();
        LastSyncLabel.Text = _worker.LastSyncTime?.ToString("HH:mm:ss") ?? "--";
        StartStopButton.Text = _worker.IsRunning ? "Stop Service" : "Start Service";
        StartStopButton.BackgroundColor = _worker.IsRunning ? Colors.Red : Color.FromArgb("#28a745");
    }

    private void OnStartStopClicked(object? sender, EventArgs e)
    {
        if (_worker.IsRunning)
            _worker.Stop();
        else
            _worker.Start();
        UpdateUI();
    }
}
