using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;
using Microsoft.AspNetCore.Http;

namespace AssistMeProject.Controllers
{
    public class ViewsController : Controller
    {
        private readonly AssistMeProjectContext _context;

        public ViewsController(AssistMeProjectContext context)
        {
            _context = context;
        }

        // GET: Views
        public async Task<IActionResult> Index()
        {
            var assistMeProjectContext = _context.View.Include(v => v.Question).Include(v => v.User);
            return View(await assistMeProjectContext.ToListAsync());
        }

        // GET: Views/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var view = await _context.View
                .Include(v => v.Question)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (view == null)
            {
                return NotFound();
            }

            return View(view);
        }

        // GET: Views/Create
        public IActionResult Create()
        {
            ViewData["QuestionID"] = new SelectList(_context.Question, "Id", "Description");
            ViewData["UserID"] = new SelectList(_context.User, "ID", "ID");
            return View();
        }

        // POST: Views/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        public async Task<int> Create([Bind("ID,UserID,QuestionID")] View view)
        {
            if (ModelState.IsValid)
            {
                var lista = _context.View.Where(x => x.UserID == view.UserID && x.QuestionID == view.QuestionID).ToList();
                if (lista.Count() > 0)
                {
                    _context.View.Remove(lista.First());
                    await _context.SaveChangesAsync();
                    return -1;
                }
                else
                {
                    _context.Add(view);
                    await _context.SaveChangesAsync();
                    return 1;
                }
            }

            
            ViewData["QuestionID"] = new SelectList(_context.Question, "Id", "Description", view.QuestionID);
            ViewData["UserID"] = new SelectList(_context.User, "ID", "ID", view.UserID);
            return 0;
        }

        // GET: Views/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var view = await _context.View.FindAsync(id);
            if (view == null)
            {
                return NotFound();
            }
            ViewData["QuestionID"] = new SelectList(_context.Question, "Id", "Description", view.QuestionID);
            ViewData["UserID"] = new SelectList(_context.User, "ID", "ID", view.UserID);
            return View(view);
        }

        // POST: Views/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,QuestionID")] View view)
        {
            if (id != view.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(view);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViewExists(view.ID))
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
            ViewData["QuestionID"] = new SelectList(_context.Question, "Id", "Description", view.QuestionID);
            ViewData["UserID"] = new SelectList(_context.User, "ID", "ID", view.UserID);
            return View(view);
        }

        // GET: Views/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var view = await _context.View
                .Include(v => v.Question)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (view == null)
            {
                return NotFound();
            }

            return View(view);
        }

        // POST: Views/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var view = await _context.View.FindAsync(id);
            _context.View.Remove(view);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViewExists(int id)
        {
            return _context.View.Any(e => e.ID == id);
        }
    }
}
