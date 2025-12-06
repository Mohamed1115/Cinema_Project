using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Controllers;

public class HallController:Controller
{
    private readonly IHallRepository _hallRepository;
  

    public HallController(IHallRepository hallRepository)
    {
        _hallRepository = hallRepository;
        
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(int id)
    {
        var hal = await _hallRepository.GetAllc(id);
        return View(hal);
    }

   

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var hall = await _hallRepository.GetHallWithMov(id);
        if (hall == null)
        {
            return NotFound($"Hall with id {id} not found");
        }

        return View(hall);
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Create(int? cinemaId)
    {
        var hall = new Hall();
        if (cinemaId.HasValue)
        {
            hall.CinemaId = cinemaId.Value;
        }
        return View(hall);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Hall hall)
    {
        var hal = await _hallRepository.CreatAsync(hall);
        if (hal == null)
            return View(hall);
        return RedirectToAction("Create", "Hall", new { id = hall.Id });        
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var hall =await _hallRepository.GetByIdAsync(id);
    
        if (hall == null)
        {
            return NotFound();
        } 
        
        await _hallRepository.DeleteAsync(id);
        TempData["Success"] = "Hall has been deleted successfully.";

        return RedirectToAction(nameof(GetAll));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var hall = await _hallRepository.GetByIdAsync(id);
        if (hall == null)
        {
            return NotFound();
        }
        return View(hall);
    }
    
    [HttpPost]
    [Authorize]
    public async  Task<IActionResult> Edit(Hall hal)
    {
        if (!ModelState.IsValid)
        {
            return View(hal);
        }

        
        var hall = await _hallRepository.GetByIdAsync(hal.Id);
        if (hall == null)
        {
            return NotFound();
        }
        
        hall.Name = hal.Name;
        hall.Description = hal.Description;
        hall.Capacity = hal.Capacity;
        hall.Type = hal.Type;
        
        await _hallRepository.UpdateAsync(hall);
        TempData["Success"] = "Hall has been updated successfully.";

        return RedirectToAction(nameof(GetById), new { id = hall.Id });
    }
    
    
    
    
    
    
}