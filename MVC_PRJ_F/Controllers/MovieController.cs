using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Controllers
{
    public class MovieController : Controller
    {

        private readonly IMovieRepository _movieRepository;
        private readonly IMovieSubImageRepository _subImageRepository;
        private string MovieImagesPath =>
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "MovieImage");

        public MovieController(IMovieRepository movieRepository, IMovieSubImageRepository subImageRepository)
        {
            _movieRepository = movieRepository;
            _subImageRepository = subImageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mov = await _movieRepository.GetAllAsync();
            return View(mov);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var mov = await _movieRepository.GetMovieWithImage(id);
            if (mov == null)
                return NotFound();
            //
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var movie = new Movie();
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile Imag, List<IFormFile> SubImages)
        {
            if (!ModelState.IsValid)
                return View(movie);

            // 1️⃣ رفع الصورة الأساسية MainImage
            if (Imag != null && Imag.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), MovieImagesPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Imag.CopyToAsync(stream);

                movie.MainImage = fileName;
            }

            // 2️⃣ حفظ الفيلم
            await _movieRepository.CreatAsync(movie);

            // 3️⃣ رفع الصور الفرعية (SubImages)
            if (SubImages != null && SubImages.Count > 0)
            {
                foreach (var img in SubImages)
                {
                    if (img.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), MovieImagesPath, fileName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await img.CopyToAsync(stream);

                        var subImage = new MovImage()
                        {
                            MovieId = movie.Id,
                            ImageName = fileName
                        };

                        await _subImageRepository.CreateAsync(subImage);
                    }
                }
            }

            TempData["Success"] = "Movie has been added successfully.";
            return RedirectToAction(nameof(GetById), new { id = movie.Id });
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            // مسح الصورة
            if (!string.IsNullOrEmpty(movie.MainImage))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), MovieImagesPath, movie.MainImage);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            await _movieRepository.DeleteAsync(id);
            TempData["Success"] = "Movie has been deleted successfully.";

            return RedirectToAction(nameof(GetAll));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Movie mov, IFormFile Imag)
        {
            if (!ModelState.IsValid)
            {
                return View(mov);
            }

            // var dbDoc = _db.Doctors.Find(d.Id);
            var movie = await _movieRepository.GetByIdAsync(mov.Id);
            if (movie == null)
            {
                return NotFound();
            }
            if (Imag is not null && Imag.Length > 0)
            {
                var oldfilePath = Path.Combine(Directory.GetCurrentDirectory(), MovieImagesPath, movie.MainImage);
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }




                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), MovieImagesPath, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Imag.CopyTo(stream);
                }

                movie.MainImage = fileName;
            }

            movie.Description= mov.Description;
            movie.Title = mov.Title;
            movie.DateTime=mov.DateTime;
            // movie.DateOfBirth = mov.DateOfBirth;
            // movie.Gender = mov.Gender;

            await _movieRepository.UpdateAsync(movie);
            TempData["Success"] = "Movie has been updated successfully.";

            return RedirectToAction(nameof(GetById), new { id = movie.Id });
        }
    }
}
