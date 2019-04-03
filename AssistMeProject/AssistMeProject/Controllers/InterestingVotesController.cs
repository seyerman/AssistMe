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
    public class InterestingVotesController : Controller
    {
        private readonly AssistMeProjectContext _context;

        public InterestingVotesController(AssistMeProjectContext context)
        {
            _context = context;
        }

        // GET: InterestingVotes
        public async Task<IActionResult> Index()
        {
            return View(await _context.InterestingVote.ToListAsync());
        }

        // GET: InterestingVotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interestingVote = await _context.InterestingVote
                .FirstOrDefaultAsync(m => m.ID == id);
            if (interestingVote == null)
            {
                return NotFound();
            }

            return View(interestingVote);
        }

        // GET: InterestingVotes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InterestingVotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID")] InterestingVote interestingVote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interestingVote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(interestingVote);
        }

        // GET: InterestingVotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interestingVote = await _context.InterestingVote.FindAsync(id);
            if (interestingVote == null)
            {
                return NotFound();
            }
            return View(interestingVote);
        }

        // POST: InterestingVotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID")] InterestingVote interestingVote)
        {
            if (id != interestingVote.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interestingVote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InterestingVoteExists(interestingVote.ID))
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
            return View(interestingVote);
        }

        // GET: InterestingVotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interestingVote = await _context.InterestingVote
                .FirstOrDefaultAsync(m => m.ID == id);
            if (interestingVote == null)
            {
                return NotFound();
            }

            return View(interestingVote);
        }

        // POST: InterestingVotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interestingVote = await _context.InterestingVote.FindAsync(id);
            _context.InterestingVote.Remove(interestingVote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InterestingVoteExists(int id)
        {
            return _context.InterestingVote.Any(e => e.ID == id);
        }
    }
}
