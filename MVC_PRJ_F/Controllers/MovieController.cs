using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Controllers
{
    public class MovieController : Controller
    {

        private readonly IActorRepository _actorRepository;
        private string ActorImagesPath =>
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "ActorImage");

        public ActorController(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Act = await _actorRepository.GetAllAsync();
            return View(Act);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var Act = await _actorRepository.GetActorWithMovies(id);
            if (Act == null)
                return NotFound();

            return View(Act);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var actor = new Actor();
            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Actor actor, IFormFile Imag)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }

            if (Imag.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), ActorImagesPath, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Imag.CopyTo(stream);
                }

                actor.Image = fileName;
            }
            await _actorRepository.CreatAsync(actor);
            TempData["Success"] = "Actor has been added successfully.";

            return RedirectToAction(nameof(GetById), new { id = actor.Id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _actorRepository.GetByIdAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            // مسح الصورة
            if (!string.IsNullOrEmpty(actor.Image))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), ActorImagesPath, actor.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            await _actorRepository.DeleteAsync(id);
            TempData["Success"] = "Actor has been deleted successfully.";

            return RedirectToAction(nameof(GetAll));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _actorRepository.GetByIdAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Actor act, IFormFile Imag)
        {
            if (!ModelState.IsValid)
            {
                return View(act);
            }

            // var dbDoc = _db.Doctors.Find(d.Id);
            var actor = await _actorRepository.GetByIdAsync(act.Id);
            if (actor == null)
            {
                return NotFound();
            }
            if (Imag is not null && Imag.Length > 0)
            {
                var oldfilePath = Path.Combine(Directory.GetCurrentDirectory(), ActorImagesPath, actor.Image);
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }




                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), ActorImagesPath, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Imag.CopyTo(stream);
                }

                actor.Image = fileName;
            }

            actor.Name = act.Name;
            actor.DateOfBirth = act.DateOfBirth;
            actor.Gender = act.Gender;

            await _actorRepository.UpdateAsync(actor);
            TempData["Success"] = "Actor has been updated successfully.";

            return RedirectToAction(nameof(GetById), new { id = actor.Id });
        }
    }
}
