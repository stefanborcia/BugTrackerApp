namespace BugTrackerApp.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateReported { get; set; }
        public bool IsResolved { get; set; }
    }
}
