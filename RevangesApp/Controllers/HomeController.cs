using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RevengeApp.Services;
using RevengeApp.Models;

namespace RevengeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRevengeService _revengeService;

        public HomeController(IRevengeService revengeService)
        {
            _revengeService = revengeService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Check if user is authenticated
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var revenges = await _revengeService.GetAllRevengesForUserAsync(userId);
                        return View(revenges);
                    }
                }

                // Return empty list for non-authenticated users
                return View(new List<Revenge>());
            }
            catch (Exception ex)
            {
                return View(new List<Revenge>());
            }
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}