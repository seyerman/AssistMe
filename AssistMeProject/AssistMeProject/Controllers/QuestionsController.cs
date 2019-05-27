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
using System.Text;

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
            initSearcher();
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            SetActiveUser();
            //Example of how to get the actual user that logged into the application
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME)))
                actualUser = model.GetUser(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME));
            ViewBag.User = actualUser; //You just put at view (in C# code) ViewBag.User and get the user logged
                                       //End of the example


            var questions = await _context.Question.Where(q => q.isArchived == false)

                    .Include(q => q.Answers)
                    .Include(q => q.InterestingVotes)
                    .Include(q => q.Views)
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
        public async Task<IActionResult> Details(int? id, int NotificationID = -1)
        {
            SetActiveUser();
            if (NotificationID!=-1)
            {
                Notification notif = _context.Notification.FirstOrDefault(a => a.Id == NotificationID);
                notif.Read = true;
                _context.Update(notif);
                _context.SaveChanges();
            }
            if (id == null)
            {
                return NotFound();
            }
            //Example of how to get the actual user that logged into the application
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME)))
            {
                actualUser = model.GetUser(HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME));


               ViewData["actualUserID"] = actualUser.ID;//Si aqui es null, lanza un error al inetntar ver la descripción de una pregunta,se debe controlar este error
               ViewBag.User = actualUser;
            }
            else
            {
                return RedirectToAction(nameof(Index), "Users", new { message = "Inice sesión" });
            }


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

            //Empieza busqueda de preguntas relacionadas
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
            //Termina busqueda de preguntas relacionadas 


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

        private void initSearcher()
        {
            if(_searcher == null)
            {
                _searcher = new BM25Searcher();
            }
            
            LoadSearcher();
        }

        private void LoadSearcher()
        {
            var questions = _context.Question
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label)
                .Include(q => q.User)
                .Include(q => q.Views)
                .Include(q => q.QuestionStudios)
                    .ThenInclude(qs => qs.Studio)
                        .ThenInclude(s => s.QuestionStudios)
                .ToList();
            foreach (var question in questions)
            {
                _searcher.AddDocument(question);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            SetActiveUser();
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
            string Activeuser = SetActiveUser();
            if (string.IsNullOrEmpty(Activeuser))
            {
                return RedirectToAction("Index", "Users", new { message = "Inicie sesión" });
            }
			if (TempData.ContainsKey("suggestLb") && TempData.ContainsKey("suggestSt") && TempData.ContainsKey("question"))
			{
				ViewData["suggestLb"] = TempData["suggestLb"] as String[];
				ViewData["suggestSt"] = TempData["suggestSt"] as String[];
				ViewData["question"] = TempData["question"] as String[];
				string[] q = TempData["question"] as String[];
				ViewBag.Related = RelatedQuestions(q[0], q[1]);
				
				TempData.Remove("suggestLb");
				TempData.Remove("suggestSt");
				TempData.Remove("question");
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
            SetActiveUser();
            string Activeuser = HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME);

            ViewBag.username = Activeuser;

            List<SelectListItem> list = new List<SelectListItem>();

            //list.Add(new SelectListItem() { Text = "Choose a Studio", Value = "NULL" });

            var studios = _context.Studio.ToList();
            list.Add(new SelectListItem() { Text = "", Value = "" });
            foreach (Studio s in studios)
            {
                list.Add(new SelectListItem() { Text = s.Name, Value = s.Name });
            }
            ViewData["Studios"] = new SelectList(list, "Value", "Text");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdvancedSearch(string query, string studio, string studio2, string studio3, string glober,
            string question_tags)
        {
            SetActiveUser();
            List<Question> questions = new List<Question>();

            if (BM25Searcher.IsValidString(query))
            {
                initSearcher();
                LoadSearcher();

                List<ISearchable> searchables = _searcher.Search(query);
                foreach (ISearchable s in searchables)
                {
                    questions.Add((Question)s);
                }

            }
            else
            {
                questions = await _context.Question.Where(q => q.isArchived == false)
                    .Include(q => q.Answers)
                    .Include(q => q.InterestingVotes)
                    .Include(q => q.Views)
                    .Include(q => q.QuestionLabels)
                        .ThenInclude(ql => ql.Label)
                    .Include(q => q.QuestionStudios)
                        .ThenInclude(qs => qs.Studio)
                    .Include(q => q.User)
                    .ToListAsync();
            }
            questions.RemoveAll(q => !q.IsUser(glober));
            if (!string.IsNullOrEmpty(question_tags))
            {
                string[] tagsStr = question_tags.Split(",");
                foreach (string t in tagsStr)
                {
                    questions.RemoveAll(q => !q.HasTag(t));
                }
            }
            questions.RemoveAll(q => !q.HasStudio(studio));
            questions.RemoveAll(q => !q.HasStudio(studio2));
            questions.RemoveAll(q => !q.HasStudio(studio3));
            return View("Index", questions);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string action,List<IFormFile> files, string studio, string studio2, string studio3,
            string question_tags, [Bind("IsArchived,Id,Title,Description,IdUser,Date,question_tags")] Question question)
        {
            string user = SetActiveUser();
            if (action == "Suggestions")
			{
				Suggestion(question.Title, question.Description);
				String[] q = { question.Title, question.Description };
				TempData["question"] = q;
			}
			else if (action == "Ask now")
			{
                List<Studio> studios = new List<Studio>();
				User actualUser = null;
				if (!string.IsNullOrEmpty(user))
				{
					actualUser = model.GetUser(user);
                    actualUser.QUESTIONS_ASKED++;
                    question.UserId = actualUser.ID;
				} else
                {
                    return RedirectToAction("Index","Users", new { message = "Inicie sesión"});
                }

                if (ModelState.IsValid)
				{
					_context.Add(question);
					if (!string.IsNullOrEmpty(studio))
					{
						//var st = await _context.Studio.FirstOrDefaultAsync(m => m.Name == studio);

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
                        studios.Add(st1);
						_context.Add(questionStudio);

						if (studio2 != studio)
						{
							var st2 = await _context.Studio.FirstOrDefaultAsync(m => m.Name == studio2);
							var questionStudio2 = new QuestionStudio
							{
								StudioId = st2.Id,
								QuestionId = question.Id
							};
                            studios.Add(st2);
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
                            studios.Add(st3);
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
                        SendEmailStudio(question, studios);
					}

					return RedirectToAction(nameof(Index));
				}
			}
            return RedirectToAction(nameof(Create));
        }


        public void SendEmailStudio(Question question, List<Studio> studios)
        {
            foreach (var st in studios)
            {
                SendEmail(question, st.Email);
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

        public List<Question> RelatedQuestions(string Title, string Description)
        {
            var relatedQuestions = new List<Question>();
            string query = Title + " " + Description;
            if (BM25Searcher.IsValidString(query))
            {
                initSearcher();
                LoadSearcher();
                List<Question> questions = new List<Question>();
                List<ISearchable> searchables = _searcher.Search(query);
                foreach (ISearchable s in searchables)
                {
                    questions.Add((Question)s);
                }
                
                foreach (ISearchable s in searchables)
                {
                    Question q = (Question)s;
                    relatedQuestions.Add(q);
                    if (relatedQuestions.Count == MAX_RELATED_QUESTIONS) break;
                }

            }
			return relatedQuestions;
        }



        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            SetActiveUser();
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
            SetActiveUser();
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
            SetActiveUser();
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
        public async Task<IActionResult> DeleteConfirmed(int id, string message)
        {
            SetActiveUser();

            int questionOwner = _context.Question.Find(id).UserId.Value;// averiguo el dueño de la pregunta referencir a quien podra ver la notificacion
            string questionDescp = _context.Question.Find(id).Title;// averiguo el dueño de la pregunta referencir a quien podra ver la notificacion



            Notification notification = new Notification
            {
                Read = false,
                UserID = questionOwner,
                Description = "Razón de eliminar pregunta: "+  message,
                Title = "Pregunta: '" + questionDescp + "' ha sido eliminada "
            };
            _context.Add(notification);
            await _context.SaveChangesAsync();


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
            SetActiveUser();
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
            SetActiveUser();
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
            SetActiveUser();
            var question = await _context.Question.FindAsync(id);
            question.isArchived = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ArchiveQuestion(int? id)
        {
            SetActiveUser();
            var question = await _context.Question.FindAsync(id);
            question.isArchived = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Questions/Details/5
        public async Task<IActionResult> OLDARCHIVE(int? id)
        {
            SetActiveUser();
            var question = await _context.Question
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label)
                .Include(q => q.User)
                .Include(q => q.Views)
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


            SetActiveUser();
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public void Suggestion(String title, String description)
		{
			String query = title + " " + description;
			RelatedQuestions(title, description);
			string[] lb = SuggestLabels(query);
			string[] st = SuggestStudios(query);
			TempData["suggestLb"] = lb;
			TempData["suggestSt"] = st;

		}

		public string[] SuggestLabels(string query)
		{

			string[] suggestion = new string[5];
			if (BM25Searcher.IsValidString(query))
			{
				initSearcher();
				List<Question> questions = new List<Question>();
				List<ISearchable> searchables = _searcher.Search(query);
				foreach (ISearchable s in searchables)
				{
					questions.Add((Question)s);
				}
				var relatedQuestions = new List<Question>();
				foreach (ISearchable s in searchables)
				{
					Question q = (Question)s;
					relatedQuestions.Add(q);
					if (relatedQuestions.Count == MAX_RELATED_QUESTIONS) break;
				}
				suggestion = SuggestedLabels(relatedQuestions);

			}
			return suggestion;
		}

		private string[] SuggestedLabels(List<Question> suggestions)
		{
			var totalLabels = new SortedSet<Label>();

			for (int i = 0; i < suggestions.Count; i++)
			{
				Question q = suggestions.ElementAt(i);
				for (int j = 0; j < q.QuestionLabels.Count; j++)
				{
					totalLabels.Add(q.QuestionLabels.ElementAt(j).Label);
				}

			}
            var l = totalLabels;
            
			var labels = new string[5];
			for (int i = 0; i < l.Count() && i < 5; i++)
			{
				if (l.ElementAt(i) != null)
				{
					labels[i] = l.ElementAt(i).Tag;
					//labels.Add(l.ElementAt(i).label.Key.Tag);
				}
				else
				{
					labels[i] = null;
				}
			}
			return labels;
		}
		public string[] SuggestStudios(String query)
		{
			string[] suggestion = new string[3];
			if (BM25Searcher.IsValidString(query))
			{
				initSearcher();
				List<Question> questions = new List<Question>();
				List<ISearchable> searchables = _searcher.Search(query);
				foreach (ISearchable s in searchables)
				{
					questions.Add((Question)s);
				}
				var relatedQuestions = new List<Question>();
				foreach (ISearchable s in searchables)
				{
					Question q = (Question)s;
					relatedQuestions.Add(q);
					if (relatedQuestions.Count == MAX_RELATED_QUESTIONS) break;
				}
				suggestion = SuggestedStudios(relatedQuestions);

			}
			return suggestion;
		}

		public string[] SuggestedStudios(List<Question> sug)
		{
			var totalStudios = new SortedSet<Studio>();

			for (int i = 0; i < sug.Count; i++)
			{
				Question q = sug.ElementAt(i);
				for (int j = 0; j < q.QuestionStudios.Count; j++)
				{
					totalStudios.Add(q.QuestionStudios.ElementAt(j).Studio);
				}

			}
			var s = totalStudios;
			var stud = new string[3];
			for (int i = 0; i < s.Count() && i < 3; i++)
			{
				if (s.ElementAt(i) != null) {
				stud[i] = s.ElementAt(i).Name;
				}else
				{
					stud[i] = null;
				}
			}
			return stud;
		}

        private void GetNotificationsOfUser()
        {
            string userActive = HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME);
            User user = model.GetUser(userActive);
            try
            {
                ViewBag.Notifications = _context.Notification.Where(p => p.UserID == user.ID && !p.Read).ToList();
            }
            catch (Exception e)
            {

            }
        }

        /**
         * This method allow to set the name of the active user. If there is no user, then pass the Studios that exist for create an account
         **/
        private String SetActiveUser()
        {
            //To pass the username active
            string USER = HttpContext.Session.GetString(UsersController.ACTIVE_USERNAME);
            if (string.IsNullOrEmpty(USER))
                ViewBag.Studios = AssistMe.GetSelectListStudios(_context);
            else
                GetNotificationsOfUser();
            ViewBag.ACTIVE_USER = USER;
            return USER;
            //End To pass the username active
        }

        public string SuggestStudiosString(string title, string details)
        {
            var studios = SuggestStudios(title + " " + details);
            StringBuilder joined = new StringBuilder();
            foreach (string s in studios)
            {
                if(!String.IsNullOrWhiteSpace(s))
                    joined = joined.Length > 0 ? joined.Append(", ").Append(s) : joined.Append(s);
            }
            return joined.ToString();
        }

        public string SuggestTagsString(string title, string details)
        {
            var studios = SuggestLabels(title + " " + details);
            StringBuilder joined = new StringBuilder();
            foreach (string s in studios)
            {
                if (!String.IsNullOrWhiteSpace(s))
                    joined = joined.Length > 0 ? joined.Append(", ").Append(s) : joined.Append(s);
            }
            return joined.ToString();
        }

        public async Task<IActionResult> RelatedQuestionsView(string title, string details)
        {
            SetActiveUser();
            var relatedQuestions = RelatedQuestions(title, details);
            return PartialView(relatedQuestions);

        }
    }
}
