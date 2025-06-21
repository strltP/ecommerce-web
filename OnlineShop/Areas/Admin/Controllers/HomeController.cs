using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Models.Db;
using OnlineShop.Services;
using System.Linq;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly OnlineShopContext _context;

        public HomeController(OnlineShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Report()
        {
            int year = DateTime.Now.Year;

            var userStats = _context.Users
                .Where(u => u.RegisterDate.HasValue && u.RegisterDate.Value.Year == year)
                .GroupBy(u => u.RegisterDate.Value.Month)
                .Select(g => new MonthlyStat { Month = g.Key, Count = g.Count() })
                .ToList();

            var orderStats = _context.Orders
                .Where(o => o.CreateDate.HasValue && o.CreateDate.Value.Year == year)
                .GroupBy(o => o.CreateDate.Value.Month)
                .Select(g => new OrderMonthlyStat
                {
                    Month = g.Key,
                    OrderCount = g.Count(),
                    TotalRevenue = g.Sum(x => x.Total ?? 0)
                })
                .ToList();

            var topProducts = _context.BestSellingFinals
                .OrderByDescending(p => p.TotalSum ?? 0)
                .Take(5) // hoặc 10 nếu bạn muốn top 10
                .ToList();

            var vm = new ReportViewModel
            {
                UserStats = userStats,
                OrderStats = orderStats,
                TopSellingProducts = topProducts
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult ExportPdf()
        {
            int year = DateTime.Now.Year;

            // Lấy dữ liệu như trong Report()
            var userStats = _context.Users
                .Where(u => u.RegisterDate.HasValue && u.RegisterDate.Value.Year == year)
                .GroupBy(u => u.RegisterDate.Value.Month)
                .Select(g => new MonthlyStat { Month = g.Key, Count = g.Count() })
                .ToList();

            var orderStats = _context.Orders
                .Where(o => o.CreateDate.HasValue && o.CreateDate.Value.Year == year)
                .GroupBy(o => o.CreateDate.Value.Month)
                .Select(g => new OrderMonthlyStat
                {
                    Month = g.Key,
                    OrderCount = g.Count(),
                    TotalRevenue = g.Sum(x => x.Total ?? 0)
                })
                .ToList();

            var topProducts = _context.BestSellingFinals
                .OrderByDescending(p => p.TotalSum ?? 0)
                .Take(5)
                .ToList();

            var report = new ReportViewModel
            {
                UserStats = userStats,
                OrderStats = orderStats,
                TopSellingProducts = topProducts
            };

            var pdfBytes = PdfReportService.GenerateReport(report);
            return File(pdfBytes, "application/pdf", "bao-cao-thong-ke.pdf");
        }




    }
}
