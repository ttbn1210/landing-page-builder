using System.Collections.ObjectModel;
using QrCommerce.Android.Workers;

namespace QrCommerce.Android.Pages;

public partial class LogsPage : ContentPage
{
    private readonly PollingWorker _worker;
    private readonly ObservableCollection<string> _logs = new();

    public LogsPage(PollingWorker worker)
    {
        InitializeComponent();
        _worker = worker;
        LogsListView.ItemsSource = _logs;
        _worker.OnLog += msg =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _logs.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {msg}");
                if (_logs.Count > 100) _logs.RemoveAt(_logs.Count - 1);
            });
        };
    }

    private void OnClearClicked(object? sender, EventArgs e)
    {
        _logs.Clear();
    }
}
