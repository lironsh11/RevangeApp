using Microsoft.AspNetCore.Mvc;
using RevengeApp.Services;
using RevengeApp.Models;

namespace RevengeApp.Controllers
{
    public class RevengeController : Controller
    {
        private readonly IRevengeService _revengeService;

        public RevengeController(IRevengeService revengeService)
        {
            _revengeService = revengeService;
        }

        // GET: רשימת נקמות
        public async Task<IActionResult> Index()
        {
            try
            {
                var revenges = await _revengeService.GetAllRevengesAsync();
                return View(revenges ?? new List<Revenge>());
            }
            catch (Exception ex)
            {
                return View(new List<Revenge>());
            }
        }

        // GET: טופס נקמה חדשה
        public IActionResult Create()
        {
            return View(new Revenge());
        }

        // POST: שמירת נקמה חדשה
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Revenge revenge)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _revengeService.AddRevengeAsync(revenge);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "שגיאה בשמירת הנקמה. נסי שוב!");
                }
            }
            return View(revenge);
        }

        // GET: טופס עריכת נקמה
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var revenge = await _revengeService.GetRevengeByIdAsync(id);
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

        // POST: שמירת עריכה
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Revenge revenge)
        {
            if (id != revenge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _revengeService.UpdateRevengeAsync(revenge);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "שגיאה בעדכון הנקמה. נסי שוב!");
                }
            }
            return View(revenge);
        }

        // GET: מחיקת נקמה
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _revengeService.DeleteRevengeAsync(id);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: סימון נקמה כהושלמה (AJAX)
        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                var success = await _revengeService.CompleteRevengeAsync(id);
                if (success)
                {
                    return Json(new { success = true, message = "הנקמה הושלמה בהצלחה!" });
                }
                return Json(new { success = false, message = "שגיאה בעדכון הנקמה" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "שגיאה בעדכון הנקמה" });
            }
        }

        // GET: פרטי נקמה (אופציונלי)
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var revenge = await _revengeService.GetRevengeByIdAsync(id);
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
    }
}