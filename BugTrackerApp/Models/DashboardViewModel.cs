namespace BugTrackerApp.Models
{
    public class DashboardViewModel
    {
        public BugStatisticsViewModel BugStatistics { get; set; }
        public IEnumerable<SolvedBug> RecentlySolvedBugs { get; set; }
    }
}
