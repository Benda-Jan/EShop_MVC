using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TestEShopMacNet7.Data;
using TestEShopMacNet7.Models.Account;
using TestEShopMacNet7.Models.Cart;
using System.Text;
using TestEShopMacNet7.Services;

namespace TestEShopMacNet7.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext dbContext;
        private UserManager<ExtendedUser> userManager;
        private EmailSenderService emailSenderService;

        public CartController(ApplicationDbContext dbContext, UserManager<ExtendedUser> userManager, EmailSenderService emailSenderService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.emailSenderService = emailSenderService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var userId = (await userManager.FindByNameAsync(User?.Identity?.Name ?? ""))?.Id;
            return View(dbContext.CartItems.Where(c => c.UserId == userId));
        }

        [HttpPost]
        public async Task<bool> AddToCart(string ProductId)
        {
            var userId = (await userManager.FindByNameAsync(User?.Identity?.Name ?? ""))?.Id;
            if (userId is not null) {
                var product = dbContext.Products.Where(p => p.Id == ProductId).FirstOrDefault();
                if (product is not null && product.InStock > 0)
                {
                    var cartItem = dbContext.CartItems.Where(c => c.UserId == userId && c.ProductId == ProductId).FirstOrDefault();
                    if (cartItem is null)
                    {
                        cartItem = new CartItem
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = userId,
                            ProductId = ProductId,
                            ProductName = product.Name,
                            Count = 1,
                            Price = (product.SalePrice == 0 ? product.Price : product.SalePrice)
                        };

                        product.InStock--;
                        dbContext.Update(product);
                        await dbContext.AddAsync(cartItem);
                        dbContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        product.InStock--;
                        dbContext.Update(product);
                        cartItem.Count++;
                        dbContext.Update(cartItem);
                        dbContext.SaveChanges();
                        return true;
                    }
                }

            }
            return false;
        }

        [HttpPost]
        public async Task<bool> RemoveFromCart(string ProductId)
        {
            var userId = (await userManager.FindByNameAsync(User?.Identity?.Name ?? ""))?.Id;
            if (userId is not null)
            {
                var product = dbContext.Products.Where(p => p.Id == ProductId).FirstOrDefault();
                if (product is not null)
                {
                    var cartItem = dbContext.CartItems.Where(c => c.UserId == userId && c.ProductId == ProductId).FirstOrDefault();
                    if (cartItem is not null && cartItem.Count > 0)
                    {
                        product.InStock++;
                        dbContext.Update(product);
                        cartItem.Count--;
                        if (cartItem.Count == 0)
                            dbContext.Remove(cartItem);
                        else
                            dbContext.Update(cartItem);
                        dbContext.SaveChanges();
                        return true;
                        //return cartItem.Price;
                    }
                }

            }
            return false;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(Order Model)
        {
            return RedirectToAction("OrderStep1", "Cart", new { ItemIds = Model.ItemIds});
        }

        public IActionResult OrderStep1(Order Model, string? TempItemIds)
        {
            if (TempItemIds is not null)
                Model.ItemIds = TempItemIds.Split(";", StringSplitOptions.RemoveEmptyEntries);
            return View(Model);
        }

        public IActionResult OrderStep2(Order Model, string? TempItemIds)
        {
            if (TempItemIds is not null)
                Model.ItemIds = TempItemIds.Split(";", StringSplitOptions.RemoveEmptyEntries);
            return View(Model);
        }

        public IActionResult OrderStep3(Order Model, string? TempItemIds)
        {
            if (TempItemIds is not null)
                Model.ItemIds = TempItemIds.Split(";", StringSplitOptions.RemoveEmptyEntries);
            return View(Model);
        }

        public async Task<IActionResult> OrderFinished(Order Model, string? TempItemIds)
        {
            if (User is not null && User.Identity is not null)
            {
                //var user = dbContext.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault(); //To address
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Děkujeme za objednávku.");
                stringBuilder.AppendLine("");

                // Generate order, mail
                if (TempItemIds is not null)
                    Model.ItemIds = TempItemIds.Split(";", StringSplitOptions.RemoveEmptyEntries);
                if (Model.ItemIds is not null)
                {
                    foreach (var cartId in Model.ItemIds)
                    {
                        var cartEntity = dbContext.CartItems.Where(c => c.Id == cartId).FirstOrDefault();
                        if (cartEntity is not null)
                        {
                            stringBuilder.AppendLine(cartEntity.ProductName + ", " + cartEntity.Price + " * " + cartEntity.Count);
                            dbContext.CartItems.Remove(cartEntity);
                        }
                    }
                    await dbContext.SaveChangesAsync();
                    emailSenderService.SendTo(stringBuilder.ToString(), "TestEShopMacNet7", "jan.benda97@seznam.cz");
                }
            }
            return View(Model);
        }
    }
}

