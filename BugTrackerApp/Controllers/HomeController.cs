using BugTrackerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BugTrackerApp.Data;
using System.Globalization;
using static BugTrackerApp.Models.Bug;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace BugTrackerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BugDbContext _context;
        public HomeController(ILogger<HomeController> logger, BugDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AcceptCookies()
        {
            // Set the cookie named "acceptedCookies" with a value of "true" and a 1-year expiration
            Response.Cookies.Append("acceptedCookies", "true", new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(365),
                HttpOnly = true,
                Secure = true // Use this with HTTPS for added security (optional)
            });

            return Content("Cookie accepted", "text/plain");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Dashboard()
        {
            var bugs = _context.Bugs
                .ToList();

            //Retrieve bug data grouped by month and severity levels
            var bugData = bugs
                .GroupBy(b => new { Month = b.DateReported.Month, Level = b.Level })
                .Select(g => new { Month = g.Key.Month, Level = g.Key.Level, Count = g.Count() })
                .ToList();

            // Extract data for monthly bug counts based on severity levels
            var monthLabels = Enumerable.Range(1, 12).Select(m => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)).ToArray();
            var highBugsMonthlyCount = new int[12];
            var mediumBugsMonthlyCount = new int[12];
            var lowBugsMonthlyCount = new int[12];

            foreach (var bug in bugData)
            {
                int monthIndex = bug.Month - 1; // Adjust month index to zero-based
                switch (bug.Level)
                {
                    case BugLevel.High:
                        highBugsMonthlyCount[monthIndex] += bug.Count;
                        break;
                    case BugLevel.Medium:
                        mediumBugsMonthlyCount[monthIndex] += bug.Count;
                        break;
                    case BugLevel.Low:
                        lowBugsMonthlyCount[monthIndex] += bug.Count;
                        break;
                }
            }
            var bugStatisticsViewModel = new BugStatisticsViewModel
            {
                HighBugsCount = bugs.Count(b => b.Level == Bug.BugLevel.High),
                MediumBugsCount = bugs.Count(b => b.Level == Bug.BugLevel.Medium),
                LowBugsCount = bugs.Count(b => b.Level == Bug.BugLevel.Low),
                MonthLabels = monthLabels,
                HighBugsMonthlyCount = highBugsMonthlyCount,
                MediumBugsMonthlyCount = mediumBugsMonthlyCount,
                LowBugsMonthlyCount = lowBugsMonthlyCount
            };
            return View(bugStatisticsViewModel);
        }       
    }
}
