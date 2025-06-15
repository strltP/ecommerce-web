﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private readonly OnlineShopContext _context;

        public ProductsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public IActionResult DeleteGallery(int id)
        {
            var gallery = _context.ProductGalleries.FirstOrDefault(x => x.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }
            string d = Directory.GetCurrentDirectory();
            string fn = d + "\\wwwroot\\images\\banners\\" + gallery.ImageName;

            if (System.IO.File.Exists(fn))
            {
                System.IO.File.Delete(fn);
            }
            _context.Remove(gallery);
            _context.SaveChanges();

            return Redirect("edit/" + gallery.ProductId);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,FullDesc,Price,Discount,ImageName,Qty,Tags,VideoUrl")] Product product, IFormFile? MainImage, IFormFile[]? GalleryImages)
        {
            if (ModelState.IsValid)
            {
                //===============save main image==========================
                if (MainImage != null)
                {
                    //-----------------
                    string org_fn;
                    org_fn = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/images/banners/" + product.ImageName);

                    if (System.IO.File.Exists(org_fn))
                    {
                        System.IO.File.Delete(org_fn);
                    }
                    //-----------------
                    product.ImageName = Guid.NewGuid() + Path.GetExtension(MainImage.FileName);
                    //-----------------
                    string ImagePath;
                    ImagePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\images\\banners\\" + product.ImageName);

                    using (var stream = new FileStream(ImagePath, FileMode.Create))
                    {
                        MainImage.CopyTo(stream);
                    }

                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                
                //===============save gallery images==========================

                if (GalleryImages != null)
                {
                    foreach (var item in GalleryImages)
                    {
                        var newgallery = new ProductGallery();
                        newgallery.ProductId = product.Id;
                        //-----------------------------------
                        newgallery.ImageName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                        string ImagePath;
                        ImagePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\images\\banners\\" + newgallery.ImageName);

                        using (var stream = new FileStream(ImagePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }
                        //--------------------------------------
                        _context.ProductGalleries.Add(newgallery);
                    }
                }
                //------------------------------------
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            //-----------------------
            ViewData["gallery"]= _context.ProductGalleries
                .Where(x => x.ProductId == product.Id)
               .ToList(); 
            //-----------------------

            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,FullDesc,Price,Discount,ImageName,Qty,Tags,VideoUrl")] Product product, IFormFile? MainImage, IFormFile[]? GalleryImages)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //-------save images--------------------
                    //========================================================
                    if (MainImage != null)
                    {
                        string d = Directory.GetCurrentDirectory();
                        // Tạo tên ảnh mới thay vì dùng product.ImageName nếu nó đang null hoặc rỗng
                        var imageName = Guid.NewGuid() + Path.GetExtension(MainImage.FileName);
                        string fn = Path.Combine(d, "wwwroot", "images", "banners", imageName);
                        //------------------------------------------------
                        // Đảm bảo thư mục tồn tại
                        var dir = Path.GetDirectoryName(fn);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        //if (System.IO.File.Exists(fn))
                        //{
                        //    System.IO.File.Delete(fn);
                        //}
                        //------------------------------------------------
                        using (var stream = new FileStream(fn, FileMode.Create))
                        {
                            MainImage.CopyTo(stream);
                        }
                        //------------------------------------------------
                        product.ImageName = imageName; // Cập nhật tên ảnh mới vào sản phẩm
                    }
                    //========================================================
                    if (GalleryImages != null)
                    {
                        foreach (var item in GalleryImages)
                        {

                            var imageName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                            //------------------------------------------------
                            string d = Directory.GetCurrentDirectory();
                            string fn = d + "\\wwwroot\\images\\banners\\" + imageName;
                            //------------------------------------------------
                            using (var stream = new FileStream(fn, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            //------------------------------------------------
                            var galleryItem = new ProductGallery();
                            galleryItem.ImageName = imageName;
                            galleryItem.ProductId = product.Id;
                            //------------------------------------------------
                            _context.ProductGalleries.Add(galleryItem);
                        }
                    }
                    //========================================================
                    //-----------------------------------------
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                //------delete images-------------------------
                string d = Directory.GetCurrentDirectory();
                string fn = d + "\\wwwroot\\images\\banners\\";
                //----------
                string mainImagePath = fn + product.ImageName;
                //--------------------------
                if (System.IO.File.Exists(mainImagePath))
                {
                    System.IO.File.Delete(mainImagePath);
                }
                //--------------------------
                var galleries = _context.ProductGalleries.Where(x => x.ProductId == id).ToList();
                if (galleries != null)
                {
                    //--------------------------
                    foreach (var item in galleries)
                    {
                        string galleryImagePath = fn + item.ImageName;
                        //--------------------------
                        if (System.IO.File.Exists(galleryImagePath))
                        {
                            System.IO.File.Delete(galleryImagePath);
                        }
                        //--------------------------
                    }
                    //--------------------------
                    _context.ProductGalleries.RemoveRange(galleries);
                }
                //--------------------------------------------
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
