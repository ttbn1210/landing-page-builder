using QrCommerce.Android.Services;

namespace QrCommerce.Android.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly StorageService _storage;
    private readonly ApiService _apiService;
    private readonly SmsService _smsService;

    public SettingsPage(StorageService storage, ApiService apiService, SmsService smsService)
    {
        InitializeComponent();
        _storage = storage;
        _apiService = apiService;
        _smsService = smsService;
        LoadSettings();
    }

    private void LoadSettings()
    {
        ServerUrlEntry.Text = _storage.GetServerUrl();
        DeviceCodeEntry.Text = _storage.GetDeviceCode();
        ApiKeyEntry.Text = _storage.GetApiKey();
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        _storage.SetServerUrl(ServerUrlEntry.Text ?? "");
        _storage.SetDeviceCode(DeviceCodeEntry.Text ?? "");
        _storage.SetApiKey(ApiKeyEntry.Text ?? "");
        await DisplayAlert("Success", "Settings saved", "OK");
    }

    private async void OnRegisterClicked(object? sender, EventArgs e)
    {
        var code = RegDeviceCode.Text?.Trim();
        var name = RegDeviceName.Text?.Trim();
        var phone = RegPhoneNumber.Text?.Trim();
        var carrier = RegCarrier.Text?.Trim();

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(phone))
        {
            await DisplayAlert("Error", "Device code and phone number are required", "OK");
            return;
        }

        var apiKey = await _apiService.RegisterDeviceAsync(code!, name ?? "", phone!, carrier ?? "");
        if (apiKey != null)
        {
            _storage.SetApiKey(apiKey);
            _storage.SetDeviceCode(code!);
            ApiKeyEntry.Text = apiKey;
            DeviceCodeEntry.Text = code;
            await DisplayAlert("Success", $"Device registered.\nAPI Key: {apiKey}", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Registration failed", "OK");
        }
    }

    private async void OnPermissionClicked(object? sender, EventArgs e)
    {
        var granted = await _smsService.RequestPermissionsAsync();
        await DisplayAlert("Permission", granted ? "SMS permission granted" : "SMS permission denied", "OK");
    }
}
