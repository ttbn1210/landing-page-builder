using Microsoft.AspNetCore.Mvc;
using QrCommerce.Shared.DTOs;
using QrCommerce.Shared.Enums;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Api;

[ApiController]
[Route("api/device")]
public class DeviceController : ControllerBase
{
    private readonly TelecomService _telecomService;
    private readonly DeviceService _deviceService;

    public DeviceController(TelecomService telecomService, DeviceService deviceService)
    {
        _telecomService = telecomService;
        _deviceService = deviceService;
    }

    [HttpGet("jobs")]
    public async Task<IActionResult> GetJobs()
    {
        var device = HttpContext.Items["Device"] as TelecomDevice;
        if (device == null) return Unauthorized();

        var jobs = await _telecomService.GetPendingJobsAsync(device.Id);
        return Ok(jobs);
    }

    [HttpPost("report")]
    public async Task<IActionResult> Report([FromBody] JobReportDto dto)
    {
        if (string.IsNullOrEmpty(dto.Status))
            return BadRequest(new { error = "Status required" });

        if (dto.Status.Equals("Sent", StringComparison.OrdinalIgnoreCase))
        {
            await _telecomService.MarkSentAsync(dto.JobId, dto.RawResponse);
        }
        else
        {
            await _telecomService.MarkFailedAsync(dto.JobId, dto.RawResponse);
        }

        return Ok(new { success = true });
    }

    [HttpPost("heartbeat")]
    public async Task<IActionResult> Heartbeat([FromBody] DeviceHeartbeatDto dto)
    {
        if (string.IsNullOrEmpty(dto.DeviceCode))
            return BadRequest(new { error = "DeviceCode required" });

        await _deviceService.HeartbeatAsync(dto);
        return Ok(new { success = true });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] DeviceRegisterDto dto)
    {
        if (string.IsNullOrEmpty(dto.DeviceCode) || string.IsNullOrEmpty(dto.PhoneNumber))
            return BadRequest(new { error = "DeviceCode and PhoneNumber required" });

        var device = await _deviceService.RegisterAsync(dto);
        return Ok(new { device.Id, device.ApiKey, device.DeviceCode });
    }
}
