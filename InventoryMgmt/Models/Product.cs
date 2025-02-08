using System.ComponentModel.DataAnnotations;

namespace InventoryMgmt.Models;

public class Product
{
    public int ProductId { get; set; }
    
    [Required]
    public required string ProductName { get; set; }
    
    [Required]
    public required string Category { get; set; }
    
    [Required]
    public required double Price { get; set; }

    // Set to default when not provided - ? not needed as it shouldn't be null in db
    public int Quantity { get; set; } = 0;
    public int LowStockThreshold { get; set; } = 10;

}