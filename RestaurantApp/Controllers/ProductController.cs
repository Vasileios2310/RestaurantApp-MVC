using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repositories;

namespace RestaurantApp.Controllers;

public class ProductController : Controller
    {
        private Repository<Product> productRepository;
        private Repository<Ingredient> ingredientRepository;
        private Repository<Category> categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            productRepository = new Repository<Product>(context);
            ingredientRepository = new Repository<Ingredient>(context);
            categoryRepository = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await productRepository.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.Ingredients = await ingredientRepository.GetAllAsync();
            ViewBag.Categories = await categoryRepository.GetAllAsync();
            if(id==0)
            {
                ViewBag.Operation = "Add";
                return View(new Product());
            }
            else
            {
                Product product = await productRepository.GetByIdAsync(id, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients.Ingredient, Category"
                });
                ViewBag.Operation = "Edit";
                return View(product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(Product product, int[] ingredientIds, int catId)
        {
            ViewBag.Ingredients = await ingredientRepository.GetAllAsync();
            ViewBag.Categories = await categoryRepository.GetAllAsync();
            if (ModelState.IsValid)
            {

                if (product.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = uniqueFileName;
                }

                if (product.ProductId == 0)
                {
                   
                    product.CategoryId = catId;

                    //add ingredientRepository
                    foreach (int id in ingredientIds)
                    {
                        product.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }

                    await productRepository.AddAsync(product);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    var existingProduct = await productRepository.GetByIdAsync(product.ProductId,
                        new QueryOptions<Product> { Includes = "ProductIngredients" });

                    if (existingProduct == null)
                    {
                        ModelState.AddModelError("", "Product not found.");
                        ViewBag.Ingredients = await ingredientRepository.GetAllAsync();
                        ViewBag.Categories = await categoryRepository.GetAllAsync();
                        return View(product);
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Stock = product.Stock;
                    existingProduct.CategoryId = catId;

                    // Update product ingredientRepository
                    existingProduct.ProductIngredients?.Clear();
                    foreach (int id in ingredientIds)
                    {
                        existingProduct.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }

                    try
                    {
                        await productRepository.UpdateAsync(existingProduct);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Error: {ex.GetBaseException().Message}");
                        ViewBag.Ingredients = await ingredientRepository.GetAllAsync();
                        ViewBag.Categories = await categoryRepository.GetAllAsync();
                        return View(product);
                    }
                }
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await productRepository.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Product not found.");
                return RedirectToAction("Index");
            } 
        }
}