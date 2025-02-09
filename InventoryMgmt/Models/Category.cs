using System.ComponentModel.DataAnnotations;

namespace InventoryMgmt.Models;

public class Category
{
    public int CategoryId { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    public string? Description { get; set; } // Can be null
    
    // Navigation Property: One Category -> Many Products
    public List<Product> Products { get; set; } = new();
    
}