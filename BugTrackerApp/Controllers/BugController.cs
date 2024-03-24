using BugTrackerApp.Data;
using BugTrackerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            return View(_context.Bugs.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.ErrorTypes = Enum.GetValues(typeof(ErrorType)).Cast<ErrorType>().Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.ToString()
            }).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bug bug)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(bug);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    ModelState.AddModelError("", "An error occurred while saving the bug.");
                    
                }
            }
            ViewBag.ErrorTypes = Enum.GetValues(typeof(ErrorType));
            return View(bug);
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

            ViewBag.ErrorTypes = Enum.GetValues(typeof(ErrorType)).Cast<ErrorType>().Select(e =>
                new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();

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

            ViewBag.ErrorTypes = Enum.GetValues(typeof(ErrorType)).Cast<ErrorType>().Select(e =>
                new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();

            return View(bug);
        }
        private bool BugExists(int id)
        {
            return _context.Bugs.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Details(int? id)
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
            ViewBag.ErrorTypes = Enum.GetValues(typeof(ErrorType));
            return View(bug);
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
    }
}
