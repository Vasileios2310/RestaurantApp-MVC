using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repositories;

namespace RestaurantApp.Controllers;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private Repository<Product> _productRepository;
    private Repository<Order> _orderRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _productRepository = new Repository<Product>(context);
        _orderRepository = new Repository<Order>(context);
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
        {
            OrderItems = new List<OrderItemViewModel>(),
            Products = await _productRepository.GetAllAsync()
        };
        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddItem(int prodId, int prodQty)
    {
        var product = await _context.Products.FindAsync(prodId);
        if (product == null)
        {
            return NotFound(); //404 error
        }

        // Retrieve or create an OrderViewModel from session or other state management
        var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel
        {
            OrderItems = new List<OrderItemViewModel>(),
            Products = await _productRepository.GetAllAsync()
        };
        
        // Check if the product is already in order
        var existingItem = model.OrderItems.FirstOrDefault(x => x.ProductId == prodId);
        
        // If the product exists , update the quantity
        if (existingItem != null)
        {
            existingItem.Quantity += prodQty;
        }
        else
        {
            model.OrderItems.Add(new OrderItemViewModel
            {
                ProductId = product.ProductId,
                Price = product.Price,
                Quantity = prodQty,
                ProductName = product.Name
            });
        }
        // update the total amount
        model.TotalAmount = model.OrderItems.Sum(x => x.Quantity * x.Price);

        // save updated OrderViewModel to session
        HttpContext.Session.Set("OrderViewModel", model);
        
        return RedirectToAction("Create" , model);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Cart()
    {
        var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
        if (model == null || model.OrderItems.Count == 0)
        {
            return RedirectToAction("Create");
        }
        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PlaceOrder()
    {
        var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel");
        if (model == null || model.OrderItems.Count == 0)
        {
            return RedirectToAction("Create");
        }

        // create a new order entity
        Order order = new Order
        {
            OrderDate = DateTime.Now,
            TotalAmount = model.TotalAmount,
            UserId = _userManager.GetUserId(User)
        };
        
        // add order items to order entity
        foreach (var item in model.OrderItems)
        {
            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            });
        }
        
        // save the order entity in database
        await _orderRepository.AddAsync(order);
        
        // clear the orderviewmodel from session 
        HttpContext.Session.Remove("OrderViewModel");
        
        // redirect to the order confirmation page
        return RedirectToAction("ViewOrders");
    }

    /// <summary>
    /// fetches the user's orders with full details
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ViewOrders()
    {
        var userId = _userManager.GetUserId(User);

        var userOrders = await _orderRepository.GetAllByIdAsync(userId, "UserId", new QueryOptions<Order>
        {
            Includes = "OrderItems.Product",
        });
        return View(userOrders);
    }

    public IActionResult Index()
    {
        return View();
    }
}