using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;

namespace AssistMeProject.Controllers
{
    public class PositiveVotesController : Controller
    {
        private readonly AssistMeProjectContext _context;

        public PositiveVotesController(AssistMeProjectContext context)
        {
            _context = context;
        }

        // GET: PositiveVotes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PositiveVote.ToListAsync());
        }

        // GET: PositiveVotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var positiveVote = await _context.PositiveVote
                .FirstOrDefaultAsync(m => m.ID == id);
            if (positiveVote == null)
            {
                return NotFound();
            }

            return View(positiveVote);
        }

        // GET: PositiveVotes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PositiveVotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID")] PositiveVote positiveVote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(positiveVote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(positiveVote);
        }

        // GET: PositiveVotes/Edit/5
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
            return View(positiveVote);
        }

        // POST: PositiveVotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID")] PositiveVote positiveVote)
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
            return View(positiveVote);
        }

        // GET: PositiveVotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var positiveVote = await _context.PositiveVote
                .FirstOrDefaultAsync(m => m.ID == id);
            if (positiveVote == null)
            {
                return NotFound();
            }

            return View(positiveVote);
        }

        // POST: PositiveVotes/Delete/5
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
