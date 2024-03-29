using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerApp.Models
{
    public class SolvedBug
    {
        public SolvedBug()
        {
            DateResolved = DateTime.Today;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BugId { get; set; }
        [Required(ErrorMessage = "Please provide steps to solve.")]
        public string StepsToSolve { get; set; }

        [Required(ErrorMessage = "Please provide the time spent.")]
        public TimeSpan TimeSpent { get; set; }
        [Required(ErrorMessage = "Please provide the date resolved.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Resolved")]
        [DateInPastAndNotFuture(ErrorMessage = "Date resolved must be today's date.")]
        public DateTime DateResolved { get; set; }
        [Required(ErrorMessage = "Please specify if the bug is resolved.")]
        public bool IsResolved { get; set; }
        public bool ShowOnDashboard { get; set; } = true; // New property
        public Bug Bug { get; set; }

        public class DateInPastAndNotFutureAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var dateResolved = (DateTime)value;
                var currentDate = DateTime.Today;

                if (dateResolved < DateTime.MinValue || dateResolved > DateTime.MaxValue)
                {
                    return new ValidationResult("Date resolved is out of valid range.");
                }

                if (dateResolved.Date != currentDate)
                {
                    return new ValidationResult("Date resolved must be today's date.");
                }

                return ValidationResult.Success;
            }
        }
    }
}
