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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
