﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;

namespace OnlineShop.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OnlineShopContext _context;

        public OrdersController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: User/Orders
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _context.Orders.Where(x=>x.UserId==userId).OrderByDescending(x=>x.Id).ToListAsync();
            return View(result);
        }

        // GET: User/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["OrderDetails"] = await _context.OrderDetails.
                                        Where(x => x.OrderId == id).ToListAsync();

            return View(order);
        }



        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
