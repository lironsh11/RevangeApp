using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RevengeApp.Services;
using RevengeApp.Models;

namespace RevengeApp.Controllers
{
    [Authorize] // Require authentication for all actions
    public class RevengeController : Controller
    {
        private readonly IRevengeService _revengeService;

        public RevengeController(IRevengeService revengeService)
        {
            _revengeService = revengeService;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        // GET: Display user's revenges only
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = GetCurrentUserId();
                var revenges = await _revengeService.GetAllRevengesForUserAsync(userId);
                return View(revenges);
            }
            catch (Exception ex)
            {
                return View(new List<Revenge>());
            }
        }

        // GET: Show create revenge form
        public IActionResult Create()
        {
            return View(new Revenge());
        }

        // POST: Save new revenge for current user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Revenge revenge)
        {
            // Remove UserId from ModelState validation since we set it manually
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = GetCurrentUserId();
                    Console.WriteLine($"Creating revenge for user: {userId}"); // Debug line
                    await _revengeService.AddRevengeForUserAsync(revenge, userId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating revenge: {ex.Message}"); // Debug line
                    ModelState.AddModelError("", "Error saving revenge. Please try again!");
                }
            }
            else
            {
                // Debug: show validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
            }

            return View(revenge);
        }

        // GET: Show edit form for user's revenge only
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var revenge = await _revengeService.GetRevengeByIdForUserAsync(id, userId);
                if (revenge == null)
                {
                    return NotFound();
                }
                return View(revenge);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // POST: Update user's revenge only
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Revenge revenge)
        {
            if (id != revenge.Id)
            {
                return NotFound();
            }

            // Remove UserId from ModelState validation
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = GetCurrentUserId();
                    await _revengeService.UpdateRevengeForUserAsync(revenge, userId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating revenge: {ex.Message}"); // Debug line
                    ModelState.AddModelError("", "Error updating revenge. Please try again!");
                }
            }

            return View(revenge);
        }

        // GET: Delete user's revenge only
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _revengeService.DeleteRevengeForUserAsync(id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Complete user's revenge only (AJAX endpoint)
        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _revengeService.CompleteRevengeForUserAsync(id, userId);
                if (success)
                {
                    return Json(new { success = true, message = "Revenge completed successfully!" });
                }
                return Json(new { success = false, message = "Error updating revenge" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error updating revenge" });
            }
        }
    }
}