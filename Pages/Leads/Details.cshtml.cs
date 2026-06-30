using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Features.Leads;
using TrackQR.Web.Infrastructure.Repositories;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.Leads;

public class DetailsModel : PageModel
{
    private readonly LeadRepository _repo;
    private readonly UpdateLeadStatusHandler _statusHandler;
    private readonly AddLeadActivityHandler _activityHandler;

    public DetailsModel(LeadRepository repo, UpdateLeadStatusHandler statusHandler, AddLeadActivityHandler activityHandler)
    {
        _repo = repo;
        _statusHandler = statusHandler;
        _activityHandler = activityHandler;
    }

    public Lead? Lead { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Lead = await _repo.GetByIdAsync(id);
        if (Lead == null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(Guid leadId, LeadStatus newStatus)
    {
        await _statusHandler.HandleAsync(leadId, newStatus);
        return RedirectToPage("Details", new { id = leadId });
    }

    public async Task<IActionResult> OnPostAddActivityAsync(Guid leadId, string action)
    {
        await _activityHandler.HandleAsync(leadId, action, null);
        return RedirectToPage("Details", new { id = leadId });
    }
}
