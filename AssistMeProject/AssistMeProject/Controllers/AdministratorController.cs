using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http.Internal;

namespace AssistMeProject.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly AssistMeProjectContext _context;
        public AssistMe model;
        private IHostingEnvironment _hostingEnvironment;

        public AdministratorController(IHostingEnvironment environment, AssistMeProjectContext context)
        {
            _context = context;
            model = new AssistMe(context);
            _hostingEnvironment = environment;

        }

        public IActionResult Index()
        {

            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("USERNAME")))
                actualUser = model.GetUser(HttpContext.Session.GetString("USERNAME"));

            if (actualUser != null)
            {
                ViewData["Admin"] = actualUser.LEVEL;
            }
            else
            {
                ViewData["Admin"] = 4;

            }

            return View();
        }


        public IActionResult ManageStudios()
        {

            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("USERNAME")))
                actualUser = model.GetUser(HttpContext.Session.GetString("USERNAME"));

            if (actualUser != null)
            {
                ViewData["Admin"] = actualUser.LEVEL;
            }
            else
            {
                ViewData["Admin"] = 4;

            }
            var studios = _context.Studio.ToList();




            return View(studios);
        }

        public IActionResult NotAdmin(User user)
        {
            return View(user);
        }

        public async Task<IActionResult> IndexAsync()
        {
            var users = (await _context.User.ToListAsync());

            return View(users);
        }

        //POST: Admin/CreateStudio
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudio(IFormFile file, [Bind("Id, Name, Unit, Description, Email")] Studio studio)
        {

            if (ModelState.IsValid)
            {
                _context.Add(studio);
                await _context.SaveChangesAsync();
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", studio.Name + "");

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                Directory.CreateDirectory(filePath);


                filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", studio.Name + "",
                               Path.GetFileName(file.FileName));
                if (file.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }


            }


            return RedirectToAction(nameof(ManageStudios));
        }


        // GET: Questions/Details/5
        public async Task<IActionResult> AsignRole(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            user.LEVEL = 2;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return View(user);
        }

        // GET: /<controller>/
        public IActionResult CreateStudio()
        {

            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("USERNAME")))
                actualUser = model.GetUser(HttpContext.Session.GetString("USERNAME"));

            if (actualUser != null)
            {
                ViewData["Admin"] = actualUser.LEVEL;
            }
            else
            {
                ViewData["Admin"] = 4;

            }

            //creamos una lista tipo SelectListItem
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Indique la unidad a la que pertenece el Studio", Value = "NULL" });

            list.Add(new SelectListItem() { Text = "Strategy", Value = "Strategy" });
            list.Add(new SelectListItem() { Text = "Specialty", Value = "Specialty" });
            list.Add(new SelectListItem() { Text = "Foundation", Value = "Foundation" });

            //Agregamos la lista a nuestro SelectList
            ViewData["Unit"] = new SelectList(list, "Value", "Text");
            return View();
        }

        // GET: Questions
        public async Task<IActionResult> AddAdmin()
        {

            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("USERNAME")))
                actualUser = model.GetUser(HttpContext.Session.GetString("USERNAME"));

            if (actualUser != null)
            {
                ViewData["Admin"] = actualUser.LEVEL;
            }
            else
            {
                ViewData["Admin"] = 4;

            }

            var users = (await _context.User.Where(u => u.LEVEL == 3).ToListAsync());
            //users.Sort();
            return View(users);
        }


        // GET: Questions
        public async Task<IActionResult> ArchivedQuestions()
        {
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("USERNAME")))
                actualUser = model.GetUser(HttpContext.Session.GetString("USERNAME"));

            if (actualUser != null)
            {
                ViewData["Admin"] = actualUser.LEVEL;
            }
            else
            {
                ViewData["Admin"] = 4;

            }

            var questions = await _context.Question.Where(q => q.isArchived == true)
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label)
                .ToListAsync();
            questions.Sort();



            return View(questions);

        }





        public async Task<IActionResult> ShowSummary()
        {
            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("USERNAME")))
                actualUser = model.GetUser(HttpContext.Session.GetString("USERNAME"));

            if (actualUser != null)
            {
                ViewData["Admin"] = actualUser.LEVEL;
            }
            else
            {
                ViewData["Admin"] = 4;

            }

            var questions = await _context.Question
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label).Include(q => q.QuestionStudios).ThenInclude(qs => qs.Studio)
                .ToListAsync();


            return View(questions);

        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> DeleteStudio(int? id)
        {
            var studio = await _context.Studio.FindAsync(id);
            _context.Studio.Remove(studio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageStudios));
        }



        // GET: Questions/Create
        public IActionResult Show()
        {

            User actualUser = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("USERNAME")))
                actualUser = model.GetUser(HttpContext.Session.GetString("USERNAME"));

            if (actualUser != null)
            {
                ViewData["Admin"] = actualUser.LEVEL;
            }
            else
            {
                ViewData["Admin"] = 4;

            }

            List<SelectListItem> listStudios = new List<SelectListItem>();

            var studios = _context.Studio.ToList();
            foreach (Studio s in studios)
            {
                listStudios.Add(new SelectListItem() { Text = s.Name, Value = s.Name });
            }


            ViewData["Studios"] = new SelectList(listStudios, "Value", "Text");

            List<SelectListItem> listUsers = new List<SelectListItem>();

            var users = _context.User.ToList();
            foreach (User u in users)
            {
                listUsers.Add(new SelectListItem() { Text = u.USERNAME, Value = u.USERNAME });
            }
            ViewData["Users"] = new SelectList(listUsers, "Value", "Text");



            List<SelectListItem> listLabels = new List<SelectListItem>();

            var labels = _context.Label.ToList();
            foreach (Label l in labels)
            {
                listLabels.Add(new SelectListItem() { Text = l.Tag, Value = l.Tag });
            }


            ViewData["Labels"] = new SelectList(listLabels, "Value", "Text");

            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowStudioTable(string studio)
        {

            ViewData["Studio"] = studio;
            var questions = await _context.Question.Where(q => q.QuestionStudios.Any(i => i.Studio.Name == studio)).Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label).Include(q => q.QuestionStudios).ThenInclude(ql => ql.Studio)
                .ToListAsync();



            return View(questions);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowTagTable(string tag)
        {

            ViewData["Tag"] = tag;
            var questions = await _context.Question.Where(q => q.QuestionLabels.Any(i => i.Label.Tag == tag)).Include(q => q.Answers)
                .Include(q => q.QuestionStudios)
                    .ThenInclude(ql => ql.Studio).Include(q => q.User)
                .ToListAsync();



            return View(questions);
        }


        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowUserTable(string user)
        {

            ViewData["Username"] = user;

            var questions = await _context.Question.Where(u => u.User.USERNAME == user)
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label).Include(q => q.QuestionStudios).ThenInclude(qs => qs.Studio)
                .ToListAsync();

            return View(questions);
        }


        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowTable()
        {
            var questions = await _context.Question
                .Include(q => q.Answers)
                .Include(q => q.QuestionLabels)
                    .ThenInclude(ql => ql.Label).Include(q => q.QuestionStudios).ThenInclude(qs => qs.Studio).Include(q => q.User)
                .ToListAsync();



            return View(questions);
        }


        // GET: Questions/Details/5
        public async Task<IActionResult> StudioDetails(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }


            var studio = await _context.Studio.FirstOrDefaultAsync(m => m.Id == id);


            if (studio == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", studio.Name + "");

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


           


            return View(studio);
        }


    }


}