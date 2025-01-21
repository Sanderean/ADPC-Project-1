using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADPC_Project_1.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace ADPC_Project_1.Controllers
{
    public class CheckupsController : Controller
    {
        private readonly PostgresContext _context;
        private readonly Cloudinary _cloudinary;

        public CheckupsController(PostgresContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        // GET: Checkups
        public async Task<IActionResult> Index()
        {
            var postgresContext = _context.Checkups.Include(c => c.Patient);
            return View(await postgresContext.ToListAsync());
        }

        // GET: Checkups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkup = await _context.Checkups
                .Include(c => c.Medicalfiles) // Включаем связанные фотографии
                .FirstOrDefaultAsync(m => m.Id == id);

            if (checkup == null)
            {
                return NotFound();
            }

            return View(checkup);
        }


        // GET: Checkups/Create
        public IActionResult Create()
        {
            ViewBag.Patientid = new SelectList(_context.Patients, "Id", "Id"); // Assuming the 'Name' is the display name for the patient
            return View();
        }


        // POST: Checkups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Checkup checkup, List<IFormFile> photos)
        {
            if (ModelState.IsValid)
            {
                // Сохранение checkup и файлов в базу данных
                _context.Add(checkup);
                await _context.SaveChangesAsync();

                var uploadedFiles = new List<Medicalfile>();

                foreach (var photo in photos)
                {
                    if (photo.Length > 0)
                    {
                        // Загрузка файла в Cloudinary
                        using var stream = photo.OpenReadStream();
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(photo.FileName, stream),
                            Folder = "checkups" // Укажите папку в Cloudinary
                        };

                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                        if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            // Сохранение информации о загруженном файле в базу данных
                            var medicalFile = new Medicalfile
                            {
                                Filename = uploadResult.PublicId,
                                Filepath = uploadResult.SecureUrl.ToString(),
                                Checkupid = checkup.Id,
                                Uploaddate = DateTime.Now
                            };

                            uploadedFiles.Add(medicalFile);
                        }
                    }
                }

                if (uploadedFiles.Any())
                {
                    _context.Medicalfiles.AddRange(uploadedFiles);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Patientid"] = new SelectList(_context.Patients, "Id", "Personalidentificationnumber", checkup.Patientid);
            return View(checkup);
        }


        // GET: Checkups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkup = await _context.Checkups
                .Include(c => c.Medicalfiles)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (checkup == null)
            {
                return NotFound();
            }

            ViewData["Patientid"] = new SelectList(_context.Patients, "Id", "Personalidentificationnumber", checkup.Patientid);
            return View(checkup);
        }


        // POST: Checkups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Checkup checkup, List<IFormFile> photos)
        {
            if (id != checkup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Обновление записи Checkup
                    _context.Update(checkup);
                    await _context.SaveChangesAsync();

                    if (photos != null && photos.Any()) {
                        // Загрузка новых файлов
                        foreach (var photo in photos)
                        {
                            if (photo.Length > 0)
                            {
                                using var stream = photo.OpenReadStream();
                                var uploadParams = new ImageUploadParams
                                {
                                    File = new FileDescription(photo.FileName, stream),
                                    Folder = "checkups" // Папка в Cloudinary
                                };

                                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var medicalFile = new Medicalfile
                                    {
                                        Filename = photo.FileName,
                                        Filepath = uploadResult.SecureUrl.ToString(),
                                        Checkupid = checkup.Id,
                                        Uploaddate = DateTime.Now
                                    };

                                    _context.Medicalfiles.Add(medicalFile);
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckupExists(checkup.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Patientid"] = new SelectList(_context.Patients, "Id", "Personalidentificationnumber", checkup.Patientid);
            return View(checkup);
        }

        // Удаление файла
        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var file = await _context.Medicalfiles.FindAsync(id);
            if (file != null)
            {
                // Удаление из Cloudinary
                var deleteParams = new DeletionParams(file.Filename); // Убедитесь, что используете правильное имя файла
                await _cloudinary.DestroyAsync(deleteParams);

                // Удаление из базы данных
                _context.Medicalfiles.Remove(file);
                await _context.SaveChangesAsync();
            }

            // Возвращаем успешный ответ
            return Json(new { success = true, fileId = id });
        }




        // GET: Checkups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkup = await _context.Checkups
                .Include(c => c.Patient)
                .Include(c => c.Medicalfiles)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (checkup == null)
            {
                return NotFound();
            }

            return View(checkup);
        }


        // POST: Checkups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var checkup = await _context.Checkups
                .Include(c => c.Medicalfiles)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (checkup == null)
            {
                return NotFound();
            }

            foreach (var file in checkup.Medicalfiles)
            {
                var deleteParams = new DeletionParams(file.Filename);
                await _cloudinary.DestroyAsync(deleteParams);
            }

            _context.Medicalfiles.RemoveRange(checkup.Medicalfiles);

            _context.Checkups.Remove(checkup);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool CheckupExists(int id)
        {
            return _context.Checkups.Any(e => e.Id == id);
        }
    }
}
