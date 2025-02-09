using InventoryMgmt.Data;
using InventoryMgmt.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgmt.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context; // Holds the database context

    // Dependency injection for the database within the constructor
    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        // Get all the categories
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(Category category)
    {
        if (ModelState.IsValid)
        { 
            _context.Categories.Add(category); // Add new category
            _context.SaveChanges(); // Commit changes
            
            TempData["success"] = $"The category {category.Name} has been added successfully.";
            
            return RedirectToAction("Index"); // Redirect to Index (List of categories)
        }
        return View(category);
    }
    
}
