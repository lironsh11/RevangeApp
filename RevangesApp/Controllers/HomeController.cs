using Microsoft.AspNetCore.Mvc;
using RevengeApp.Models;
using RevengeApp.Services;

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
                // Get real data from database
                var revenges = await _revengeService.GetAllRevengesAsync();
                return View(revenges);
            }
            catch (Exception ex)
            {
                // Return empty list if error
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