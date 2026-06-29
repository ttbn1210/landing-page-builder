using Microsoft.Maui.Hosting;
using QrCommerce.Android.Services;
using QrCommerce.Android.Workers;

namespace QrCommerce.Android;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<SmsService>();
        builder.Services.AddSingleton<PollingWorker>();
        builder.Services.AddSingleton<StorageService>();

        builder.Services.AddTransient<Pages.DashboardPage>();
        builder.Services.AddTransient<Pages.LogsPage>();
        builder.Services.AddTransient<Pages.SettingsPage>();

        return builder.Build();
    }
}
