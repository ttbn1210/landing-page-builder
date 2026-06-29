using Microsoft.EntityFrameworkCore;
using QrCommerce.Shared.DTOs;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Data;

namespace QrCommerce.Web.Services;

public class DeviceService
{
    private readonly AppDbContext _db;

    public DeviceService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TelecomDevice?> ValidateApiKeyAsync(string apiKey)
    {
        return await _db.TelecomDevices.FirstOrDefaultAsync(d => d.ApiKey == apiKey);
    }

    public async Task<TelecomDevice> RegisterAsync(DeviceRegisterDto dto)
    {
        var device = new TelecomDevice
        {
            DeviceCode = dto.DeviceCode,
            DeviceName = dto.DeviceName,
            PhoneNumber = dto.PhoneNumber,
            Carrier = dto.Carrier,
            ApiKey = Guid.NewGuid().ToString("N"),
            IsOnline = true,
            LastHeartbeat = DateTime.UtcNow
        };

        _db.TelecomDevices.Add(device);
        await _db.SaveChangesAsync();
        return device;
    }

    public async Task HeartbeatAsync(DeviceHeartbeatDto dto)
    {
        var device = await _db.TelecomDevices.FirstOrDefaultAsync(d => d.DeviceCode == dto.DeviceCode);
        if (device != null)
        {
            device.BatteryLevel = dto.Battery;
            device.NetworkType = dto.Network;
            device.IsOnline = true;
            device.LastHeartbeat = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<TelecomDevice>> GetDevicesAsync(int page = 1, int pageSize = 20)
    {
        return await _db.TelecomDevices
            .OrderByDescending(d => d.LastHeartbeat)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetDevicesCountAsync()
    {
        return await _db.TelecomDevices.CountAsync();
    }

    public async Task MarkOfflineDevicesAsync()
    {
        var threshold = DateTime.UtcNow.AddMinutes(-2);
        var offlineDevices = await _db.TelecomDevices
            .Where(d => d.IsOnline && d.LastHeartbeat < threshold)
            .ToListAsync();

        foreach (var device in offlineDevices)
        {
            device.IsOnline = false;
        }

        await _db.SaveChangesAsync();
    }
}
