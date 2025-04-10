using System.Text.Json;
using InventoryMgmt.Data;
using InventoryMgmt.Models;
using InventoryMgmt.Areas.ProductManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMgmt.Areas.ProductManagement.Controllers;

[Authorize]
[Area("ProductManagement")]
[Route("[area]/[controller]")]
public class CartController : Controller
{
    private readonly ApplicationDbContext _context; // Holds the db context
    private readonly UserManager<IdentityUser> _userManager;
    
    // Dependency injection
    public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        // Get cart from session and return it to the view
        var cart = GetCart();
        return View(cart);
    }

    // Add products to the cart
    [HttpGet("AddToCart/{productId:int}")]
    public IActionResult AddToCart(int productId)
    {
        // Get the product from db
        var product = _context.Products.Find(productId);
        
        // Product not found or if it's deleted
        if (product == null)
        {
            return NotFound();
        }
    
        // Get the cart from the session
        var cart = GetCart();
        
        // Look for product already in the cart
        var Cart = cart.FirstOrDefault(i => i.ProductId == productId);

        if (Cart == null)
        {
            // Add product to the cart
            cart.Add(new Cart { 
                ProductId = product.ProductId, 
                ProductName = product.ProductName, 
                Price = product.Price, 
                Quantity = 1 
            });
        }
        else
        {   
            // Increment the quantity of product already in cart
            Cart.Quantity++;
        }
    
        // Save the cart to the session again
        SaveCart(cart);

        // Return JSON since this method is called by thr AJAX
        return Json(new { message = $"{product.ProductName} added to cart." });
    }

    // Remove Item from Cart
    [HttpGet("RemoveFromCart/{productId:int}")]
    public IActionResult RemoveFromCart(int productId)
    {
        // Get cart from session
        var cart = GetCart();
        
        // Remove where product id matches
        cart.RemoveAll(i => i.ProductId == productId);
        
        // Save cart again
        SaveCart(cart);
        
        // Refrsh the page basically
        return RedirectToAction("Index");
    }

    // Checkout: Save Order to Database
    [HttpPost("PlaceOrder")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(string Name)
    {
        var cart = GetCart();
        if (!cart.Any())
            return RedirectToAction("Index");

        string? userEmail = null;

        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            userEmail = user?.Email;
        }

        var order = new Order
        {
            CustomerName = Name,
            OrderDate = DateTime.UtcNow,
            Total = cart.Sum(i => i.Price * i.Quantity),
            OrderItems = cart.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList(),
            UserEmail = userEmail
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        foreach (var item in cart)
        {
            var product = _context.Products.Find(item.ProductId);
            if (product != null)
            {
                product.Quantity -= item.Quantity;
                _context.Products.Update(product);
            }
        }

        _context.SaveChanges();
        HttpContext.Session.Remove("Cart");

        TempData["success"] = $"Your order (#{order.OrderId}) has been placed!";
        return RedirectToAction("MyOrders", "Order");
    }

    
    // Get cart from session
    private List<Cart> GetCart()
    {
        var cartJson = HttpContext.Session.GetString("Cart");
        return cartJson != null ? JsonSerializer.Deserialize<List<Cart>>(cartJson) ?? new List<Cart>() : new List<Cart>();
    }

    // Save cart to session
    private void SaveCart(List<Cart> cart)
    {
        HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
    }
    
}