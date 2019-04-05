    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using AssistMeProject.Models;
using Microsoft.AspNetCore.Http;

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
            public async Task<IActionResult> IndexAsync()
            {
                var users = (await _context.User.ToListAsync());

                  return View(users);
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
            public async Task<IActionResult> AddAdminList()
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

            // GET: /<controller>/
            public IActionResult UsersList()
            {
                return View();
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
                    return RedirectToAction(nameof(IndexAsync));
                }
                return View(_context);
            }
        }
    }

  


