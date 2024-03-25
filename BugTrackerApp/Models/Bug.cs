using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerApp.Models
{
    public class Bug
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime DateReported { get; set; }
        [Required]
        public ErrorType Type { get; set; }
        [Required]
        public BugLevel Level { get; set; }
        [Required]
        public bool IsHighPriority { get; set; }
        public enum BugLevel
        {
            High,
            Medium,
            Low
        }
    }
}
