using Microsoft.Maui.Storage;

namespace QrCommerce.Android.Services;

public class StorageService
{
    private const string ServerUrlKey = "server_url";
    private const string ApiKeyKey = "api_key";
    private const string DeviceCodeKey = "device_code";

    public string GetServerUrl() => Preferences.Get(ServerUrlKey, "http://10.0.2.2:5000");
    public void SetServerUrl(string url) => Preferences.Set(ServerUrlKey, url);

    public string GetApiKey() => Preferences.Get(ApiKeyKey, "");
    public void SetApiKey(string key) => Preferences.Set(ApiKeyKey, key);

    public string GetDeviceCode() => Preferences.Get(DeviceCodeKey, "DEVICE_01");
    public void SetDeviceCode(string code) => Preferences.Set(DeviceCodeKey, code);
}
