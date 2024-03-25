using BugTrackerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerApp.Data
{
    public class BugDbContext : IdentityDbContext
    {
        public BugDbContext(DbContextOptions<BugDbContext> options)
            : base(options)
        {
        }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<SolvedBug> SolvedBugs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolvedBug>().HasNoKey(); // Define SolvedBug as a keyless entity type
            base.OnModelCreating(modelBuilder);
        }
    }
}
