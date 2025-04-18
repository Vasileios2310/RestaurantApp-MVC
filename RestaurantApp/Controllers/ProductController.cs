using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repositories;

namespace RestaurantApp.Controllers;

public class ProductController : Controller
{
    private Repository<Product> _productRepository;
    private Repository<Ingredient> _ingredientRepository; 
    private Repository<Category> _categoryRepository;

    public ProductController(ApplicationDbContext context)
    {
        _productRepository = new Repository<Product>(context);
        _ingredientRepository = new Repository<Ingredient>(context);
        _categoryRepository = new Repository<Category>(context);
    }

    public async Task<IActionResult> Index()
    {
        return View(await _productRepository.GetAllAsync());
    }

    /// <summary>
    /// Fetch all ingredients and categories from the database
    /// And store them in ViewBag, which is a dynamic object you can use in Razor views to pass simple data from controller to view.
    /// Dropdown selection in form or Multi-select or checkboxes or 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> AddEdit(int id)
    {
        ViewBag.Ingredients = await _ingredientRepository.GetAllAsync();
        ViewBag.Categories = await _categoryRepository.GetAllAsync();
        if (id == 0)
        {
            ViewBag.Operation  = "Add"; // add a new Product
            return View (new Product());
        }
        else
        {
            ViewBag.Operation = "Edit"; // else edit the product
            return View();
        }
    }
}