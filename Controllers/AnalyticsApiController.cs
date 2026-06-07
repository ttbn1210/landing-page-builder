using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LandingPageBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller}")]
    [Authorize]
    public class AnalyticsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AnalyticsApiController> _logger;

        public AnalyticsApiController(ApplicationDbContext context, ILogger<AnalyticsApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("page/{pageId:int}")]
        public async Task<ActionResult> GetPageAnalytics(int pageId)
        {
            var page = await _context.LandingPages.FindAsync(pageId);
            if (page == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page.UserId != userId)
                return Forbid();

            var analytics = _context.PageAnalytics
                .Where(a => a.LandingPageId == pageId)
                .AsEnumerable()
                .GroupBy(a => a.VisitedAt.Date)
                .Select(g => new
                {
                    date = g.Key,
                    views = g.Count()
                })
                .OrderByDescending(x => x.date)
                .Take(30)
                .ToList();

            var totalViews = _context.PageAnalytics.Count(a => a.LandingPageId == pageId);
            var uniqueVisitors = _context.PageAnalytics.Where(a => a.LandingPageId == pageId).Select(a => a.IpAddress).Distinct().Count();

            return Ok(new
            {
                totalViews,
                uniqueVisitors,
                dailyData = analytics
            });
        }

        [HttpPost("track")]
        [AllowAnonymous]
        public async Task<ActionResult> TrackPageView([FromBody] TrackingData data)
        {
            try
            {
                var pageAnalytic = new PageAnalytics
                {
                    LandingPageId = data.PageId,
                    Referrer = data.Referrer,
                    UserAgent = Request.Headers["User-Agent"].ToString(),
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Device = GetDeviceType(Request.Headers["User-Agent"].ToString())
                };

                _context.PageAnalytics.Add(pageAnalytic);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking page view");
                return StatusCode(500);
            }
        }

        private string GetDeviceType(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return "Unknown";

            userAgent = userAgent.ToLower();
            if (userAgent.Contains("mobile") || userAgent.Contains("android"))
                return "Mobile";
            if (userAgent.Contains("tablet") || userAgent.Contains("ipad"))
                return "Tablet";
            return "Desktop";
        }
    }

    public class TrackingData
    {
        public int PageId { get; set; }
        public string? Referrer { get; set; }
    }
}
