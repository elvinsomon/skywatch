using Microsoft.EntityFrameworkCore;

namespace SkyWatch.VisualizationSystem.UI.Data;

public class AlertService
{
    private readonly ApplicationDbContext _appDbContext;

    public AlertService(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Alert>> GetAllAlertsAsync()
        => await _appDbContext.Alerts.ToListAsync();
    
    public async Task<bool> InsertAlertAsync(Alert alert)
    {
        await _appDbContext.Alerts.AddAsync(alert);
        await _appDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Alert> GetAlertAsync(int id)
        => await _appDbContext.Alerts.FirstOrDefaultAsync(c => c.Id.Equals(id)) ?? 
           throw new ArgumentNullException($"Alert with Id {id} not found");
    
    public async Task<bool> UpdateAlertAsync(Alert alert)
    {
        _appDbContext.Alerts.Update(alert);
        await _appDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAlertAsync(Alert alert)
    {
        _appDbContext.Remove(alert);
        await _appDbContext.SaveChangesAsync();
        return true;
    }
}