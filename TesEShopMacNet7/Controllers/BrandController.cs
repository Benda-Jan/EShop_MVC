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
    public class BrandController : Controller
    {
        private ApplicationDbContext dbContext;

        public BrandController(ApplicationDbContext dbContext)
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
                var brand = dbContext.Brands.Where(c => c.Id == Id).FirstOrDefault();
                if (brand is not null && Id is not null)
                {
                    dbContext.Brands.Remove(brand);
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
        public async Task<IActionResult> Create(string? Name)
        {
            if (ModelState.IsValid && Name is not null)
            {
                    await dbContext.Brands.AddAsync(new Brand(Name));
                    await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Brand");
        }

        public IActionResult Edit(string? Id)
        {
            var brand = dbContext.Brands.Where(b => b.Id == Id).FirstOrDefault();
            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand? Model)
        {
            if (ModelState.IsValid && Model is not null){
                dbContext.Update(Model);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Brand");
        }
    }
}

