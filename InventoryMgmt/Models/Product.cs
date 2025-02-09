using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMgmt.Models;

public class Product
{
    public int ProductId { get; set; }
    
    [Required]
    public required string ProductName { get; set; }
    
    [Required]
    public int CategoryId { get; set; }  // Foreign key

    [ForeignKey("CategoryId")]
    public Category Category { get; set; } = null!; // Navigation property
    
    [Required]
    public required double Price { get; set; }

    // Set to default when not provided - ? not needed as it shouldn't be null in db
    public int Quantity { get; set; } = 0;
    public int LowStockThreshold { get; set; } = 10;

}