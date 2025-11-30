using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Models;
using MVC_PRJ_F.IRepositories;

namespace MVC_PRJ_F.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMovieRepository _movieRepository;
    private readonly ICinemaRepository _cinemaRepository;
    private readonly ICinemaMovieRepository _cinemaMovieRepository;

    public HomeController(
        ILogger<HomeController> logger,
        IMovieRepository movieRepository,
        ICinemaRepository cinemaRepository,
        ICinemaMovieRepository cinemaMovieRepository)
    {
        _logger = logger;
        _movieRepository = movieRepository;
        _cinemaRepository = cinemaRepository;
        _cinemaMovieRepository = cinemaMovieRepository;
    }

    public async Task<IActionResult> Index()
    {
        // Get featured movies (active movies, ordered by date)
        var allMovies = await _movieRepository.GetAllAsync();
        var featuredMovies = allMovies
            .Where(m => m.Status)
            .OrderByDescending(m => m.DateTime)
            .Take(4)
            .ToList();

        // Get coming soon movies (future dates or not active)
        var comingSoonMovies = allMovies
            .Where(m => !m.Status || m.DateTime > DateTime.Now)
            .OrderBy(m => m.DateTime)
            .Take(2)
            .ToList();

        // Get featured movie for hero section (first featured movie)
        var featuredMovie = featuredMovies.FirstOrDefault();

        // Get featured cinemas
        var allCinemas = await _cinemaRepository.GetAllAsync();
        var featuredCinemas = allCinemas.Take(3).ToList();

        // Calculate stats
        var totalMovies = allMovies.Count;
        var activeCinemas = allCinemas.Count;
        var todayShows = await _cinemaMovieRepository.GetAllAsync();
        var todayShowsCount = todayShows.Count;

        ViewBag.FeaturedMovies = featuredMovies;
        ViewBag.ComingSoonMovies = comingSoonMovies;
        ViewBag.FeaturedCinemas = featuredCinemas;
        ViewBag.FeaturedMovie = featuredMovie;
        ViewBag.TotalMovies = totalMovies;
        ViewBag.ActiveCinemas = activeCinemas;
        ViewBag.TodayShows = todayShowsCount;
        ViewBag.TicketsSold = 1200; // Placeholder - can be calculated from bookings if available

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}