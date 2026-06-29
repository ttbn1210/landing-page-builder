using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using QrCommerce.Shared.DTOs;

namespace QrCommerce.Android.Services;

public class ApiService
{
    private readonly HttpClient _client;
    private readonly StorageService _storage;

    public ApiService(StorageService storage)
    {
        _storage = storage;
        _client = new HttpClient();
    }

    private void ConfigureClient()
    {
        _client.BaseAddress = new Uri(_storage.GetServerUrl());
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("X-API-KEY", _storage.GetApiKey());
    }

    public async Task<List<TelecomJobDto>> GetPendingJobsAsync()
    {
        try
        {
            ConfigureClient();
            var response = await _client.GetAsync("/api/device/jobs");
            if (!response.IsSuccessStatusCode) return new List<TelecomJobDto>();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TelecomJobDto>>(json) ?? new List<TelecomJobDto>();
        }
        catch
        {
            return new List<TelecomJobDto>();
        }
    }

    public async Task<bool> ReportJobAsync(int jobId, string status, string rawResponse = "")
    {
        try
        {
            ConfigureClient();
            var dto = new JobReportDto { JobId = jobId, Status = status, RawResponse = rawResponse };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/device/report", content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendHeartbeatAsync(int battery, string network)
    {
        try
        {
            ConfigureClient();
            var dto = new DeviceHeartbeatDto
            {
                DeviceCode = _storage.GetDeviceCode(),
                Battery = battery,
                Network = network
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/device/heartbeat", content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> RegisterDeviceAsync(string deviceCode, string deviceName, string phoneNumber, string carrier)
    {
        try
        {
            ConfigureClient();
            var dto = new DeviceRegisterDto
            {
                DeviceCode = deviceCode,
                DeviceName = deviceName,
                PhoneNumber = phoneNumber,
                Carrier = carrier
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/device/register", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(json)!;
                return (string)result.apiKey;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}
