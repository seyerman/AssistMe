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
    //
    public class PositiveVController : Controller
    {
        private readonly AssistMeProjectContext _context;


    
        public PositiveVController(AssistMeProjectContext context)
        {
            _context = context;
        }

        // GET: PositiveV
        public async Task<IActionResult> Index()
        {
            var assistMeProjectContext = _context.PositiveVote.Include(i => i.Answer).Include(i => i.User);
            return View(await assistMeProjectContext.ToListAsync());
        }

        // GET: PositiveV/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var positiveVote = await _context.PositiveVote
                .Include(i => i.Answer)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (positiveVote == null)
            {
                return NotFound();
            }

            return View(positiveVote);
        }



        // POST: PositiveV/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        public async Task<int> Create([Bind("ID,UserID,AnswerID")] PositiveVote positiveVote)
        {
            if (ModelState.IsValid)
            {
                var lista = _context.PositiveVote.Where(x => x.UserID == positiveVote.UserID && x.AnswerID == positiveVote.AnswerID).ToList();
                if (lista.Count() > 0)
                {
                    _context.PositiveVote.Remove(lista.First());
                    await _context.SaveChangesAsync();
                    return -1;
                }
                else
                {
                    _context.Add(positiveVote);
                    await _context.SaveChangesAsync();
                    return 1;
                }
                
            }



            return 0;
            
        }

        // GET: PositiveV/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var positiveVote = await _context.PositiveVote.FindAsync(id);
            if (positiveVote == null)
            {
                return NotFound();
            }

            ViewData["AnswerID"] = new SelectList(_context.Answer, "Id", "Description", positiveVote.AnswerID);
            ViewData["UserID"] = new SelectList(_context.User, "ID", "ID", positiveVote.UserID);

            return View(positiveVote);
        }

        // POST: PositiveV/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,AnswerID")] PositiveVote positiveVote)
        {
            if (id != positiveVote.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(positiveVote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositiveVoteExists(positiveVote.ID))
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

            ViewData["AnswerID"] = new SelectList(_context.Answer, "Id", "Description", positiveVote.AnswerID);
            ViewData["UserID"] = new SelectList(_context.User, "ID", "ID", positiveVote.UserID);


            return View(positiveVote);
        }

        // GET: PositiveV/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var positiveVote = await _context.PositiveVote
                .Include(i => i.Answer)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (positiveVote == null)
            {
                return NotFound();
            }

            return View(positiveVote);
        }

        // POST: PositiveV/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var positiveVote = await _context.PositiveVote.FindAsync(id);
            _context.PositiveVote.Remove(positiveVote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PositiveVoteExists(int id)
        {
            return _context.PositiveVote.Any(e => e.ID == id);
        }
    }
}
