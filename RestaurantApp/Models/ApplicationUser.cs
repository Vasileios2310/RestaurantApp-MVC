using Microsoft.AspNetCore.Identity;

namespace RestaurantApp.Models;
/// <summary>
/// This is a one-to-many relationship from ApplicationUser to Order.
/// </summary>
public class ApplicationUser : IdentityUser
{ 
    public ICollection<Order>? Orders { get; set; }
}