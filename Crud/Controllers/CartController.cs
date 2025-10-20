using Crud.Data;
using Crud.ViewModel;   
using Crud.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    public readonly ApplicationDBContext dbContext;

    public CartController(ApplicationDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // ✅ Add to Cart
    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] CartItemViewModel cartItem)
    {
        var existingItem = await dbContext.CartItems
            .FirstOrDefaultAsync(c => c.UserId == cartItem.UserId && c.ProductId == cartItem.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += cartItem.Quantity;
            dbContext.CartItems.Update(existingItem);
        }
        else
        {
            var newItem = new CartItem
            {
                Id = Guid.NewGuid(),
                UserId = cartItem.UserId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                CreatedDate = DateTime.UtcNow
            };
            dbContext.CartItems.Add(newItem);
        }

        await dbContext.SaveChangesAsync();
        return Ok(new { message = "Item added to cart successfully" });
    }

    // ✅ Get all user cart items
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(Guid userId)
    {
        var cartItems = await dbContext.CartItems
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .Select(c => new CartItemViewModel
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductName = c.Product.ProductName,
                Price = c.Product.PriceInCents,
                Image = c.Product.Image,
                Quantity = c.Quantity
            })
            .ToListAsync();

        return Ok(cartItems);
    }

    // ✅ Remove a single item
    [HttpDelete("{cartItemId}")]
    public async Task<IActionResult> RemoveItem(Guid cartItemId)
    {
        var item = await dbContext.CartItems.FindAsync(cartItemId);
        if (item == null) return NotFound();

        dbContext.CartItems.Remove(item);
        await dbContext.SaveChangesAsync();
        return Ok(new { message = "Item removed" });
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateCartItem([FromBody] CartItemViewModel cartItem)
    {
        var existingItem = await dbContext.CartItems.FindAsync(cartItem.Id);

        if (existingItem == null)
            return NotFound(new { message = "Cart item not found" });

        existingItem.Quantity = cartItem.Quantity;
        dbContext.CartItems.Update(existingItem);
        await dbContext.SaveChangesAsync();

        return Ok(new { message = "Cart item updated successfully" });
    }



    [HttpGet("get-user-orders/{userId}")]
    public async Task<IActionResult> GetUserOrders(Guid userId)
    {
        try
        {
            var orders = await dbContext.BuyedProducts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .Select(c => new BuyedProductViewModel
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    ProductId = c.ProductId,
                    ProductName = c.Product.ProductName,
                    Quantity = c.Quantity,
                    TotalAmount = c.TotalAmount,
                    PurchaseDate = c.PurchaseDate
                })
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound(new { message = "No orders found for this user." });
            }

            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error fetching user orders.",
                error = ex.Message
            });
        }
    }


    [HttpGet("get-cart-count/{userId}")]
    public async Task<IActionResult> GetCartCount(Guid userId)
    {
        var cartCount = await dbContext.CartItems.CountAsync(c => c.UserId == userId);

        return Ok(new CartCountViewModel
        {
            CartCount = cartCount
        });
    }


}
