using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.Json;

namespace LandingPageBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller}")]
    public class FormSubmissionApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormSubmissionApiController> _logger;

        public FormSubmissionApiController(ApplicationDbContext context, ILogger<FormSubmissionApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SubmitForm([FromBody] FormSubmitData data)
        {
            try
            {
                var page = await _context.LandingPages.FindAsync(data.PageId);
                if (page == null)
                    return NotFound();

                var submission = new FormSubmission
                {
                    LandingPageId = data.PageId,
                    FormName = data.FormName,
                    SubmittedData = JsonSerializer.Serialize(data.FormData),
                    SubmitterEmail = data.Email,
                    SubmittedAt = DateTime.UtcNow
                };

                _context.FormSubmissions.Add(submission);
                await _context.SaveChangesAsync();

                // Send email notification
                _ = SendEmailNotificationAsync(page, data);

                return Ok(new { success = true, message = "Form submitted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting form");
                return StatusCode(500, "Error submitting form");
            }
        }

        [HttpGet("submissions/{pageId:int}")]
        [Authorize]
        public async Task<ActionResult> GetSubmissions(int pageId)
        {
            var page = await _context.LandingPages.FindAsync(pageId);
            if (page == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page.UserId != userId)
                return Forbid();

            var submissions = await _context.FormSubmissions
                .Where(s => s.LandingPageId == pageId)
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();

            return Ok(submissions);
        }

        private async Task SendEmailNotificationAsync(LandingPage page, FormSubmitData data)
        {
            try
            {
                // TODO: Implement actual email sending using SendGrid or similar service
                _logger.LogInformation($"Form submission received from {data.Email} for page {page.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email notification");
            }
        }
    }

    public class FormSubmitData
    {
        public int PageId { get; set; }
        public string FormName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Dictionary<string, string> FormData { get; set; } = new();
    }
}
