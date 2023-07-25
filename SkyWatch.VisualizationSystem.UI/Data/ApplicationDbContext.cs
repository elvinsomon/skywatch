using Microsoft.EntityFrameworkCore;

namespace SkyWatch.VisualizationSystem.UI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    
    public DbSet<Alert> Alerts { get; set; }
}