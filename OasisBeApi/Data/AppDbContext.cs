using Microsoft.EntityFrameworkCore;
using Oasis.Models;

namespace Oasis.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Level> Levels { get; set; } = null!;
    public DbSet<Mood> Moods { get; set; } = null!;
    public DbSet<ActivityCategory> ActivityCategories { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<MemberMood> MemberMoods { get; set; } = null!;
    public DbSet<Activity> Activities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite key for MemberMood
        modelBuilder.Entity<MemberMood>()
            .HasKey(mm => new { mm.MoodId, mm.MemberId });

        // Team leader FK restriction (prevent cascade delete)
        modelBuilder.Entity<Team>()
            .HasOne(t => t.Leader)
            .WithMany()
            .HasForeignKey(t => t.LeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}