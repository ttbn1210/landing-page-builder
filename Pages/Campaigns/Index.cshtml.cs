using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Infrastructure.Repositories;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.Campaigns;

public class IndexModel : PageModel
{
    private readonly CampaignRepository _repo;

    public IndexModel(CampaignRepository repo)
    {
        _repo = repo;
    }

    public List<Campaign> Campaigns { get; set; } = new();

    public async Task OnGetAsync()
    {
        Campaigns = await _repo.GetAllAsync();
    }
}
