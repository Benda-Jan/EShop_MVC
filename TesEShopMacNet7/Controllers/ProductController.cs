using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestEShopMacNet7.Data;
using TestEShopMacNet7.Models.Product;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestEShopMacNet7.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult Details(string? Id)
        {
            if (ModelState.IsValid && Id is not null)
            {
                var product = dbContext.Products.Where(p => p.Id == Id).FirstOrDefault();
                return View(product);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<bool> Delete(string? Name)
        {
            if (ModelState.IsValid)
            {
                var product = dbContext.Products.Where(p => p.Name == Name).FirstOrDefault();
                if (product is not null && Name is not null)
                {
                    dbContext.Products.Remove(product);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product? Model)
        {
            if (Model is not null)
            {
                Model.Id = Guid.NewGuid().ToString();
                Model.Category = dbContext.Categories.Where(c => c.Id == Model.CategoryId).FirstOrDefault();
                Model.Brand = dbContext.Brands.Where(b => b.Id == Model.BrandId).FirstOrDefault();
                Model.TimeAdded = DateTime.Now.ToFileTime();
                //if (ModelState.IsValid)
                //{

                await dbContext.Products.AddAsync(Model);
                    await dbContext.SaveChangesAsync();
                //}
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Edit(string? Id)
        {
            if (ModelState.IsValid)
            {
                var product = dbContext.Products.Where(p => p.Id == Id).FirstOrDefault();
                if (product is not null)
                {
                    return View(product);
                }
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product? Model)
        {
            if (Model is not null && Model.Category is null)
                Model.Category = dbContext.Categories.Where(c => c.Id == Model.CategoryId).FirstOrDefault();
            if (Model is not null && Model.Brand is null)
                Model.Brand = dbContext.Brands.Where(b => b.Id == Model.BrandId).FirstOrDefault();

            if (ModelState.IsValid && Model is not null)
            {
                dbContext.Update(Model);
                await dbContext.SaveChangesAsync();
                return View(Model);
            }
            return RedirectToAction("Index", "Product");
        }
    }
}

