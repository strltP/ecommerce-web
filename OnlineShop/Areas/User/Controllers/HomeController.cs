using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Areas.User.Models;
using OnlineShop.Models.Db;
using System.Security.Claims;

namespace OnlineShop.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly OnlineShopContext _context;
        public HomeController(OnlineShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user= _context.Users.FirstOrDefault(x => x.Id == userId);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var viewModel = new EditProfileViewModel
            {
                FullName = user.FullName
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userInDb = await _context.Users.FindAsync(userId);
            if (userInDb == null) return NotFound();

            userInDb.FullName = model.FullName;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }





    }
}
