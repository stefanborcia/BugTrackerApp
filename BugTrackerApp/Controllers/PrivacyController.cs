using Microsoft.AspNetCore.Mvc;

namespace BugTrackerApp.Controllers
{
    public class PrivacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Accept(string choice)
        {
            if (choice == "accept")
            {
                // Set cookie indicating acceptance
                SetCookie("AcceptedPrivacyPolicy", "true");
            }
            else if (choice == "decline")
            {
                // Set cookie indicating decline
                SetCookie("AcceptedPrivacyPolicy", "false");
            }
            else if (choice == "save")
            {
                // Set cookie indicating saved preferences
                SetCookie("PrivacyPolicyPreferencesSaved", "true");
            }

            return RedirectToAction("Index");
        }

        private void SetCookie(string name, string value)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30) // Expires in 30 days
            };
            Response.Cookies.Append(name, value, cookieOptions);
        }
    }

}
