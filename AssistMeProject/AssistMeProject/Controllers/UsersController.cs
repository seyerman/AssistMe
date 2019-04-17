using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace AssistMeProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly AssistMeProjectContext _context;
        public const String ACTIVE_USERNAME = "USERNAME";
        public AssistMe model;

        public UsersController(AssistMeProjectContext context)
        {
            _context = context;
            model = new AssistMe(context);
        }

        // GET: Users
        public IActionResult Index(string message)
        {
            setActiveUser();
            ViewBag.MESSAGE = message;
            return View();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            setActiveUser();
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GOOGLE_KEY,LEVEL,USERNAME,PASSWORD,EMAIL,PHOTO,QUESTIONS_ANSWERED,POSITIVE_VOTES_RECEIVED,QUESTIONS_ASKED,INTERESTING_VOTES_RECEIVED,DESCRIPTION,INTERESTS_OR_KNOWLEDGE,COUNTRY,CITY")] User user)
        {
            setActiveUser();
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Users",new { message = "Congratulations, please login below"});
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit()
        {
            setActiveUser();
            string currentlyActiveUsername = HttpContext.Session.GetString(ACTIVE_USERNAME);

            if (currentlyActiveUsername == null)
            {
                return RedirectToAction("Index", "Users", new { message = "Please, login below" });
            }

            User user = model.GetUser(currentlyActiveUsername);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ID,GOOGLE_KEY,LEVEL,USERNAME,PASSWORD,EMAIL,PHOTO,QUESTIONS_ANSWERED,POSITIVE_VOTES_RECEIVED,QUESTIONS_ASKED,INTERESTING_VOTES_RECEIVED,DESCRIPTION,INTERESTS_OR_KNOWLEDGE,COUNTRY,CITY")] User user)
        {
            setActiveUser();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction("Index", "Users", new { message = user.PASSWORD });
                return RedirectToAction(nameof(Profile));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            setActiveUser();
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            setActiveUser();
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void setActiveUser()
        {
            //To pass the username active
            ViewBag.ACTIVE_USER = HttpContext.Session.GetString(ACTIVE_USERNAME);
            //End To pass the username active
        }

        private bool UserExists(int id)
        {
            setActiveUser();
            return _context.User.Any(e => e.ID == id);
        }

        [HttpGet]
        public IActionResult Profile(string viewingToUser)
        {
            setActiveUser();
            string currentlyActiveUsername = HttpContext.Session.GetString(ACTIVE_USERNAME);

            if (!string.IsNullOrEmpty(currentlyActiveUsername))
                return View(model.GetUser(currentlyActiveUsername));
            if (string.IsNullOrEmpty(viewingToUser))
                return RedirectToAction("Index", "Users", new { message = "Please, login below" });
            return View(model.GetUser(viewingToUser));
        }

        [HttpPost]
        public IActionResult Profile(string username, string password, string method)
        {
            setActiveUser();
            User found = model.FindUser(username,password,method);
            if (found == null)
            {
                return RedirectToAction("Index","Users",new { message = "Error, try again"});
            } else
            {
                //Only username it's saved for have a better security but this might be slower because have to search user every time
                HttpContext.Session.SetString(ACTIVE_USERNAME, found.USERNAME);
                return View(found);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(ACTIVE_USERNAME);
            return RedirectToAction("Index","Users");
        }

    }
}
