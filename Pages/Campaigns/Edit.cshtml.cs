using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Features.Campaigns;
using TrackQR.Web.Infrastructure.Repositories;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.Campaigns;

public class EditModel : PageModel
{
    private readonly CampaignRepository _repo;
    private readonly UpdateCampaignHandler _handler;

    public EditModel(CampaignRepository repo, UpdateCampaignHandler handler)
    {
        _repo = repo;
        _handler = handler;
    }

    [BindProperty]
    public Guid Id { get; set; }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string? Description { get; set; }

    [BindProperty]
    public CampaignStatus Status { get; set; }

    [BindProperty]
    public DateTime? StartDate { get; set; }

    [BindProperty]
    public DateTime? EndDate { get; set; }

    [BindProperty]
    public decimal? Budget { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var campaign = await _repo.GetByIdAsync(id);
        if (campaign == null) return NotFound();

        Id = campaign.Id;
        Name = campaign.Name;
        Description = campaign.Description;
        Status = campaign.Status;
        StartDate = campaign.StartDate;
        EndDate = campaign.EndDate;
        Budget = campaign.Budget;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        await _handler.HandleAsync(Id, Name, Description, Status, StartDate, EndDate, Budget);
        return RedirectToPage("Index");
    }
}
