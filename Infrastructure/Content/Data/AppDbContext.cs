using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Domain.Models;

namespace Infrastructure.Content.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<Candidate> Candidates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Candidate>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Candidate>()
                .Property(c => c.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");
        }

    }
}