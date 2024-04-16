using Microsoft.AspNetCore.Identity;

namespace BugTrackerApp.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
    }
}
