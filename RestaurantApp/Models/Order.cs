namespace RestaurantApp.Models;
/// <summary>
/// The UserId --> Foreign Key --> EF Core actually uses in the database, to link the order to a user — it's the connection point.
/// The User --> Navigation Property --> This is not required for EF Core to build the relationship.
/// </summary>
public class Order
{
    public Order()
    {
        OrderItems =new List<OrderItem>();
    }

    public int OrderId { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public string? UserId { get; set; } // Foreign Key
    
    public ApplicationUser User { get; set; } // navigation property
    
    public decimal TotalAmount { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; }
}