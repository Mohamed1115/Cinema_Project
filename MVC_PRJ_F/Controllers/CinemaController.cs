using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Controllers;

public class CinemaController:Controller
{
    private readonly ICinemaRepository _cinemaRepository;
    private string CinemaImagesPath =>
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "CinemaImage");

    public  CinemaController(ICinemaRepository cinemaRepository)
    {
        _cinemaRepository = cinemaRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var Cnm = await _cinemaRepository.GetAllAsync();
        return View(Cnm);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var Cnm = await _cinemaRepository.GetAllAsync();
        return View(Cnm);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var Cnm = await _cinemaRepository.GetCinemaWithMovies(id);
        if (Cnm == null)
            return NotFound();

        return View(Cnm);
    }

    [HttpGet]
    public async Task<IActionResult> GetHalls(int id)
    {
        var Cnm = await _cinemaRepository.GetMovieWithHalls(id);
        if (Cnm == null)
            return NotFound();
        return View(Cnm);
    }
    
    
    
    
    

    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        var cinema = new Cinema();
        return View(cinema);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Cinema cinema, IFormFile Imag)
    {
        try
        {
            // 1. التحقق من ModelState
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                TempData["Error"] = "Validation failed: " + string.Join(", ", errors);
                return View(cinema);
            }

            // 2. معالجة الصورة (اختيارية)
            if (Imag != null && Imag.Length > 0)
            {
                try
                {
                    // التحقق من نوع الملف
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(Imag.FileName).ToLower();
                    
                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["Error"] = "Only image files (jpg, jpeg, png, gif) are allowed";
                        return View(cinema);
                    }

                    // التحقق من حجم الملف (5MB)
                    if (Imag.Length > 5 * 1024 * 1024)
                    {
                        TempData["Error"] = "Image size must be less than 5MB";
                        return View(cinema);
                    }

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(CinemaImagesPath, fileName);

                    // التأكد من وجود المجلد
                    if (!Directory.Exists(CinemaImagesPath))
                    {
                        Directory.CreateDirectory(CinemaImagesPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Imag.CopyToAsync(stream);
                    }

                    cinema.Image = fileName;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Failed to upload image: {ex.Message}";
                    return View(cinema);
                }
            }
            else
            {
                // إذا لم يتم رفع صورة، استخدم صورة افتراضية أو اتركها فاضية
                cinema.Image = "default-cinema.png"; // أو اتركها null حسب الـ model
            }

            // 3. حفظ السينما في قاعدة البيانات
            await _cinemaRepository.CreatAsync(cinema);
            TempData["Success"] = "Cinema has been added successfully.";

            return RedirectToAction(nameof(GetById), new { id = cinema.Id });
        }
        catch (Exception ex)
        {
            // التقاط أي خطأ آخر
            TempData["Error"] = $"Failed to create cinema: {ex.Message}. Inner: {ex.InnerException?.Message}";
            return View(cinema);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var cinema =await _cinemaRepository.GetByIdAsync(id);
    
        if (cinema == null)
        {
            return NotFound();
        }
    
        // مسح الصورة
        if (!string.IsNullOrEmpty(cinema.Image))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), CinemaImagesPath, cinema.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        await _cinemaRepository.DeleteAsync(id);
        TempData["Success"] = "Cinema has been deleted successfully.";

        return RedirectToAction(nameof(GetAll));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var cinema = await _cinemaRepository.GetByIdAsync(id);
        if (cinema == null)
        {
            return NotFound();
        }
        return View(cinema);
    }
    
    [HttpPost]
    [Authorize]
    public async  Task<IActionResult> Edit(Cinema Cnm,IFormFile Imag)
    {
        if (!ModelState.IsValid)
        {
            return View(Cnm);
        }

        // var dbDoc = _db.Doctors.Find(d.Id);
        var cinema = await _cinemaRepository.GetByIdAsync(Cnm.Id);
        if (cinema == null)
        {
            return NotFound();
        }
        if ( Imag is not null && Imag.Length > 0)
        {
            var oldfilePath = Path.Combine(Directory.GetCurrentDirectory(), CinemaImagesPath, cinema.Image);
            if (System.IO.File.Exists(oldfilePath))
            {
                System.IO.File.Delete(oldfilePath);
            }
            
            
            
            
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName) ;
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), CinemaImagesPath, fileName);

                
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Imag.CopyTo(stream);
            }

            cinema.Image = fileName;
        }
        
        cinema.Name = Cnm.Name;
        cinema.Address = Cnm.Address;
        cinema.Country = Cnm.Country;
        cinema.City = Cnm.City;
        cinema.Phone = Cnm.Phone;
        cinema.Email = Cnm.Email;
        // cinema.DateOfBirth = Cnm.DateOfBirth;
        // cinema.Gender = Cnm.Gender;
        
        await _cinemaRepository.UpdateAsync(cinema);
        TempData["Success"] = "Cinema has been updated successfully.";

        return RedirectToAction(nameof(GetById), new { id = cinema.Id });
    }
    
}