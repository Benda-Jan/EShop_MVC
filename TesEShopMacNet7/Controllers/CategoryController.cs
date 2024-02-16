using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestEShopMacNet7.Data;
using TestEShopMacNet7.Models.Product;
using TestEShopMacNet7.Models.Role;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestEShopMacNet7.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<bool> Delete(string? Id)
        {
            if (ModelState.IsValid)
            {
                var category = dbContext.Categories.Where(c => c.Id == Id).FirstOrDefault();
                if (category is not null && Id is not null)
                {
                    dbContext.Categories.Remove(category);
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
        public async Task<IActionResult> Create(string? InternalName, string? DisplayName)
        {
            if (ModelState.IsValid && InternalName is not null && DisplayName is not null)
            {
                    await dbContext.Categories.AddAsync(new Models.Product.Category { InternalName = InternalName, DisplayName = DisplayName, Id = Guid.NewGuid().ToString()});
                    await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(string? Id)
        {
            var category = dbContext.Categories.Where(c => c.Id == Id).FirstOrDefault();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category? Model)
        {
            if (ModelState.IsValid && Model is not null){
                dbContext.Update(Model);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Category");
        }
    }
}

