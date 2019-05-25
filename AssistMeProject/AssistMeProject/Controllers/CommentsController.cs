using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;

namespace AssistMeProject.Controllers
{
    public class CommentsController : Controller
    {
        private readonly AssistMeProjectContext _context;

        public CommentsController(AssistMeProjectContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            SetActiveUser();
            var assistMeProjectContext = _context.Comment.Include(c => c.Answer);
            return View(await assistMeProjectContext.ToListAsync());
        }

        public async Task<IActionResult> CommentList(int? AnswerId)
        {
            SetActiveUser();
            var assistMeProjectContext = _context.Comment.Where(c => c.AnswerId == AnswerId).Include(s=>s.Answer).Include(c=>c.User);
            return PartialView(await assistMeProjectContext.ToListAsync());
        }
        public IActionResult lista(int? AnswerId)
        {
            SetActiveUser();
            var assistMeProjectContext = _context.Comment.Where(c => c.AnswerId == AnswerId).Include(s => s.Answer).Include(c => c.User);
            return PartialView( assistMeProjectContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            SetActiveUser();
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Answer ).Include(c=>c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }
       
        // GET: Comments/Create
        public IActionResult Create(int? AnswerId)
        {
            SetActiveUser();
            string Activeuser = HttpContext.Session.GetString("USERNAME");
            if (string.IsNullOrEmpty(Activeuser))
            {
                return RedirectToAction("Index", "Users", new { message = "Please Log In" });
            }

            
           // ViewData["UserId"] = Int32.Parse(Activeuser);
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int AnswerId,[Bind("AnswerId,Id,Description,Date")] Comment comment)
        {
            SetActiveUser();
            if (ModelState.IsValid)
            {
                string Activeuser = HttpContext.Session.GetString("USERNAME");
                int activeUserId = _context.User.First(u => u.USERNAME.Equals(Activeuser)).ID;
                comment.UserId = activeUserId;
                comment.Date = DateTime.Now;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                var QuestionID = _context.Answer.Find(AnswerId).QuestionID;
                return RedirectToAction("Details", "Questions", new { id = QuestionID });
            }
           // ViewData["AnswerId"] = new SelectList(_context.Answer, "Id", "Description", comment.AnswerId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            SetActiveUser();
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["AnswerId"] = new SelectList(_context.Answer, "Id", "Description", comment.AnswerId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnswerId,Id,Description,Date")] Comment comment)
        {
            SetActiveUser();
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["AnswerId"] = new SelectList(_context.Answer, "Id", "Description", comment.AnswerId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            SetActiveUser();
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Answer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SetActiveUser();
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }

        /**
         * This method allow to set the name of the active user. If there is no user, then pass the Studios that exist for create an account
         **/
        private void SetActiveUser()
        {
            //To pass the username active
            string USER = HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME);
            if (string.IsNullOrEmpty(USER))
                ViewBag.Studios = AssistMe.GetSelectListStudios(_context);
            ViewBag.ACTIVE_USER = USER;
            //End To pass the username active
        }

    }
}
