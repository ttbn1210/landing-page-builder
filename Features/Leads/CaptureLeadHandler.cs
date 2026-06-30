using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;
using TrackQR.Web.Services.Leads;

namespace TrackQR.Web.Features.Leads;

public class CaptureLeadHandler
{
    private readonly AppDbContext _db;
    private readonly LeadScoreService _scoreService;

    public CaptureLeadHandler(AppDbContext db, LeadScoreService scoreService)
    {
        _db = db;
        _scoreService = scoreService;
    }

    public async Task<Lead?> HandleAsync(string sessionKey, string? fullName, string? phone, string? email, string? message)
    {
        var session = await _db.VisitorSessions
            .FirstOrDefaultAsync(s => s.SessionKey == sessionKey);

        if (session == null) return null;

        // Check if lead already exists for this session
        var existingLead = await _db.Leads
            .FirstOrDefaultAsync(l => l.SessionId == session.Id);

        if (existingLead != null)
        {
            existingLead.FullName = fullName ?? existingLead.FullName;
            existingLead.Phone = phone ?? existingLead.Phone;
            existingLead.Email = email ?? existingLead.Email;
            existingLead.Message = message ?? existingLead.Message;
            await _db.SaveChangesAsync();
            return existingLead;
        }

        var score = await _scoreService.CalculateScoreAsync(session.Id);

        var lead = new Lead
        {
            SessionId = session.Id,
            FullName = fullName,
            Phone = phone,
            Email = email,
            Message = message,
            Status = LeadStatus.New,
            Score = score
        };

        _db.Leads.Add(lead);

        // Mark session as converted
        session.IsConverted = true;

        await _db.SaveChangesAsync();

        return lead;
    }
}
