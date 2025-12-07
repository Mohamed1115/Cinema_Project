using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Controllers;

public class CinemaMovieController:Controller
{
    private readonly ICinemaMovieRepository _cinemaMovieRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IHallRepository _hallRepository;
    
    public CinemaMovieController(ICinemaMovieRepository cinemaMovieRepository, IMovieRepository movieRepository, IHallRepository hallRepository)
    {
        _cinemaMovieRepository = cinemaMovieRepository;
        _movieRepository = movieRepository;
        _hallRepository = hallRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cm = await _cinemaMovieRepository.GetAllAsync();
        return View(cm);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllByMovieId(int id)
    {
        var cm = await _cinemaMovieRepository.GetAllByMovieId(id);
        
        // Ensure we have movie details for the header, even if no showtimes exist
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie == null) return NotFound();
        
        ViewBag.Movie = movie;
        
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
    public async Task<IActionResult> Create(int? hallId, int? cinemaId)
    {
        var cinemaMovie = new CinemaMovies();
        
        if (hallId.HasValue)
        {
            cinemaMovie.HallId = hallId.Value;
            
        }
        
        if (cinemaId.HasValue)
        {
            cinemaMovie.CinemaId = cinemaId.Value;
        }
        
        // Get all movies for dropdown
        var movies = await _movieRepository.GetAllAsync();
        ViewBag.Movies = movies;
        
        return View(cinemaMovie);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CinemaMovies cinemaMovie)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                // جمع كل الأخطاء من ModelState
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                TempData["Error"] = string.Join("<br>", errors);
                
                // إعادة تحميل قائمة الأفلام
                var movies = await _movieRepository.GetAllAsync();
                ViewBag.Movies = movies;
                
                return View(cinemaMovie);
            }
            
            // Get Hall Capacity for available seats
            var hall = await _hallRepository.GetByIdAsync(cinemaMovie.HallId);
            if (hall != null)
            {
                cinemaMovie.Quantity = hall.Capacity;
            }
            else
            {
                TempData["Error"] = "Selected hall does not exist.";
                var movies = await _movieRepository.GetAllAsync();
                ViewBag.Movies = movies;
                return View(cinemaMovie);
            }

            // التحقق من وجود الفيلم (optional - can be removed if not needed)
            // var movie = await _movieRepository.GetByIdAsync(cinemaMovie.MovieId);
            // if (movie == null)
            // {
            //     TempData["Error"] = "Selected movie does not exist.";
            //     var movies = await _movieRepository.GetAllAsync();
            //     ViewBag.Movies = movies;
            //     return View(cinemaMovie);
            // }

            // حفظ CinemaMovie
            await _cinemaMovieRepository.CreatAsync(cinemaMovie);
            
            TempData["Success"] = "Movie show has been added successfully.";
            return RedirectToAction("GetById", "Cinema", new { id = cinemaMovie.CinemaId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"An error occurred while creating the movie show: {ex.Message}";
            
            // إعادة تحميل قائمة الأفلام
            var movies = await _movieRepository.GetAllAsync();
            ViewBag.Movies = movies;
            
            return View(cinemaMovie);
        }
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
    
    // Mm@123456
    
    
    
    
    
    
    
    
    
    
    
    
    
}