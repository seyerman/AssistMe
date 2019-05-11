using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;
using System.Globalization;

namespace AssistMeProject.Controllers
{
    //
    public class AnswersController : Controller
    {
        private readonly AssistMeProjectContext _context;

        public AnswersController(AssistMeProjectContext context)
        {
            _context = context;
        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            var assistMeProjectContext = _context.Answer.Include(a => a.Question);
            return View(await assistMeProjectContext.Include(a => a.PositiveVotes).ToListAsync());
        }

        public async Task<IActionResult> AnswerList(int? QuestionID)
        {
            var asssitMeProjectContext = _context.Answer.Where(a => a.QuestionID == QuestionID).Include(a=> a.Question).Include(a => a.Comments).Include(a=>a.User);
            return PartialView(await asssitMeProjectContext.ToListAsync());

        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Question)
                .Include(a => a.Comments)
                .Include(a => a.PositiveVotes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create(int? QuestionID)
        {

            string Activeuser = HttpContext.Session.GetString("USERNAME");
            if (string.IsNullOrEmpty(Activeuser))
            {
                return RedirectToAction("Index", "Users", new { message = "Please Log In" });
            }
            Question question= _context.Question.Find(QuestionID);
            ViewData["Questioner"] = question.User.USERNAME;
            ViewData["TitleQ"] = question.Title;
            ViewData["DescriptionQ"] = question.Description;

            //int activeUserId = _context.User.First(u => u.USERNAME.Equals(Activeuser)).ID;
            ViewData["UserId"] = question.User.ID;//activeUserId;
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int QuestionID,[Bind("QuestionID,Id,Description,Date")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                string Activeuser = HttpContext.Session.GetString("USERNAME");
                int activeUserId = _context.User.First(u => u.USERNAME.Equals(Activeuser)).ID;
                answer.Date = DateTime.Now;
                answer.UserId = activeUserId;
                _context.Add(answer);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details","Questions",new { id = QuestionID });
            }
            
            return View(answer);
        }

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["QuestionID"] = new SelectList(_context.Question, "Id", "Description", answer.QuestionID);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionID,Id,Description,Date")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
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
            ViewData["QuestionID"] = new SelectList(_context.Question, "Id", "Description", answer.QuestionID);
            return View(answer);
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answer.FindAsync(id);
            _context.Answer.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(int id)
        {
            return _context.Answer.Any(e => e.Id == id);
        }

        // GET: api/Default/5/5
        [HttpGet("api/Answers/Correct/{aid}/{uid}", Name = "MarkAsRead")]
        public async Task<JsonResult> MarkAsRead(int aid, int uid)
        {
            Answer an = _context.Answer.First(a => a.Id == aid);
            string error = "";
            int status = 0;
            if (an != null)
            {
                an.correctAnswer = !an.correctAnswer;
                status = an.correctAnswer ? 1 : -1;
                _context.SaveChanges();
            }
            else {
                error = "La respuesta no existe.";
            }
            var json = new JsonResult(new { status=status, error=error});
            return json;
        }
        // GET: api/Default/5/5
        [HttpGet("api/Answers/{quid}/{uid}", Name = "GetAnswersList")]
        public async Task<JsonResult> GetAnswersList(int quid, int uid)
        {
            var urlParams = Request.Query;
            var answers = _context.Answer
                .Where(a => a.QuestionID == quid)
                .Include(a => a.Comments)
                .ToList().Select(an => {
                var data = new
                {
                    id = an.Id,
                    questionID = an.QuestionID,
                    description = an.Description,
                    date = an.Date,
                    comments = an.Comments.ToList(),
                    userVote = false,//an.HasUserVote
                    votes = 0 // an.Votes.COunt
                };
                data.comments.ForEach(c => c.Answer = null);
                return data;
            });



            string opt = urlParams["since"] + "";
            if (opt != null && !"".Equals(opt))
            {
                var dt = DateTime.Parse(opt);
                answers = answers.Where(a => a.date >= dt);
            }
            opt = urlParams["until"];
            if (opt != null && !"".Equals(opt))
            {
                var dt = DateTime.Parse(opt);
                answers = answers.Where(a => a.date <= dt);
            }
            opt = urlParams["studios"];
            if (opt != null && !"".Equals(opt)) {
                string[] studios = opt.Contains(",")?opt.Split(","):new string[1]{ opt};
                //answers = answers.Where(a => studios.Contains(answers.Autor.Studio));
            }
            opt = urlParams["votes"];
            if (opt != null && !"".Equals(opt))
                if ("any".Equals(opt))
                    answers = answers.Where(a => a.votes > 0);
                else if ("no".Equals(opt))
                    answers = answers.Where(a => a.votes == 0);
                else
                    answers = answers.Where(a => a.votes >= int.Parse(opt));
            

            var json = new JsonResult(answers.ToList());
            return json;
        }
    }
}
