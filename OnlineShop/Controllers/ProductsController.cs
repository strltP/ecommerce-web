using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(int page = 1, int pageSize = 3)
        {
            ////Lấy tất cả danh mục để hiển thị trong View
            //ViewData["Categories"] = _context.Categories.OrderBy(c => c.Id).ToList();
            //List<Product> products = _context.Products.OrderByDescending(x=>x.Id).ToList();
            //return View(products);

            //var totalProducts = _context.Products.Count();
            //var products = _context.Products
            //    .OrderByDescending(p => p.Id)
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();

            //ViewData["Categories"] = _context.Categories.OrderBy(c => c.Id).ToList();
            //ViewData["CurrentPage"] = page;
            //ViewData["TotalPages"] = (int)Math.Ceiling((double)totalProducts / pageSize);
            //return View(products);


            var query = _context.Products.OrderByDescending(p => p.Id);
            var products = PaginationHelper.Paginate(query, page, pageSize, out int totalPages);
            var totalProducts = query.Count();


            ViewData["Categories"] = _context.Categories.OrderBy(c => c.Id).ToList();
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["TotalProducts"] = totalProducts;


            return View(products);

        }

        //cate
        public IActionResult ByCategory(int categoryId, int page = 1, int pageSize = 3)
        {
            //var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);



            //if (category == null)
            //{
            //    ViewData["Message"] = "Danh mục không tồn tại.";
            //    return View("NotFound");
            //}

            //var products = _context.Products
            //    .Where(p => p.CategoryId == categoryId)
            //    .OrderByDescending(p => p.Id)
            //    .ToList();

            //ViewData["CategoryName"] = category.Name;
            //ViewData["Categories"] = _context.Categories.ToList();

            //return View("Index", products); // sử dụng lại view Index

            //var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            //if (category == null)
            //{
            //    ViewData["Message"] = "Danh mục không tồn tại.";
            //    return View("NotFound");
            //}

            //var totalProducts = _context.Products.Count(p => p.CategoryId == categoryId);
            //var products = _context.Products
            //    .Where(p => p.CategoryId == categoryId)
            //    .OrderByDescending(p => p.Id)
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();

            //ViewData["CategoryName"] = category.Name;
            //ViewData["Categories"] = _context.Categories.ToList();
            //ViewData["CurrentPage"] = page;
            //ViewData["TotalPages"] = (int)Math.Ceiling((double)totalProducts / pageSize);
            //ViewData["CategoryId"] = categoryId;

            //return View("Index", products);

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                ViewData["Message"] = "Danh mục không tồn tại.";
                return View("NotFound");
            }

            var query = _context.Products
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.Id);

            var products = PaginationHelper.Paginate(query, page, pageSize, out int totalPages);
            var totalProducts = query.Count();

            ViewData["CategoryName"] = category.Name;
            ViewData["Categories"] = _context.Categories.ToList();
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["CategoryId"] = categoryId;
            ViewData["TotalProducts"] = totalProducts;

            return View("Index", products);


        }



        public IActionResult SearchProducts(string SearchText, int page = 1, int pageSize = 3)
        {
            //var products = _context.Products
            //    .Where(x => EF.Functions.Like(x.Title, "%" + SearchText + "%") ||
            //    EF.Functions.Like(x.Tags, "%" + SearchText + "%")
            //    ).OrderBy(x=>x.Title).ToList();
            //return View("Index",products);

            //var query = _context.Products
            //    .Where(x => EF.Functions.Like(x.Title, "%" + SearchText + "%") ||
            //    EF.Functions.Like(x.Tags, "%" + SearchText + "%"));

            //var totalProducts = query.Count();

            //var products = query
            //    .OrderBy(x => x.Title)
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();

            //ViewData["Categories"] = _context.Categories.ToList();
            //ViewData["CurrentPage"] = page;
            //ViewData["TotalPages"] = (int)Math.Ceiling((double)totalProducts / pageSize);
            //ViewData["SearchText"] = SearchText;

            //return View("Index", products);

            var query = _context.Products
        .Where(x => EF.Functions.Like(x.Title, "%" + SearchText + "%") ||
                    EF.Functions.Like(x.Tags, "%" + SearchText + "%"))
        .OrderBy(x => x.Title);

            var products = PaginationHelper.Paginate(query, page, pageSize, out int totalPages);
            var totalProducts = query.Count();

            ViewData["SearchText"] = SearchText;
            ViewData["Categories"] = _context.Categories.ToList();
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["TotalProducts"] = totalProducts;

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


    }
}
