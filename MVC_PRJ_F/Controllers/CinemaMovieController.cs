using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Controllers;

public class CinemaMovieController:Controller
{
    private readonly ICinemaMovieRepository _cinemaMovieRepository;
    public CinemaMovieController(ICinemaMovieRepository cinemaMovieRepository)
    {
        _cinemaMovieRepository = cinemaMovieRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cm = await _cinemaMovieRepository.GetAllAsync();
        return View(cm);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var cm = await _cinemaMovieRepository.GetMovieWithHall(id);
        if (cm == null)
            return NotFound();

        return View(cm);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var cinemaMovie = new CinemaMovies();
        return View(cinemaMovie);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CinemaMovies cinemaMovie,IFormFile Imag)
    {
        if (!ModelState.IsValid)
        {
            return View(cinemaMovie);
        }

        
        await _cinemaMovieRepository.CreatAsync(cinemaMovie);
        TempData["Success"] = "CinemaMovies has been added successfully.";

        return RedirectToAction(nameof(GetById), new { id = cinemaMovie.Id });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var cinemaMovie =await _cinemaMovieRepository.GetByIdAsync(id);
    
        if (cinemaMovie == null)
        {
            return NotFound();
        }
        
        await _cinemaMovieRepository.DeleteAsync(id);
        TempData["Success"] = "CinemaMovies has been deleted successfully.";

        return RedirectToAction(nameof(GetAll));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var cinemaMovie = await _cinemaMovieRepository.GetByIdAsync(id);
        if (cinemaMovie == null)
        {
            return NotFound();
        }
        return View(cinemaMovie);
    }
    
    [HttpPost]
    [Authorize]
    public async  Task<IActionResult> Edit(CinemaMovies cm)
    {
        if (!ModelState.IsValid)
        {
            return View(cm);
        }

        // var dbDoc = _db.Doctors.Find(d.Id);
        var cinemaMovie = await _cinemaMovieRepository.GetByIdAsync(cm.Id);
        if (cinemaMovie == null)
        {
            return NotFound();
        }
        
        
        cinemaMovie.Price = cm.Price;
        cinemaMovie.Time = cm.Time;
        
        
        await _cinemaMovieRepository.UpdateAsync(cinemaMovie);
        TempData["Success"] = "CinemaMovies has been updated successfully.";

        return RedirectToAction(nameof(GetById), new { id = cinemaMovie.Id });
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}