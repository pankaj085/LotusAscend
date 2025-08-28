using LotusAscend.Models;
using Microsoft.EntityFrameworkCore;

namespace LotusAscend.AppDataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Member> Members { get; set; }
    public DbSet<OTP> Otps { get; set; }
    public DbSet<PointTransaction> PointTransactions { get; set; }
    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enforce username uniqueness at the database level
        modelBuilder.Entity<Member>()
            .HasIndex(m => m.Username)
            .IsUnique();

        modelBuilder.Entity<Member>()
            .HasOne(m => m.Otp)
            .WithOne(o => o.Member)
            .HasForeignKey<OTP>(o => o.MemberId);
    }
}