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
    
    public class AnswersController : Controller
    {
        private readonly AssistMeProjectContext _context;
        public AssistMe model;

        public AnswersController(AssistMeProjectContext context)
        {
            _context = context;
        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            SetActiveUser();
            var assistMeProjectContext = _context.Answer.Include(a => a.Question);
            return View(await assistMeProjectContext.Include(a => a.PositiveVotes).ToListAsync());
        }


        public async Task<IActionResult> AnswerList(int? QuestionID, int? userId)
        {
            SetActiveUser();
            ViewData["userID"] = userId;
            var asssitMeProjectContext = _context.Answer.Where(a => a.QuestionID == QuestionID).Include(a=> a.Question).Include(a=>a.PositiveVotes).Include(a => a.Comments).Include(a=>a.User);
            return PartialView(await asssitMeProjectContext.ToListAsync());

        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            SetActiveUser();
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
        public IActionResult Create(int? QuestionID, Boolean repeat)
        {
            SetActiveUser();
            string Activeuser = HttpContext.Session.GetString("USERNAME");
            if (string.IsNullOrEmpty(Activeuser))
            {
                return RedirectToAction("Index", "Users", new { message = "Please Log In" });
            }
            Question question = _context.Question.Include(q => q.User).First(q => q.Id == QuestionID);

            ViewData["Questioner"] = question.User.USERNAME;
            ViewData["TitleQ"] = question.Title;
            ViewData["DescriptionQ"] = question.Description;

            //int activeUserId = _context.User.First(u => u.USERNAME.Equals(Activeuser)).ID;
            ViewData["UserId"] = question.User.ID;//activeUserId;
            ViewData["IsRepeat"] = repeat;
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int QuestionID,string UrlOriginalQuestion,[Bind("QuestionID,Id,Description,Date,UrlOriginalQuestion")] Answer answer)
        {
            SetActiveUser();
            if (ModelState.IsValid)
            {

                string Activeuser = HttpContext.Session.GetString("USERNAME");
                int activeUserId = _context.User.First(u => u.USERNAME.Equals(Activeuser)).ID;
                answer.Date = DateTime.Now;
                answer.UserId = activeUserId;
                _context.Add(answer);

                if (UrlOriginalQuestion!=null)
                {
                    _context.Question.Find(QuestionID).Insignia = "SEEN BEFORE";
                }

                int questionOwner = _context.Question.Find(QuestionID).UserId.Value;// averiguo el dueño de la pregunta referencir a quien podra ver la notificacion
                
                Notification notification = new Notification {  Read = false,UserID= questionOwner,
                TimeAnswer=answer.Date,QuestionId=QuestionID,
                    Title = Activeuser +" Respondio tu pregunta "
                };
                _context.Add(notification);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details","Questions",new { id = QuestionID });
            }
            
            return View(answer);
        }

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            SetActiveUser();
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
            SetActiveUser();
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
            SetActiveUser();
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
            SetActiveUser();
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
            SetActiveUser();
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
            SetActiveUser();
            var urlParams = Request.Query;
            var ans = await _context.Answer
                .Where(a => a.QuestionID == quid)
                .Include(a => a.Comments)
                .Include(a => a.User)
                .ThenInclude(u => u.Studio)
                .ToListAsync();

                var answers=ans.Select(an =>
                {
                    var data = new
                    {
                        id = an.Id,
                        questionID = an.QuestionID,
                        description = an.Description,
                        date = an.Date,
                        comments = an.Comments.ToList(),
                        userVote =  an.UserVote(uid),
                        votes = an.PositiveVotes.Count(),
                        an.correctAnswer,
                        autor = new { name = an.User.USERNAME, img = (an.User.PHOTO != null) ? an.User.PHOTO : "http://placehold.it/60x60/FFF/444", studio = an.User.Studio.Name }
                    };
                    data.comments.ForEach(c => {
                        c.Answer = null;
                        c.User.Comments = null;
                        c.User.Answers = null;
                        c.User.PHOTO = (c.User.PHOTO != null) ? c.User.PHOTO : "http://placehold.it/60x60/FFF/444";
                        c.User.Studio.Users=null;
                    });
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
            if (opt != null && !"".Equals(opt))
            {
                string[] studios = opt.Contains(",") ? opt.Split(",") : new string[1] { opt };
                answers = answers.Where(a => studios.Contains(a.autor.studio));
            }
            opt = urlParams["votes"];
            if (opt != null && !"".Equals(opt))
            {
                if ("any".Equals(opt))
                    answers = answers.Where(a => a.votes > 0);
                else if ("no".Equals(opt))
                    answers = answers.Where(a => a.votes == 0);
                else
                    answers = answers.Where(a => a.votes >= int.Parse(opt));
            }
            opt = urlParams["reply"];
            if (opt != null && !"".Equals(opt))
            {
                if ("any".Equals(opt))
                    answers = answers.Where(a => a.comments.Count() > 0);
                else if ("no".Equals(opt))
                    answers = answers.Where(a => a.comments.Count() == 0);
                else
                    answers = answers.Where(a => a.comments.Count() >= int.Parse(opt));
            }

            var json = new JsonResult(answers.ToList());
            return json;
        }

        [HttpGet("api/Questions/{uid}", Name = "GetQuestionsList")]
        public async Task<JsonResult> GetQuestionsList(int uid)
        {
            SetActiveUser();
            var urlParams = Request.Query;
            var ans = await _context.Question
                .Include(a => a.Answers)
                .Include(a => a.User)
                .ThenInclude(u => u.Studio)
                .Include(q=> q.QuestionStudios)
                .Include(q => q.QuestionLabels)
                .ToListAsync();

            var questions = ans.Select(qu =>
            {
                var studios = qu.QuestionStudios.Select(st=>new {name=st.Studio.Name, id=st.StudioId });

                var data = new
                {
                    id = qu.Id,
                    title=qu.Title,
                    description = qu.Description,
                    date = qu.Date,
                    answers = qu.Answers.Count(),
                    userVote = qu.UserVote(uid),
                    votes = qu.InterestingVotes.Count(),
                    solved = qu.Answers.Any(a => a.correctAnswer),
                    studios,
                    askAgain=qu.AskAgain,
                    labels=qu.QuestionLabels.Select(ql=>ql.Label).Where(l=>l!=null),
                    autor = new { name = qu.User.USERNAME, img = (qu.User.PHOTO != null) ? qu.User.PHOTO : "http://placehold.it/60x60/FFF/444", studio = qu.User.Studio.Name }
                };
                return data;
            });
            string opt = urlParams["since"] + "";
            if (opt != null && !"".Equals(opt))
            {
                var dt = DateTime.Parse(opt);
                questions = questions.Where(a => a.date >= dt);
            }
            opt = urlParams["until"];
            if (opt != null && !"".Equals(opt))
            {
                var dt = DateTime.Parse(opt);
                questions  = questions.Where(a => a.date <= dt);
            }
            opt = urlParams["studios"];
            if (opt != null && !"".Equals(opt))
            {
                string[] studios = opt.Contains(",") ? opt.Split(",") : new string[1] { opt };
                questions  = questions.Where(a => a.studios.Any(stu=> studios.Contains(stu.name)));
            }
            opt = urlParams["votes"];
            if (opt != null && !"".Equals(opt))
            {
                if ("any".Equals(opt))
                    questions = questions.Where(a => a.votes > 0);
                else if ("no".Equals(opt))
                    questions = questions.Where(a => a.votes == 0);
                else
                    questions = questions.Where(a => a.votes >= int.Parse(opt));
            }
            opt = urlParams["reply"];
            if (opt != null && !"".Equals(opt))
            {
                if ("any".Equals(opt))
                    questions = questions.Where(a => a.answers > 0);
                else if ("no".Equals(opt))
                    questions = questions.Where(a => a.answers == 0);
                else
                    questions = questions.Where(a => a.answers >= int.Parse(opt));
            }

            var json = new JsonResult(questions.ToList());
            return json;
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
