using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace BugTrackerApp.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DateReported { get; set; }
        public ErrorType Type { get; set; }
        public BugLevel Level { get; set; }
        public SolvedBug? SolvedBug { get; set; }
        public bool ShowInBugList { get; set; } = true;
        public bool IsHighPriority { get; set; }
        // Navigation property to SolvedBug
        public enum BugLevel
        {
            High,
            Medium,
            Low
        }
    }
}
