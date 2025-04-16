namespace RestaurantApp.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }
    
    public int OrderId { get; set; } // Foreign Key
    
    public int ProductId { get; set; } // Foreign Key
    
    public Order? Order { get; set; } // navigation property
    
    public Product? Product { get; set; } // navigation property
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
}