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

    //Ingredient/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IngredientId, Name")] Ingredient ingredient)
    {
        if (ModelState.IsValid)
        {
            await ingredientRepository.AddAsync(ingredient);
            return RedirectToAction("Index");
        }
        return View(ingredient);
    }
    
    //Ingredient/Edit
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        return View(await ingredientRepository.GetByIdAsync(id, new QueryOptions<Ingredient>
            {
                Includes = "ProductIngredients.Product"
            }
        ));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Ingredient ingredient)
    {
        await ingredientRepository.UpdateAsync(ingredient);
        return RedirectToAction("Index");
    }
    
    //Ingredient/Delete
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        return View(await ingredientRepository.GetByIdAsync(id, new QueryOptions<Ingredient> { Includes = "ProductIngredients.Product"}));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Ingredient ingredient)
    {
        await ingredientRepository.DeleteAsync(ingredient.IngredientId);
        return RedirectToAction("Index");
    }
}