using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Controllers;

public class CategoryController:Controller
{
        private readonly ICategoryRepository _categoryRepository;
    private string CategoryImagesPath =>
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "CategoryImage");

    public  CategoryController(ICategoryRepository CategoryRepository)
    {
        _categoryRepository = CategoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cat = await _categoryRepository.GetAllAsync();
        return View(cat);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var cat = await _categoryRepository.GetCategoryWithMovies(id);
        if (cat == null)
            return NotFound();

        return View(cat);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var category = new Category();
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category,IFormFile Imag)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        if ( Imag.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName) ;
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), CategoryImagesPath, fileName);

                
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Imag.CopyTo(stream);
            }

            category.Image = fileName;
        }
        await _categoryRepository.CreatAsync(category);
        TempData["Success"] = "Category has been added successfully.";

        return RedirectToAction(nameof(GetById), new { id = category.Id });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var category =await _categoryRepository.GetByIdAsync(id);
    
        if (category == null)
        {
            return NotFound();
        }
    
        // مسح الصورة
        if (!string.IsNullOrEmpty(category.Image))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), CategoryImagesPath, category.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        await _categoryRepository.DeleteAsync(id);
        TempData["Success"] = "Category has been deleted successfully.";

        return RedirectToAction(nameof(GetAll));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }
    
    [HttpPost]
    [Authorize]
    public async  Task<IActionResult> Edit(Category cat,IFormFile Imag)
    {
        if (!ModelState.IsValid)
        {
            return View(cat);
        }

        // var dbDoc = _db.Doctors.Find(d.Id);
        var category = await _categoryRepository.GetByIdAsync(cat.Id);
        if (category == null)
        {
            return NotFound();
        }
        if ( Imag is not null && Imag.Length > 0)
        {
            var oldfilePath = Path.Combine(Directory.GetCurrentDirectory(), CategoryImagesPath, category.Image);
            if (System.IO.File.Exists(oldfilePath))
            {
                System.IO.File.Delete(oldfilePath);
            }
            
            
            
            
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName) ;
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), CategoryImagesPath, fileName);

                
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Imag.CopyTo(stream);
            }

            category.Image = fileName;
        }
        
        category.Name = cat.Name;
        category.Description = cat.Description;
       
        
        await _categoryRepository.UpdateAsync(category);
        TempData["Success"] = "Category has been updated successfully.";

        return RedirectToAction(nameof(GetById), new { id = category.Id });
    }
    
    
    
    
    
    
    
    
}