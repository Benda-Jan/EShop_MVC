using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TestEShopMacNet7.Models.Account;
using TestEShopMacNet7.Models.Role;
using Microsoft.EntityFrameworkCore;
using TestEShopMacNet7.Data;

namespace TestEShopMacNet7.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ExtendedUser> userManager;

        private ApplicationDbContext dbContext;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ExtendedUser> userManager, ApplicationDbContext dbContext)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole? model)
        {
            if (ModelState.IsValid && model is not null && model.Name is not null)
            {
                var result = await roleManager.CreateAsync(model);
                if (result.Succeeded)
                    return RedirectToAction("Edit", "Role", new { model.Id});
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string? Id)
        {
            var role = dbContext.Roles.Where(r => r.Id == Id).FirstOrDefault();
            var model = new RoleViewModel(userManager);
            if (role is not null)
            {
                model.Id = role.Id;
                model.Name = role.Name ?? "Not-found";
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModification? model)
        {
            if (ModelState.IsValid && model is not null && model.Name is not null)
            {
                if (model.AddIds is not null)
                {
                    foreach (var addId in model.AddIds)
                    {
                        var user = await userManager.FindByIdAsync(addId);
                        if (user is not null)
                            await userManager.AddToRoleAsync(user, model.Name);
                    }
                }
                if (model.RemoveIds is not null)
                {
                    foreach (var removeId in model.RemoveIds)
                    {
                        var user = await userManager.FindByIdAsync(removeId);
                        if (user is not null)
                            await userManager.RemoveFromRoleAsync(user, model.Name);
                    }
                }
            }
            //if (ModelState.IsValid && model is not null)
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<bool> Delete(string Name)
        {
            var role = await roleManager.FindByNameAsync(Name);
            if (role is not null)
            {
                var result = await roleManager.DeleteAsync(role);
                return result.Succeeded;
            }
            return false;
        }
    }
}

