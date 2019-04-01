using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssistMeProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AssistMeProject.Controllers
{
    public class AdminController : Controller
    {

        private readonly AssistMeProjectContext _context;
        public AssistMe model;

        public AdminController(AssistMeProjectContext context)
        {
            _context = context;
            model = new AssistMe(context);
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        // GET: /<controller>/
        public IActionResult CreateStudio()
        {
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

        // GET: /<controller>/
        public IActionResult AddAdminList()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult UsersList()
        {
            return View();
        }

        // POST: Cotizacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> addAdmin(User user, Admin admin)
        {
            String msg = admin.addAdmin(user.ID);
            //no se que hace
            return View(user);
        }


        //POST: Admin/CreateStudio
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudio([Bind("Id, Name, Unit, Description")] Studio studio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}

  


