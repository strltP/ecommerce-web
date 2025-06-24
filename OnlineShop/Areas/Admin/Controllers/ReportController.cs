using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;
using OnlineShop.Areas.Admin.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ReportController : Controller
    {
        private readonly OnlineShopContext _context;

        public ReportController(OnlineShopContext context)
        {
            _context = context;
        }

        public IActionResult UserReport(int? year)
        {
            var model = new UserReportViewModel();

            var users = _context.Users
                .Where(u => u.RegisterDate.HasValue)
                .ToList();

            model.TotalUsers = users.Count;
            model.TotalAdmins = users.Count(u => u.IsAdmin);

            // Available years
            model.AvailableYears = users
                .Select(u => u.RegisterDate!.Value.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            model.SelectedYear = year ?? DateTime.Now.Year;

            // Users per day (7 ngày gần nhất)
            var sevenDays = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-i))
                .OrderBy(d => d)
                .ToList();

            foreach (var date in sevenDays)
            {
                var count = users.Count(u =>
                    u.RegisterDate!.Value.Date == date.Date);

                model.UsersPerDay[date.ToString("dd/MM")] = count;
            }

            // Users per month 
            for (int month = 1; month <= 12; month++)
            {
                var count = users.Count(u =>
                    u.RegisterDate!.Value.Year == model.SelectedYear &&
                    u.RegisterDate!.Value.Month == month);

                model.UsersPerMonth[$"Th {month}"] = count;
            }

            // Users per year
            model.UsersPerYear = users
                .GroupBy(u => u.RegisterDate!.Value.Year)
                .ToDictionary(g => g.Key, g => g.Count());

            return View(model);
        }

        public IActionResult OrderReport(int? year)
        {
            var model = new OrderReportViewModel();

            var orders = _context.Orders
                .Where(o => o.CreateDate.HasValue)
                .ToList();

            model.TotalOrders = orders.Count;
            model.TotalRevenue = orders.Sum(o => o.Total ?? 0);

            model.AvailableYears = orders
                .Select(o => o.CreateDate!.Value.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            model.SelectedYear = year ?? DateTime.Now.Year;

            // Đơn theo 7 ngày gần nhất
            var days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-i))
                .OrderBy(d => d)
                .ToList();

            foreach (var date in days)
            {
                var count = orders.Count(o =>
                    o.CreateDate!.Value.Date == date.Date);

                model.OrdersPerDay[date.ToString("dd/MM")] = count;
            }

            // Đơn + doanh thu theo tháng
            for (int month = 1; month <= 12; month++)
            {
                var monthlyOrders = orders
                    .Where(o => o.CreateDate!.Value.Year == model.SelectedYear &&
                                o.CreateDate!.Value.Month == month);

                model.OrdersPerMonth[$"Th {month}"] = monthlyOrders.Count();
                model.RevenuePerMonth[$"Th {month}"] = monthlyOrders.Sum(o => o.Total ?? 0);
            }

            // Đơn theo trạng thái
            model.OrdersByStatus = orders
                .GroupBy(o => o.Status ?? "Không rõ")
                .ToDictionary(g => g.Key, g => g.Count());


            //
            model.RevenuePerDay = days.ToDictionary(
                d => d.ToString("dd/MM"),
                d => orders.Where(o => o.CreateDate!.Value.Date == d.Date).Sum(o => o.Total ?? 0));
            model.OrdersPerYear = orders
                .GroupBy(o => o.CreateDate!.Value.Year)
                .ToDictionary(g => g.Key, g => g.Count());

            model.RevenuePerYear = orders
                .GroupBy(o => o.CreateDate!.Value.Year)
                .ToDictionary(g => g.Key, g => g.Sum(o => o.Total ?? 0));


            return View(model);
        }

    }
}
