using Domain.Entities.JukeBox;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class GastroOnContext : DbContext
{
    public GastroOnContext(DbContextOptions<GastroOnContext> options) : base(options)
    {
    }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GastroOnContext).Assembly);
    }
}