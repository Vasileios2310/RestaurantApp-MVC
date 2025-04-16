using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Models;

namespace RestaurantApp.Data;
/// <summary>
/// This is custom DbContext class uses ApplicationUser for identity, like User, Role and Claims
/// and constructor is for configure connection between db and app.
/// Db<Set> is like a container for querying and saving instances of your entity classes.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Product>? Products { get; set; }
    
    public DbSet<Category> Categories{ get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderItem> OrderItems { get; set; }
    
    public DbSet<Ingredient> Ingredients { get; set; }
    
    public DbSet<ProductIngredient> ProductIngredients { get; set; }

    /// <summary>
    /// This method allows me to make the shape for relationship, between tables 
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ProductIngredient>()
            .HasKey(pi => new { pi.ProductId, pi.IngredientId });
        
        modelBuilder.Entity<ProductIngredient>()
            .HasOne(pi => pi.Ingredient)
            .WithMany(pi => pi.ProductIngredients)
            .HasForeignKey(pi => pi.IngredientId);
        
        modelBuilder.Entity<ProductIngredient>()
            .HasOne(pi=>pi.Product)
            .WithMany(pi=>pi.ProductIngredients)
            .HasForeignKey(pi=>pi.ProductId);
        
        // Seed data --> to populate the table Category
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Appetizer" },
            new Category { CategoryId = 2, Name = "Entree"},
            new Category { CategoryId = 3, Name = "Side Dish"},
            new Category { CategoryId = 4, Name = "Dessert"},
            new Category { CategoryId = 5, Name = "Beverage"}
            );
        // Seed data --> to populate the table Ingredient
        modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient { IngredientId = 1, Name = "Beef" },
            new Ingredient { IngredientId = 2, Name = "Chicken" },
            new Ingredient { IngredientId = 3, Name = "Fish" },
            new Ingredient { IngredientId = 4, Name = "Tortilla" },
            new Ingredient { IngredientId = 5, Name = "Lettuce" },
            new Ingredient { IngredientId = 6, Name = "Tomato" }
            );
        
        // Seed data --> to populate the table Product , m is for compiling into a float to decimal
        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, Name = "Beef Taco" , Description = "A delicious beef taco" , Price = 3.00m, Stock = 100, CategoryId = 2},
            new Product { ProductId = 2, Name = "Chicken Taco" , Description = "A delicious chicken taco" , Price = 3.50m, Stock = 101, CategoryId = 2},
            new Product { ProductId = 3, Name = "Fish Taco" , Description = "A delicious taco" , Price = 1.95m, Stock = 90, CategoryId = 2}
            );

        // Seed data, we say that ProductId=1 --> Beef taco,  has IngredientId = 4 --> tortilla , IngredientId = 5 --> lettuce, IngredientId = 6 --> tomato etc.
        modelBuilder.Entity<ProductIngredient>().HasData(
            new ProductIngredient { ProductId = 1, IngredientId = 1 },
            new ProductIngredient { ProductId = 1, IngredientId = 4 },
            new ProductIngredient { ProductId = 1, IngredientId = 5 },
            new ProductIngredient { ProductId = 1, IngredientId = 6 },
            new ProductIngredient { ProductId = 2, IngredientId = 2 },
            new ProductIngredient { ProductId = 2, IngredientId = 4 },
            new ProductIngredient { ProductId = 2, IngredientId = 5 },
            new ProductIngredient { ProductId = 2, IngredientId = 6 },
            new ProductIngredient { ProductId = 3, IngredientId = 3 },
            new ProductIngredient { ProductId = 3, IngredientId = 4 },
            new ProductIngredient { ProductId = 3, IngredientId = 5 },
            new ProductIngredient { ProductId = 3, IngredientId = 6 }
        );
    }
}
