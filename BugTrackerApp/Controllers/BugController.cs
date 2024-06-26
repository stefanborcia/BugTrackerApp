﻿using BugTrackerApp.Data;
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
            if (ModelState.IsValid)
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

        public IActionResult EditSolvedBug(int? id, int? existingSolved)
        {
            if (id == null || existingSolved == null)
            {
                return NotFound();
            }

            // Note: This finds the corresponding SolvedBug, adjust as needed for your setup
            var solvedBug = _context.SolvedBugs.FirstOrDefault(sb => sb.BugId == id && sb.BugId == existingSolved);


            if (solvedBug == null)
            {
                return NotFound();
            }

            // ... (code to populate ViewBag if needed)

            return View(solvedBug);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSolvedBug(int id, [Bind("SolvedBugId,BugId,StepsToSolve,TimeSpent,IsResolved,BugStatus,DateResolved")]
            SolvedBug solvedBug)
        {
            if (id != solvedBug.BugId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(solvedBug);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // ... (handle concurrency conflicts)
                }
                return RedirectToAction("Dashboard", "Home");
            }

            return View(solvedBug);
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
            var viewModel = new SolvedBug { BugId = id };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Solve(SolvedBug solvedBug)
        {
            if (ModelState.IsValid)
            {
                return View(solvedBug);
            }

            try
            {
                UpdateOrCreateSolvedBug(solvedBug);
                UpdateBugStatus(solvedBug.BugId);
                return RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                // Log the exception
                return View(solvedBug);
            }
        }
        private void UpdateOrCreateSolvedBug(SolvedBug solvedBug)
        {
            var existingSolvedBug = _context.SolvedBugs.FirstOrDefault(sb => sb.BugId == solvedBug.BugId);

            if (existingSolvedBug != null)
            {
                // Update existing SolvedBug
                existingSolvedBug.BugId=solvedBug.BugId;
                existingSolvedBug.StepsToSolve = solvedBug.StepsToSolve;
                existingSolvedBug.TimeSpent = solvedBug.TimeSpent;
                existingSolvedBug.IsResolved = solvedBug.IsResolved;
                existingSolvedBug.BugStatus = solvedBug.BugStatus;
                existingSolvedBug.DateResolved = solvedBug.DateResolved;
            }
            else
            {
                // Create a new SolvedBug
                _context.SolvedBugs.Add(solvedBug);
            }

            _context.SaveChanges();
        }
        private void UpdateBugStatus(int bugId)
        {
            var bugToUpdate = _context.Bugs.Find(bugId);
            if (bugToUpdate != null)
            {
                bugToUpdate.ShowInBugList = false;
                _context.SaveChanges();
            }
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
