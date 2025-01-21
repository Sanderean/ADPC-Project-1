using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADPC_Project_1.Models;
using System.Text;

namespace ADPC_Project_1.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PostgresContext _context;

        public PatientsController(PostgresContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ExportToCSV()
        {
            var patients = await _context.Patients.ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("Id,Personal ID,Name,Surname,Date of Birth,Sex");

            foreach (var patient in patients)
            {
                csv.AppendLine($"{patient.Id},{patient.Personalidentificationnumber},{patient.Name},{patient.Surname},{patient.Dateofbirth},{patient.Sex}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "Patients.csv");
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Personalidentificationnumber,Name,Surname,Dateofbirth,Sex")] Patient patient)
        {
            var existingPatient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Personalidentificationnumber == patient.Personalidentificationnumber);

            if (existingPatient != null)
            {
                ModelState.AddModelError("Personalidentificationnumber", "A patient with this Personal ID already exists.");
                return View(patient);
            }

            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Personalidentificationnumber,Name,Surname,Dateofbirth,Sex")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            var existingPatient = await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Personalidentificationnumber == patient.Personalidentificationnumber);

            if (existingPatient != null && existingPatient.Id != patient.Id)
            {
                ModelState.AddModelError("Personalidentificationnumber", "A patient with this Personal ID already exists.");
                return View(patient);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
