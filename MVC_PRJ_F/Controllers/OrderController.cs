using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Migrations;
using MVC_PRJ_F.Models;
using Stripe;
using Stripe.Checkout;


namespace MVC_PRJ_F.Controllers;
[Authorize]
public class OrderController:Controller

{

    private readonly ICartRepository _cartRepository;
    private readonly ICinemaMovieRepository _CinemaMovieRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, 
        ICinemaMovieRepository cinemaMovieRepository, ICouponRepository couponRepository)
    {
        _cartRepository = cartRepository;
        _userManager = userManager;
        _CinemaMovieRepository = cinemaMovieRepository;
        _couponRepository = couponRepository;
    }

    [HttpGet]
    public async Task<IActionResult> AddToCart(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();
        var cnMovie = await _CinemaMovieRepository.GetByIdAsync(id);
        if (cnMovie.Quantity <= 0)
        {
            TempData["Error"] = "No seats available for this movie.";
            return RedirectToAction("GetAllByMovieId","CinemaMovie",new {id = cnMovie.CinemaId});

        }
            
        
        var existingCart = await _cartRepository.GetByMovieAndUserAsync(id, user.Id);

        // لو السلة تحتوي على نفس الفيلم → نزود الكمية
        if (existingCart != null)
        {
            existingCart.Quantity++;
            existingCart.TotalPrice += cnMovie.Price;

            cnMovie.Quantity--;
            await _CinemaMovieRepository.UpdateAsync(cnMovie);
            await _cartRepository.UpdateAsync(existingCart);
            var carts1 = await _cartRepository.GetByIdUserAsync(user.Id);
            int totalPrice1=0;
            foreach (var c in carts1)
            {
                totalPrice1 += c.TotalPrice;
            }
            user.TotalCarts = totalPrice1;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(GetAll));
        }

        // غير موجود بالسلة → أنشئ Cart جديد
        var cartUser = new Cart
        {
            UserId = user.Id,
            Quantity = 1,
            CinemaMovieId = id,
            TotalPrice = cnMovie.Price
        };

        cnMovie.Quantity--; // ↓ نقص المقعد
        await _CinemaMovieRepository.UpdateAsync(cnMovie);
        await _cartRepository.CreatAsync(cartUser);
        var carts = await _cartRepository.GetByIdUserAsync(user.Id);
        int totalPrice=0;
        foreach (var c in carts)
        {
            totalPrice += c.TotalPrice;
        }
        user.TotalCarts = totalPrice;
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(GetAll));

    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();
        
        var carts = await _cartRepository.GetByIdUserAsync(user.Id);
        
        ViewBag.TotalCarts = user.TotalCarts;
        return View(carts);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCart(int Quantity,int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();
        
        
        var cartUser = await _cartRepository.GetByIdAsync(id);
        if (cartUser == null)
            return NotFound();
        user.TotalCarts -= cartUser.TotalPrice;
        var movie = await _CinemaMovieRepository.GetByIdAsync(cartUser.CinemaMovieId);
        int oldQty = cartUser.Quantity;

        // لو زاد الكمية
        if (Quantity > oldQty)
        {
            int diff = Quantity - oldQty;
            if (movie.Quantity < diff)
            {
                TempData["Error"] = "Not enough seats available!";
                return RedirectToAction(nameof(GetAll));
            }
            movie.Quantity -= diff;
        }
        else if (Quantity < oldQty) // لو قلل الكمية
        {
            int diff = oldQty - Quantity;
            movie.Quantity += diff;
        }
        cartUser.Quantity = Quantity;
        cartUser.TotalPrice = movie.Price*Quantity;
        await _CinemaMovieRepository.UpdateAsync(movie);
        await _cartRepository.UpdateAsync(cartUser);
        var carts = await _cartRepository.GetByIdUserAsync(user.Id);
        int totalPrice=0;
        foreach (var c in carts)
        {
            totalPrice += c.TotalPrice;
        }
        user.TotalCarts = totalPrice;
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(GetAll));
    }

    [HttpGet]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var cartUser = await _cartRepository.GetByIdAsync(id);
        if (cartUser == null)
            return NotFound();
        var movie = await _CinemaMovieRepository.GetByIdAsync(cartUser.CinemaMovieId);
        movie.Quantity = movie.Quantity + cartUser.Quantity;
        
        var user = await _userManager.GetUserAsync(User);
        await _CinemaMovieRepository.UpdateAsync(movie);
        await _cartRepository.DeleteAsync(cartUser.Id);
        var carts = await _cartRepository.GetByIdUserAsync(user.Id);
        int totalPrice=0;
        foreach (var c in carts)
        {
            totalPrice += c.TotalPrice;
        }
        user.TotalCarts = totalPrice;
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(GetAll));
    }


    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(string code)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var carts = await _cartRepository.GetByIdUserAsync(user.Id);
        int totalPrice=0;
        foreach (var c in carts)
        {
            totalPrice += c.TotalPrice;
        }
        
        var cop = await _couponRepository.GetByCodeAsync(code);
        if (cop == null)
            return NotFound();
        int discount = (totalPrice * cop.DiscountAmount) / 100;

        if (discount > 100)
            discount = 100;
        totalPrice -= discount;
        
        
        
        
        
        user.TotalCarts = totalPrice;
        if(user.TotalCarts < 0)
            user.TotalCarts = 0;
        await _userManager.UpdateAsync(user);
        
        // TempData["Success"] = $"Coupon applied! You saved {discount:F0} EGP.";
        return RedirectToAction(nameof(GetAll));
    }




    public async Task<IActionResult> Pay()
    {
        // StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        var user = await _userManager.GetUserAsync(User);
        var carts = await _cartRepository.GetByIdUserAsync(user.Id);

        var lineItems = new List<SessionLineItemOptions>();
        
            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "egp",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Cart Total After Discount",
                    },
                    UnitAmount = user.TotalCarts*100,
                },
                Quantity = 1,
            });
        

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = $"{Request.Scheme}://{Request.Host}/Order/success",
            CancelUrl = $"{Request.Scheme}://{Request.Host}/Order/cancel",
        };

        var service = new SessionService();
        var session = service.Create(options);

        return Redirect(session.Url);
    }
    
    [HttpGet]
    public async Task<IActionResult> success()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();
        var carts = await _cartRepository.GetByIdUserAsync(user.Id);
        await _cartRepository.DeleteByUserAsync(user.Id);
        return RedirectToAction(nameof(GetAll));
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}