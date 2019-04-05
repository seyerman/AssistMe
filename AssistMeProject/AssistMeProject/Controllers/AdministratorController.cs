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
    public class AdministratorController : Controller
    {
        private readonly AssistMeProjectContext _context;
        public AssistMe model;
        private BM25Searcher _searcher;

        public AdministratorController(AssistMeProjectContext context)
        {
            _context = context;
            model = new AssistMe(context);
        }

        // GET: Questions
        public async Task<IActionResult> AddAdmin()
        {
            //Example of how to get the actual user that logged into the application
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("USERNAME")))
                actualUser = model.GetUser(HttpContext.Session.GetString("USERNAME"));
            ViewBag.User = actualUser; //You just put at view (in C# code) ViewBag.User and get the user logged
            //End of the example
            var users = (await _context.User.ToListAsync());
          
            return View(users);
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.Include(q => q.Answers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        private void initSearcher()
        {
            _searcher = new BM25Searcher();
            LoadSearcher();
        }

        private void LoadSearcher()
        {
            var questions = _context.Question.ToList();
            foreach (var question in questions)
            {
                _searcher.AddDocument(question);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            initSearcher();
            List<Question> questions = new List<Question>();
            List<ISearchable> searchables = _searcher.Search(query);
            foreach (ISearchable s in searchables)
            {
                questions.Add((Question)s);
            }
            return View(questions);
            //return View(await _context.Question.ToListAsync());
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            return View();
        }

     
        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsArchived,Id,Title,Description,IdUser,Date")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddAdmin));
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IsArchived,Id,Title,Description,IdUser,Date")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AddAdmin));
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddAdmin));
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
