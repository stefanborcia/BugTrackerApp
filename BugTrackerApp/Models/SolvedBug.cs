using System.ComponentModel.DataAnnotations;

namespace BugTrackerApp.Models
{
    public class SolvedBug
    {
        [Required]
        public string? StepsToSolve { get; set; }
        [Required]
        public TimeSpan TimeSpent { get; set; }
        [Required]
        public DateTime DateResolved { get; set; }
        [Required]
        public bool IsResolved { get; set; }
        public int BugId { get; set; }
        public Bug? Bug { get; set; }
    }
}
