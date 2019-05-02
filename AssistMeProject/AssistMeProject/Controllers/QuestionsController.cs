using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;
using System.Net.Mail;
using System.Globalization;

namespace AssistMeProject.Controllers
{
    public class QuestionsController : Controller
    {

        private const int MAX_RELATED_QUESTIONS = 5;

        private readonly AssistMeProjectContext _context;
        public AssistMe model;
        private BM25Searcher _searcher;

        public QuestionsController(AssistMeProjectContext context)
        {
            _context = context;
            model = new AssistMe(context);
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {

            //Example of how to get the actual user that logged into the application
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("USERNAME")))
                actualUser = model.GetUser(HttpContext.Session.GetString("USERNAME"));
            ViewBag.User = actualUser; //You just put at view (in C# code) ViewBag.User and get the user logged
            //End of the example
            var questions = await _context.Question
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label)
                .ToListAsync();
            questions.Sort();



            return View(questions);

        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            initSearcher();

            var relatedQuestions = new List<Question>();
            List<ISearchable> searchables = _searcher.Search(question.Title);

            foreach (ISearchable s in searchables)
            {
                Question q = (Question)s;
                if (q.Id != question.Id)
                    relatedQuestions.Add(q);
                if (relatedQuestions.Count == MAX_RELATED_QUESTIONS) break;
            }


            ViewBag.Related = relatedQuestions;
            return View(question);
        }

        private void initSearcher()
        {
            _searcher = new BM25Searcher();
            LoadSearcher();
        }

        private void LoadSearcher()
        {
            var questions = _context.Question
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label)
                .ToList();
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
            return View("Index", questions);
            //return View(await _context.Question.ToListAsync());
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            string Activeuser = HttpContext.Session.GetString("USERNAME");
            if (string.IsNullOrEmpty(Activeuser))
            {
                return RedirectToAction("Index", "Users", new { message = "Please Log In"} );
            }

            ViewBag.username = Activeuser;


            return View();
        }

        public IActionResult AdvancedSearch()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string question_tags, [Bind("IsArchived,Id,Title,Description,IdUser,Date")] Question question)
        {
            question.Username = HttpContext.Session.GetString("USERNAME");
            if (ModelState.IsValid)
            {
                _context.Add(question);
                string[] tagsStr = question_tags.Split(",");
                foreach (string t in tagsStr)
                {
                    var tag = await _context.Label.FirstOrDefaultAsync(m => m.Tag == t);
                    if (tag == null)
                    {
                        tag = new Label();
                        tag.Tag = t;
                        _context.Add(tag);
                    }
                    tag.NumberOfTimes++;
                    var questionLabel = new QuestionLabel
                    {
                        LabelId = tag.Id,
                        QuestionId = question.Id
                    };
                    _context.Add(questionLabel);
                }
                await _context.SaveChangesAsync();

              

                try
                {
                    Email manager = new Email();
                    MailMessage email = new MailMessage("proyectofinalinge@gmail.com", "proyectofinalinge@gmail.com", question.Title, "Tienes una nueva pregunta \n"+question.Description);
                    manager.EnviarCorreo(email);
                
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.Id == id);
        }

		public async Task<IActionResult> UpdateDate(int? id)
		{
			var question = await _context.Question.FindAsync(id);
			if (question == null)
			{
				return NotFound();
			}
			question.Date = DateTime.Now;
            question.AskAgain = true;
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
			}
			return RedirectToAction(nameof(Details), new { id = question.Id });
		}


        [HttpGet("api/Questions/{uid}", Name = "GetQuestionsList")]
        public async Task<JsonResult> GetQuestionsList(int uid) {
            var urlParams = Request.Query;
            var questions = _context.Question
                .Include(a => a.Answers)
                .ToList().Select(qt => {
                    var data = new
                    {
                        id = qt.Id,
                        title = qt.Title,
                        description = qt.Description,
                        date = qt.Date,
                        answersCount = qt.Answers.Count(),
                        userVote = false,//an.HasUserVote
                        votes = 0 // an.Votes.COunt
                    };
                    return data;
                });

            string opt = urlParams["since"]+"";
            if (opt != null && !"".Equals(opt))
            {
                var dt = DateTime.Parse(opt);
                questions = questions.Where(a => a.date >= dt);
            }
            opt = urlParams["until"];
            if (opt != null && !"".Equals(opt))
            {
                var dt = DateTime.Parse(opt);
                questions = questions.Where(a => a.date <= dt);
            }
            opt = urlParams["studios"];
            if (opt != null && !"".Equals(opt))
            {
                string[] studios = opt.Contains(",") ? opt.Split(",") : new string[1] { opt };
                //questions = questions.Where(q => q.studios.Any(st=> studios.Contains(st)));
            }
            opt = urlParams["votes"];
            if (opt != null && !"".Equals(opt))
                if ("any".Equals(opt))
                    questions = questions.Where(a => a.votes > 0);
                else if ("no".Equals(opt))
                    questions = questions.Where(a => a.votes == 0);
                else
                    questions = questions.Where(a => a.votes >= int.Parse(opt));
            /*
            opt = urlParams["autor"];
            if (opt != null && !"".Equals(opt))
                questions = questions.Where(a => opt.Equals(a.autor));
            */

            var json = new JsonResult(questions.ToList());
            return json;
        }

    }
}
