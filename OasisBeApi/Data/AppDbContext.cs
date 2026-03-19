using Microsoft.EntityFrameworkCore;
using Oasis.Models;
using Oasis.Data.Seed;

namespace Oasis.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Level> Levels { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<TeamInvitation> TeamInvitations { get; set; } = null!;
    public DbSet<Mood> Moods { get; set; } = null!;
    public DbSet<MemberMood> MemberMoods { get; set; } = null!;
    public DbSet<ActivityCategory> ActivityCategories { get; set; } = null!;
    public DbSet<Activity> Activities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite key for MemberMood
        modelBuilder.Entity<MemberMood>()
            .HasKey(mm => new { mm.MoodId, mm.MemberId });

        // Team leader FK restriction
        modelBuilder.Entity<Team>()
            .HasOne(t => t.Leader)
            .WithMany()
            .HasForeignKey(t => t.LeaderId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-one between Member and User
        modelBuilder.Entity<Member>()
            .HasOne(m => m.User)
            .WithOne(u => u.Member)
            .HasForeignKey<User>(u => u.MemberId);

        // TeamInvitation relationships
        modelBuilder.Entity<TeamInvitation>()
            .HasOne(ti => ti.Team)
            .WithMany(t => t.TeamInvitations)
            .HasForeignKey(ti => ti.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TeamInvitation>()
            .HasOne(ti => ti.InvitedBy)
            .WithMany()
            .HasForeignKey(ti => ti.InvitedById)
            .OnDelete(DeleteBehavior.Restrict);

        // unique team name
        modelBuilder.Entity<Team>()
            .HasIndex(t => t.Name)
            .IsUnique();

        // SEED DATA
        modelBuilder.Entity<Level>().HasData(LevelSeed.GetLevels());

        modelBuilder.Entity<ActivityCategory>().HasData(ActivitySeed.GetCategories());

        modelBuilder.Entity<Activity>().HasData(ActivitySeed.GetActivities());

        base.OnModelCreating(modelBuilder);
    }
}