namespace BugTrackerApp.Models
{
    public class BugStatisticsViewModel
    {
        public int SolvedBugsCount { get; set; }
        public int UnsolvedBugsCount { get; set; }
        public int HighBugsCount { get; set; }
        public int MediumBugsCount { get; set; }
        public int LowBugsCount { get; set; }
        public List<int> MonthlyBugCounts { get; set; }
        public string[] MonthLabels { get; set; }
        public int[] HighBugsMonthlyCount { get; set; }
        public int[] MediumBugsMonthlyCount { get; set; }
        public int[] LowBugsMonthlyCount { get; set; }
    }
}
