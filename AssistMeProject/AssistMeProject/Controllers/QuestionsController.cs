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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AssistMeProject.Controllers
{
    //
    public class QuestionsController : Controller
    {

        private const int MAX_RELATED_QUESTIONS = 5;

        private readonly AssistMeProjectContext _context;
        private IHostingEnvironment _hostingEnvironment;
        public AssistMe model;
        private BM25Searcher _searcher;

        public QuestionsController(IHostingEnvironment environment, AssistMeProjectContext context)
        {
            _hostingEnvironment = environment;
            _context = context;
            model = new AssistMe(context);
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {

            //Example of how to get the actual user that logged into the application
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME)))
                actualUser = model.GetUser(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME));
            ViewBag.User = actualUser; //You just put at view (in C# code) ViewBag.User and get the user logged
                                       //End of the example


            var questions = await _context.Question.Where(q => q.isArchived == false)

                    .Include(q => q.Answers)
                    .Include(q => q.InterestingVotes)
                    .Include(q => q.QuestionLabels)
                        .ThenInclude(ql => ql.Label)
                    .Include(q => q.QuestionStudios)
                        .ThenInclude(qs => qs.Studio)
                    .Include(q => q.User)
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
            //Example of how to get the actual user that logged into the application
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME)))
                actualUser = model.GetUser(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME));

            ViewData["actualUserID"] = actualUser.ID;//Si aqui es null, lanza un error al inetntar ver la descripción de una pregunta,se debe controlar este error
            ViewBag.User = actualUser;


            if (actualUser != null)
            {
                ViewData["Admin"] = actualUser.LEVEL;

            }
            else
            {
                ViewData["Admin"] = 4;

            }


            var question = await _context.Question
                .Include(q => q.Answers)
                    .ThenInclude(x => x.PositiveVotes)
                .Include(q => q.InterestingVotes)
                .Include(q => q.Views)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label)
                .Include(q => q.QuestionStudios)
                    .ThenInclude(qs => qs.Studio)
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (question == null)
            {
                return NotFound();
            }




            if (question.Views.All(x => x.UserID != actualUser.ID))
            {
                var view = new View { UserID = actualUser.ID, QuestionID = question.Id };
                _context.View.Add(view);
                _context.SaveChanges();
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

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", question.Id + "");

            List<string> files = new List<string>();

            if (Directory.Exists(filePath))
            {
                string[] rawFiles = Directory.GetFiles(filePath);
                foreach(string rf in rawFiles)
                {
                    files.Add(Path.GetFileName(rf));
                }
            }

            ViewBag.FileNames = files;


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
                .Include(q => q.User)
                .Include(q => q.QuestionStudios)
                    .ThenInclude(qs => qs.Studio)
                .ToList();
            foreach (var question in questions)
            {
                _searcher.AddDocument(question);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (BM25Searcher.IsValidString(query))
            {
                initSearcher();
                List<Question> questions = new List<Question>();
                List<ISearchable> searchables = _searcher.Search(query);
                foreach (ISearchable s in searchables)
                {
                    questions.Add((Question)s);
                }
                return View("Index", questions);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            string Activeuser = HttpContext.Session.GetString("USERNAME");
            if (string.IsNullOrEmpty(Activeuser))
            {
                return RedirectToAction("Index", "Users", new { message = "Please Log In" });
            }

            ViewBag.username = Activeuser;

            List<SelectListItem> list = new List<SelectListItem>();

            //list.Add(new SelectListItem() { Text = "Choose a Studio", Value = "NULL" });

            var studios = _context.Studio.ToList();
            foreach (Studio s in studios)
            {
                list.Add(new SelectListItem() { Text = s.Name, Value = s.Name });
            }


            ViewData["Studios"] = new SelectList(list, "Value", "Text");




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
        public async Task<IActionResult> Create(List<IFormFile> files, string studio, string studio2, string studio3, 
            string question_tags, [Bind("IsArchived,Id,Title,Description,IdUser,Date")] Question question)
        {
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME)))
            {
                actualUser = model.GetUser(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME));
                question.UserId = actualUser.ID;
            }

            if (ModelState.IsValid)
            {
                _context.Add(question);
                if (!string.IsNullOrEmpty(studio))
                {
                    var st = await _context.Studio.FirstOrDefaultAsync(m => m.Name == studio);

                    if (!string.IsNullOrEmpty(question_tags))
                    {
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
                    }

                    var st1 = await _context.Studio.FirstOrDefaultAsync(m => m.Name == studio);
                    var questionStudio = new QuestionStudio
                    {
                        StudioId = st1.Id,
                        QuestionId = question.Id
                    };
                    _context.Add(questionStudio);

                    if(studio2 != studio)
                    {
                        var st2 = await _context.Studio.FirstOrDefaultAsync(m => m.Name == studio2);
                        var questionStudio2 = new QuestionStudio
                        {
                            StudioId = st2.Id,
                            QuestionId = question.Id
                        };
                        _context.Add(questionStudio2);
                    }

                    if (studio3 != studio && studio3 != studio2)
                    {
                        var st3 = await _context.Studio.FirstOrDefaultAsync(m => m.Name == studio3);
                        var questionStudio3 = new QuestionStudio
                        {
                            StudioId = st3.Id,
                            QuestionId = question.Id
                        };
                        _context.Add(questionStudio3);
                    }


                    await _context.SaveChangesAsync();
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", question.Id + "");

                    if (Directory.Exists(filePath))
                    {
                        Directory.Delete(filePath, true);
                    }
                    Directory.CreateDirectory(filePath);

                    foreach (var formFile in files)
                    {
                        filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", question.Id + "",
                                       Path.GetFileName(formFile.FileName));
                        if (formFile.Length > 0)
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                        }
                    }
                    SendEmailStudio(question, st);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }


        public void SendEmailStudio(Question question, Studio studio)
        {
            var users = _context.User.Where(p => p.StudioId == studio.Id);

            foreach (var user in users)
            {
                SendEmail(question, user.EMAIL);
            }

        }

        public void SendEmail(Question question, string name)
        {

            try
            {
                Email manager = new Email();
                string mail = "Tienes una nueva pregunta \n " + AssistMe.DOMINIO + "/Questions/Details/" + question.Id;
                manager.EnviarCorreo(name, question.Title, mail);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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


        // GET: Questions/Delete/5
        public async Task<IActionResult> Archive(int? id)
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

        // GET: Questions/Delete/5
        public async Task<IActionResult> Desarchivar(int? id)
        {
            var question = await _context.Question.FindAsync(id);
            question.isArchived = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ArchiveQuestion(int? id)
        {
            var question = await _context.Question.FindAsync(id);
            question.isArchived = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Questions/Details/5
        public async Task<IActionResult> OLDARCHIVE(int? id)
        {

            var question = await _context.Question
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label)
                .Include(q => q.User)
                .Include(q => q.QuestionStudios)
                    .ThenInclude(qs => qs.Studio)
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


        // GET: Questions/Details/5
        public async Task<IActionResult> ArchivedQuestionDetails(int? id)
        {



            if (id == null)
            {
                return NotFound();
            }
            //Example of how to get the actual user that logged into the application
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME)))
                actualUser = model.GetUser(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME));

            ViewData["actualUserID"] = actualUser.ID;//Si aqui es null, lanza un error al inetntar ver la descripción de una pregunta,se debe controlar este error

            if (actualUser != null)
            {
                ViewData["Admin"] = actualUser.LEVEL;

            }
            else
            {
                ViewData["Admin"] = 4;

            }


            var question = await _context.Question
                .Include(q => q.Answers)
                    .ThenInclude(x => x.PositiveVotes)
                .Include(q => q.InterestingVotes)
                .Include(q => q.Views)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label)
                .Include(q => q.QuestionStudios)
                    .ThenInclude(qs => qs.Studio)
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (question == null)
            {
                return NotFound();
            }




            if (question.Views.All(x => x.UserID != actualUser.ID))
            {
                var view = new View { UserID = actualUser.ID, QuestionID = question.Id };
                _context.View.Add(view);
                _context.SaveChanges();
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

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", question.Id + "");

            List<string> files = new List<string>();

            if (Directory.Exists(filePath))
            {
                string[] rawFiles = Directory.GetFiles(filePath);
                foreach (string rf in rawFiles)
                {
                    files.Add(Path.GetFileName(rf));
                }
            }

            ViewBag.FileNames = files;


            return View(question);
        }

    }
}
