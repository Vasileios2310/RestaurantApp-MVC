using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repositories;

namespace RestaurantApp.Controllers;

public class IngredientController : Controller
{
    
    private Repository<Ingredient> ingredientRepository;

    public IngredientController(ApplicationDbContext context)
    { 
        ingredientRepository = new Repository<Ingredient>(context);
    }

    public async Task<IActionResult> Index()
    {
        return View(await ingredientRepository.GetAllAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        return View(await ingredientRepository.GetByIdAsync(id, new QueryOptions<Ingredient>() { Includes = "ProductIngredients.Product"}));
    }
}