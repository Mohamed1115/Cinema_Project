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
    public async Task<IActionResult> GetAll()
    {
        var Cnm = await _cinemaRepository.GetAllAsync();
        return View(Cnm);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var Cnm = await _cinemaRepository.GetCinemaWithHallsAndMovies(id);
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
    public async Task<IActionResult> Create(Cinema cinema,IFormFile Imag)
    {
        if (!ModelState.IsValid)
        {
            return View(cinema);
        }

        if ( Imag.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName) ;
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), CinemaImagesPath, fileName);

                
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Imag.CopyTo(stream);
            }

            cinema.Image = fileName;
        }
        await _cinemaRepository.CreatAsync(cinema);
        TempData["Success"] = "Cinema has been added successfully.";

        return RedirectToAction(nameof(GetById), new { id = cinema.Id });
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
        // cinema.DateOfBirth = Cnm.DateOfBirth;
        // cinema.Gender = Cnm.Gender;
        
        await _cinemaRepository.UpdateAsync(cinema);
        TempData["Success"] = "Cinema has been updated successfully.";

        return RedirectToAction(nameof(GetById), new { id = cinema.Id });
    }
    
}