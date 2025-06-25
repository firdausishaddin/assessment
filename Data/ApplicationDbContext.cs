using Microsoft.EntityFrameworkCore;
using assessment.Models;

namespace assessment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PlatformDto> Platforms { get; set; } = null!;
        public DbSet<WellDto> Wells { get; set; } = null!;
        public DbSet<UserDto> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlatformDto>()
                .HasMany(p => p.Wells)
                .WithOne(w => w.Platform)
                .HasPrincipalKey(p => p.PlatformId)
                .HasForeignKey(w => w.PlatformId);
        }
    }
}