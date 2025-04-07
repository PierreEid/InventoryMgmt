using InventoryMgmt.Data;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgmt.Areas.ProductManagement.Controllers;

[Area("ProductManagement")]
[Route("[area]/[controller]/[action]")]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var orders = _context.Orders.OrderByDescending(o => o.OrderId).ToList();
        return View(orders);
    }
}