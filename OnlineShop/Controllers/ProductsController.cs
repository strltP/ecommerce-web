﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;
using OnlineShop.Models.Db;
using System.Text.RegularExpressions;
namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly OnlineShopContext _context;
        public ProductsController(OnlineShopContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, int pageSize = 3, int? minPrice = null, int? maxPrice = null)
        {
            var query = _context.Products.AsQueryable();

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            query = query.OrderByDescending(p => p.Id);

            var products = PaginationHelper.Paginate(query, page, pageSize, out int totalPages);
            var totalProducts = query.Count();

            SetCommonViewData(page, totalPages, totalProducts, minPrice, maxPrice);

            return View(products);
        }



        //cate
        public IActionResult ByCategory(int categoryId, int page = 1, int pageSize = 3, int? minPrice = null, int? maxPrice = null)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                ViewData["Message"] = "Danh mục không tồn tại.";
                return View("NotFound");
            }

            var query = _context.Products.Where(p => p.CategoryId == categoryId);

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            var products = PaginationHelper.Paginate(query.OrderByDescending(p => p.Id), page, pageSize, out int totalPages);
            var totalProducts = query.Count();

            ViewData["CategoryName"] = category.Name;
            ViewData["CategoryId"] = categoryId;

            SetCommonViewData(page, totalPages, totalProducts, minPrice, maxPrice);

            return View("Index", products);
        }




        public IActionResult SearchProducts(string SearchText, int page = 1, int pageSize = 3, int? minPrice = null, int? maxPrice = null)
        {
            var query = _context.Products
                .Where(x => EF.Functions.Like(x.Title, "%" + SearchText + "%") ||
                            EF.Functions.Like(x.Tags, "%" + SearchText + "%"));

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            var products = PaginationHelper.Paginate(query.OrderBy(x => x.Title), page, pageSize, out int totalPages);
            var totalProducts = query.Count();

            ViewData["SearchText"] = SearchText;

            SetCommonViewData(page, totalPages, totalProducts, minPrice, maxPrice);

            return View("Index", products);
        }


        public IActionResult ProductDetails(int id)
        {
            Product? product = _context.Products.FirstOrDefault(x=>x.Id==id);

            //=------------------------------------
            if(product == null)
            {
                return NotFound();
            }
            //-------------------------------------
            ViewData["gallery"] = _context.ProductGalleries
                .Where(x => x.ProductId == id).ToList();

            //-----------------------------------------

            ViewData["NewProducts"] = _context.Products.Where(x => x.Id != id).Take(6).OrderByDescending(x=>x.Id).ToList();
            //---------------------------------------------
            ViewData["comments"] = _context.Comments
                .Where(x => x.ProductId == id)
                .OrderByDescending(x => x.CreateDate).ToList();

            return View(product);

            

        }

        [HttpPost]
        public IActionResult SubmitComment(string name, string email, string comment, int productId)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(comment) && productId != 0)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success)
                {
                    TempData["ErrorMessage"] = "Email is not valid";
                    return Redirect("/Products/ProductDetails/" + productId);
                }

                Comment newComment = new Comment();
                newComment.Name = name;
                newComment.Email = email;
                newComment.CommentText = comment;
                newComment.ProductId = productId;
                newComment.CreateDate = DateTime.Now;

                _context.Comments.Add(newComment);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Youre comment submited success fully";
                return Redirect("/Products/ProductDetails/" + productId);
            }
            else
            {
                TempData["ErrorMessage"] = "Please complete youre information";
                return Redirect("/Products/ProductDetails/" + productId);
            }

        }

        private void SetCommonViewData(int page, int totalPages, int totalProducts, int? minPrice, int? maxPrice)
        {
            ViewData["Categories"] = _context.Categories.OrderBy(c => c.Id).ToList();
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["TotalProducts"] = totalProducts;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;
            ViewData["SliderMaxPrice"] = _context.Products.Max(p => p.Price);
        }




    }
}
