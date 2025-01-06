using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADPC_Project_1.Models;

namespace ADPC_Project_1.Controllers
{
    public class MedicalrecordsController : Controller
    {
        private readonly PostgresContext _context;

        public MedicalrecordsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: Medicalrecords
        public async Task<IActionResult> Index()
        {
            var postgresContext = _context.Medicalrecords.Include(m => m.Patient);
            return View(await postgresContext.ToListAsync());
        }

        // GET: Medicalrecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalrecord = await _context.Medicalrecords
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalrecord == null)
            {
                return NotFound();
            }

            return View(medicalrecord);
        }

        // GET: Medicalrecords/Create
        public IActionResult Create()
        {
            ViewData["Patientid"] = new SelectList(_context.Patients, "Id", "Id");
            return View();
        }

        // POST: Medicalrecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Patientid,Diseasename,Startdate,Enddate")] Medicalrecord medicalrecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalrecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Patientid"] = new SelectList(_context.Patients, "Id", "Id", medicalrecord.Patientid);
            return View(medicalrecord);
        }

        // GET: Medicalrecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalrecord = await _context.Medicalrecords.FindAsync(id);
            if (medicalrecord == null)
            {
                return NotFound();
            }
            ViewData["Patientid"] = new SelectList(_context.Patients, "Id", "Id", medicalrecord.Patientid);
            return View(medicalrecord);
        }

        // POST: Medicalrecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Patientid,Diseasename,Startdate,Enddate")] Medicalrecord medicalrecord)
        {
            if (id != medicalrecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalrecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalrecordExists(medicalrecord.Id))
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
            ViewData["Patientid"] = new SelectList(_context.Patients, "Id", "Id", medicalrecord.Patientid);
            return View(medicalrecord);
        }

        // GET: Medicalrecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalrecord = await _context.Medicalrecords
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalrecord == null)
            {
                return NotFound();
            }

            return View(medicalrecord);
        }

        // POST: Medicalrecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalrecord = await _context.Medicalrecords.FindAsync(id);
            if (medicalrecord != null)
            {
                _context.Medicalrecords.Remove(medicalrecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalrecordExists(int id)
        {
            return _context.Medicalrecords.Any(e => e.Id == id);
        }
    }
}
