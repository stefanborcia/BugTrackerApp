using BugTrackerApp.Data;
using BugTrackerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BugTrackerApp.Controllers
{
    public class BugController : Controller
    {

        private readonly BugDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public BugController(BugDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var bugsToShow = _context.Bugs.Where(b => b.ShowInBugList);
            return View(bugsToShow.ToList());
        }

        public IActionResult Create()
        {
            PopulateErrorTypesInViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bug bug)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Add(bug);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Handle specific database update errors (e.g., unique constraint violations)
                    ModelState.AddModelError("", "An error occurred while saving the bug. Please check the details.");
                    // Log specific details about the exception
                }
                catch (Exception ex)
                {
                    // Handle other unexpected exceptions
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    // Log specific details about the exception
                }
            }
            // ModelState is not valid, re-populate ErrorTypes and return to the create view with validation errors
            PopulateErrorTypesInViewBag();
            return View(bug);
        }

        private void PopulateErrorTypesInViewBag()
        {
            var errorTypes = Enum.GetValues(typeof(ErrorType))
                .Cast<ErrorType>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();

            errorTypes.Insert(0, new SelectListItem("-- Select Error Type --", ""));
            ViewBag.ErrorTypes = errorTypes;
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = _context.Bugs.Find(id);

            if (bug == null)
            {
                return NotFound();
            }
            PopulateErrorTypesInViewBag();
            return View(bug);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Bug bug)
        {
            if (id != bug.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bug);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BugExists(bug.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "The bug you tried to edit was modified by another user. Please reload the data and try again.");
                        return View(bug);
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateErrorTypesInViewBag();
            return View(bug);
        }
        private bool BugExists(int id)
        {
            return _context.Bugs.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _context.Bugs
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bug == null)
            {
                return NotFound();
            }
            return View(bug);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bug = await _context.Bugs.FindAsync(id);

            if (bug == null)
            {
                return NotFound();
            }
            _context.Bugs.Remove(bug);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Solve(int id)
        {
            var bug = _context.Bugs.Find(id);
            if (bug == null)
            {
                return NotFound();
            }

            PopulateErrorTypesInViewBag();
            return View(new SolvedBug { BugId = id }); // Pass a new instance of SolvedBug with BugId set
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Solve(SolvedBug solvedBug)
        {
            if (ModelState.IsValid)
            {
                // If model state is not valid, return to the same view
                return View(solvedBug);
            }
            var bugToUpdate = _context.Bugs.Find(solvedBug.BugId);
            if (bugToUpdate != null)
            {
                bugToUpdate.ShowInBugList = false;
                _context.SaveChanges();
            }

            // Save SolvedBug if needed
            _context.SolvedBugs.Add(solvedBug);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }
        public IActionResult BugDetails(int id)
        {
            var solvedBug = _context.SolvedBugs
                .Include(sb => sb.Bug) // Include the related Bug data
                .FirstOrDefault(sb => sb.BugId == id);


            if (solvedBug == null)
            {
                return NotFound();
            }

            return View(solvedBug);
        }
    }
}
