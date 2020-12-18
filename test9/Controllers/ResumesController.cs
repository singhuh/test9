using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test9.Data;
using test9.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace test9.Controllers
{
    [Authorize(Roles ="User, Admin")]
    public class ResumesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ResumesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Resumes
        public async Task<IActionResult> Index()
        {
            var LoginUserId = _userManager.GetUserId(User);
            if (LoginUserId != null)
            {
                var myResumes = _context.Resumes.Where(m => m.UserId == LoginUserId);
                return View(await myResumes.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            //return View(await _context.Resume.ToListAsync());
        }

        // GET: Resumes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .FirstOrDefaultAsync(m => m.ResumeID == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // GET: Resumes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Resumes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Resume resume, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return View(resume);
            }

            var ext = Path.GetExtension(file.FileName);
            var newFileName = "resumes" + DateTime.Now.ToString("yymm") + ext;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resumes", newFileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            resume.ResumeFile = "/Resumes/" + newFileName;
            _context.Add(resume);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Resumes");
            //if (ModelState.IsValid)
            //{
            //    _context.Add(resume);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(resume);
        }

        // GET: Resumes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null)
            {
                return NotFound();
            }
            return View(resume);
        }

        // POST: Resumes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ResumeID,UserId,Description,ResumeFile")] Resume resume, IFormFile file)
        {
            if (id != resume.ResumeID)
            {
                return NotFound();
            }

            var currentresume = await _context.Resumes.FindAsync(id);
            if (file == null || file.Length == 0)
            {
                currentresume.ResumeID = id;
                currentresume.Description = resume.Description;
                currentresume.UserId = resume.UserId;
                currentresume.ResumeFile = currentresume.ResumeFile;

                _context.Update(currentresume);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Resumes");
            }
            else
            {
                var ext = Path.GetExtension(file.FileName);
                var newFileName = "resumes-modified" + DateTime.Now.ToString("yymm") + ext;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resumes", newFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var RelativePath = currentresume.ResumeFile;
                var AbsolutePath = "wwwroot" + RelativePath;

                if (System.IO.File.Exists(AbsolutePath))
                {
                    System.IO.File.Delete(AbsolutePath);
                }

                currentresume.ResumeID = id;
                currentresume.Description = resume.Description;
                currentresume.UserId = resume.UserId;
                currentresume.ResumeFile = "/Resumes/" + newFileName;

                _context.Update(currentresume);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Resumes");
            }

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(resume);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ResumeExists(resume.ResumeID))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(resume);
        }

        // GET: Resumes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .FirstOrDefaultAsync(m => m.ResumeID == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // POST: Resumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var resume = await _context.Resumes.FindAsync(id);


            var RelativePath = resume.ResumeFile;
            var AbsolutePath = "wwwroot" + RelativePath;

            if (System.IO.File.Exists(AbsolutePath))
            {
                System.IO.File.Delete(AbsolutePath);
            }

            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResumeExists(long id)
        {
            return _context.Resumes.Any(e => e.ResumeID == id);
        }
    }
}
