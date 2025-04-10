using InventoryMgmt.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMgmt.Areas.ProductManagement.Controllers;

[Area("ProductManagement")]
[Route("[area]/[controller]")]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    
    public OrderController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("")]
    public IActionResult Index()
    {
        var orders = _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.OrderId)
            .ToList();

        return View(orders);
    }
    
    [Authorize]
    [HttpGet("MyOrders")]
    public async Task<IActionResult> MyOrders()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var orders = _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserEmail == user.Email)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return View(orders);
    }
}