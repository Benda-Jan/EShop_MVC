using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestEShopMacNet7.Data;
using TestEShopMacNet7.Models;
using TestEShopMacNet7.Models.Product;

namespace TestEShopMacNet7.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext dbContext;
    public readonly int ProductsPerPage = 8;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        this.dbContext = dbContext;
    }

    public IActionResult Index()
    {
        ViewData["Pages"] = (int)Math.Ceiling((double)dbContext.Products.Count() / ProductsPerPage);
        return View(dbContext.Products.Take(ProductsPerPage));
    }

    [HttpPost]
    public IActionResult Index(string[]? Categories, string[]? Brands, string OrderBy, int ActualPage)
    {
        IQueryable<Product> productsCategory = dbContext.Products; 
        IQueryable<Product> brandsCategory = dbContext.Products;
        if (Categories is not null && Categories.Count() > 0)
        {
            ViewData["Categories"] = String.Join(",", Categories);
            productsCategory = dbContext.Products.Where(p => Categories.Any(c => c == p.CategoryId));
        }
        if (Brands is not null && Brands.Count() > 0)
        {
            ViewData["Brands"] = String.Join(",", Brands);
            brandsCategory = dbContext.Products.Where(p => Brands.Any(b => b == p.BrandId));
        }
        var products = productsCategory.Intersect(brandsCategory);

        switch (OrderBy)
        {
            case "cheap":
                products = products.OrderBy(a => a.SalePrice == 0 ? a.Price : a.SalePrice);
                break;
            case "expensive":
                products = products.OrderByDescending(a => a.SalePrice == 0 ? a.Price : a.SalePrice);
                break;
            case "newest":
                products = products.OrderBy(a => a.TimeAdded);
                break;
            case "oldest":
                products = products.OrderByDescending(a => a.TimeAdded);
                break;
            default:
                break;
        }
        //products = products.OrderBy(a => a.Price);

        ViewData["OrderBy"] = OrderBy;
        ViewData["ActualPage"] = ActualPage;
        ViewData["Pages"] = (int)Math.Ceiling((double)products.Count() / ProductsPerPage);
        return View(products.Skip((ActualPage - 1) * ProductsPerPage).Take(ProductsPerPage));
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

