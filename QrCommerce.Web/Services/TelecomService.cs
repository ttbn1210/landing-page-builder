using Microsoft.EntityFrameworkCore;
using QrCommerce.Shared.DTOs;
using QrCommerce.Shared.Enums;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Data;

namespace QrCommerce.Web.Services;

public class TelecomService
{
    private readonly AppDbContext _db;

    public TelecomService(AppDbContext db)
    {
        _db = db;
    }

    public async Task EnqueueAsync(string phoneNumber, string message, int priority = 1)
    {
        var device = await _db.TelecomDevices.FirstOrDefaultAsync(d => d.IsOnline);
        var deviceId = device?.Id ?? 1;

        var job = new TelecomJob
        {
            DeviceId = deviceId,
            PhoneNumber = phoneNumber,
            Message = message,
            Priority = priority,
            Status = TelecomJobStatus.Pending
        };

        _db.TelecomJobs.Add(job);
        await _db.SaveChangesAsync();
    }

    public async Task<List<TelecomJobDto>> GetPendingJobsAsync(int deviceId)
    {
        return await _db.TelecomJobs
            .Where(j => j.DeviceId == deviceId && j.Status == TelecomJobStatus.Pending)
            .OrderBy(j => j.Priority)
            .ThenBy(j => j.CreatedAt)
            .Take(10)
            .Select(j => new TelecomJobDto
            {
                Id = j.Id,
                PhoneNumber = j.PhoneNumber,
                Message = j.Message,
                Priority = j.Priority
            })
            .ToListAsync();
    }

    public async Task MarkSendingAsync(int jobId)
    {
        var job = await _db.TelecomJobs.FindAsync(jobId);
        if (job != null)
        {
            job.Status = TelecomJobStatus.Sending;
            await _db.SaveChangesAsync();
        }
    }

    public async Task MarkSentAsync(int jobId, string rawResponse = "")
    {
        var job = await _db.TelecomJobs.Include(j => j.Logs).FirstOrDefaultAsync(j => j.Id == jobId);
        if (job != null)
        {
            job.Status = TelecomJobStatus.Sent;
            job.SentAt = DateTime.UtcNow;

            var log = new TelecomLog
            {
                TelecomJobId = jobId,
                PhoneNumber = job.PhoneNumber,
                Message = job.Message,
                Status = TelecomJobStatus.Sent,
                RawResponse = rawResponse
            };
            _db.TelecomLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }

    public async Task MarkFailedAsync(int jobId, string rawResponse = "")
    {
        var job = await _db.TelecomJobs.FindAsync(jobId);
        if (job != null)
        {
            job.Status = TelecomJobStatus.Failed;
            job.RetryCount++;

            var log = new TelecomLog
            {
                TelecomJobId = jobId,
                PhoneNumber = job.PhoneNumber,
                Message = job.Message,
                Status = TelecomJobStatus.Failed,
                RawResponse = rawResponse
            };
            _db.TelecomLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }

    public async Task RetryFailedAsync()
    {
        var failedJobs = await _db.TelecomJobs
            .Where(j => j.Status == TelecomJobStatus.Failed && j.RetryCount < 3)
            .ToListAsync();

        foreach (var job in failedJobs)
        {
            job.Status = TelecomJobStatus.Pending;
        }

        await _db.SaveChangesAsync();
    }

    public async Task<List<TelecomJob>> GetJobsAsync(int page = 1, int pageSize = 20, string? search = null)
    {
        var query = _db.TelecomJobs
            .Include(j => j.Device)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(j => j.PhoneNumber.Contains(search) || j.Message.Contains(search));
        }

        return await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetJobsCountAsync(string? search = null)
    {
        var query = _db.TelecomJobs.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(j => j.PhoneNumber.Contains(search) || j.Message.Contains(search));
        }
        return await query.CountAsync();
    }

    public async Task<List<TelecomLog>> GetLogsAsync(int page = 1, int pageSize = 20)
    {
        return await _db.TelecomLogs
            .Include(l => l.TelecomJob)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetLogsCountAsync()
    {
        return await _db.TelecomLogs.CountAsync();
    }
}
