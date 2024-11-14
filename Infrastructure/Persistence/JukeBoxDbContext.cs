using Domain.Entities.JukeBox;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;
public class JukeBoxDbContext : DbContext
{
    public JukeBoxDbContext(DbContextOptions<JukeBoxDbContext> options) : base(options)
    {
    }
    public DbSet<VolumeDto> VolumeDtos { get; set; }

    public DbSet<WishListDto> WishListDtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VolumeDto>().ToTable("jukeBoxVolume");
        modelBuilder.Entity<WishListDto>().ToTable("jukeBoxWishList");
    }
}