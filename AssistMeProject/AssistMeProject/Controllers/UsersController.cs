using AssistMeProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Text;

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
            string userS = SetActiveUser();
            ViewBag.MESSAGE = message;
            return View();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string userS = SetActiveUser();
            if (string.IsNullOrEmpty(userS))
                return RedirectToAction("", "", new { message = "Error, Inicie sesión" });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int LEVEL, string USERNAME, string PASSWORD, string EMAIL,
            string PHOTO, int QUESTIONS_ANSWERED, int POSITIVE_VOTES_RECEIVED, int QUESTIONS_ASKED, int INTERESTING_VOTES_RECEIVED,
            string DESCRIPTION, string INTERESTS_OR_KNOWLEDGE, string COUNTRY, string CITY, string StudioName)
        {
            User user = new User();
            user.LEVEL = LEVEL;
            user.USERNAME = USERNAME;
            user.PASSWORD = PASSWORD;
            user.EMAIL = EMAIL;
            user.PHOTO = PHOTO;
            user.QUESTIONS_ASKED = QUESTIONS_ASKED;
            user.QUESTIONS_ANSWERED = QUESTIONS_ANSWERED;
            user.POSITIVE_VOTES_RECEIVED = POSITIVE_VOTES_RECEIVED;
            user.INTERESTING_VOTES_RECEIVED = INTERESTING_VOTES_RECEIVED;
            user.DESCRIPTION = DESCRIPTION;
            user.INTERESTS_OR_KNOWLEDGE = INTERESTS_OR_KNOWLEDGE;
            user.COUNTRY = COUNTRY;
            user.CITY = CITY;
            user.StudioId = _context.Studio.FirstOrDefault(a => a.Name.Equals(StudioName)).Id;
            if (ModelState.IsValid)
            {
                bool exist = UserExists(user.USERNAME);
                string message = "";
                if (exist)
                {
                    message = "El usuario ya existe, digite uno nuevo";
                }
                else
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    message = "Cuenta creada, inicie sesión";
                }
                return RedirectToAction("Index", "Users", new { message });
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit()
        {
            string userS = SetActiveUser();
            if (string.IsNullOrEmpty(userS))
                return RedirectToAction("Index", "Users", new { message = "Error, Inicie sesión" });
            string currentlyActiveUsername = HttpContext.Session.GetString(ACTIVE_USERNAME);

            if (currentlyActiveUsername == null)
            {
                return RedirectToAction("Index", "Users", new { message = "Inicie sesión" });
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
        public async Task<IActionResult> Edit([Bind("ID,GOOGLE_KEY,LEVEL,USERNAME,PASSWORD,EMAIL,PHOTO,QUESTIONS_ANSWERED,POSITIVE_VOTES_RECEIVED,QUESTIONS_ASKED,INTERESTING_VOTES_RECEIVED,DESCRIPTION,INTERESTS_OR_KNOWLEDGE,COUNTRY,CITY,StudioId")] User user)
        {
            
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(ACTIVE_USERNAME)))
                return RedirectToAction("Index", "Users", new { message = "Error, Inicie sesión" });
            if (ModelState.IsValid && CheckUsername(user.USERNAME))
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

        private bool CheckUsername(string username)
        {
            return _context.User.Count(p => p.USERNAME.Equals(username)) == 1;
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string userS = SetActiveUser();
            if (string.IsNullOrEmpty(userS))
                return RedirectToAction("", "", new { message = "Error, Inicie sesión" });
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
            string userS = SetActiveUser();
            if (string.IsNullOrEmpty(userS))
                return RedirectToAction("", "", new { message = "Error, Inicie sesión" });
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }

        private bool UserExists(string username)
        {
            return _context.User.Any(e => e.USERNAME.Equals(username));
        }

        [HttpGet]
        public IActionResult Profile(string viewingToUser)
        {
            string userS = SetActiveUser();
            if (string.IsNullOrEmpty(userS))
                return RedirectToAction("Index", "Users", new { message = "Error, Inicie sesión" });
            string currentlyActiveUsername = HttpContext.Session.GetString(ACTIVE_USERNAME);

            if (!string.IsNullOrEmpty(currentlyActiveUsername))
            {
                if (string.IsNullOrEmpty(viewingToUser))
                    return View(model.GetUser(currentlyActiveUsername));
                ViewData["ACTIVE_USER"] = currentlyActiveUsername;
                User found = model.GetUser(viewingToUser);
                found.QUESTIONS_ASKED = _context.Question.Count(a => a.UserId == found.ID);//We profile added this because it wasn't implemented action of increase or decrease when a user makes or answer a question
                found.QUESTIONS_ANSWERED = _context.Answer.Count(a => a.UserId == found.ID);
                return View(found);
            }
            else
            {
                return RedirectToAction("Index", "Users", new { message = "Inicie sesión" });
            }
        }

        [HttpPost]
        public IActionResult Profile(string username, string password, string method)
        {
            User found = model.FindUser(username, password, method);
            found.QUESTIONS_ASKED = _context.Question.Count(a => a.UserId==found.ID); //We profile added this because it wasn't implemented action of increase or decrease when a user makes or answer a question
            found.QUESTIONS_ANSWERED = _context.Answer.Count(a => a.UserId == found.ID);
            if (found == null)
            {
                return RedirectToAction("Index", "Users", new { message = "Error, prueba de nuevo" });
            }
            else
            {
                //Only username it's saved for have a better security but this might be slower because have to search user every time it's needed
                HttpContext.Session.SetString(ACTIVE_USERNAME, found.USERNAME);
                ViewData["ACTIVE_USER"] = username;
                SetActiveUser();
                return View(found);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(ACTIVE_USERNAME);
            return RedirectToAction("Index", "Users");
        }

        public IActionResult AllNotifications()
        {
            string userActive = SetActiveUser();
            if (string.IsNullOrEmpty(userActive))
            {
                return RedirectToAction("Index", "Users", new { message = "Error, Inicie sesión" });
            }
            else
            {
                int id = model.GetUser(userActive).ID;
                var notifications = _context.Notification.Where(p => p.UserID == id).ToList();
                GetNotificationsOfUser();
                return View(notifications);
            }


        }

        private void GetNotificationsOfUser()
        {
            string userActive = HttpContext.Session.GetString(ACTIVE_USERNAME);
            User user = model.GetUser(userActive);
            try
            {
                ViewBag.Notifications = _context.Notification.Where(p => p.UserID == user.ID && !p.Read).ToList();
            } catch (Exception e)
            {

            }
        }

        public IActionResult LostPassword(string email)
        {
            User found = _context.User.FirstOrDefault(a => a.EMAIL.Equals(email));
            if (found==null)
                return RedirectToAction("Index", "Users", new { message = "Errror, no se encontro ningún usuario registrado con el correo: " + email });
            string message = "<br><h2>Tu nombre de usuario es: <span style=\"color:#f40000\"><bold>" + found.USERNAME+ "</bold></span><br>Y tu contraseña es: <span style=\"color:#f40000\"><bold>" + found.PASSWORD+ "</bold></span></h2>" +
                "<br><br>Gracias por formar parte de la familia Globant";
            try
            {
                Email manager = new Email();
                manager.EnviarCorreo(email, "Recuperar contraseña", message,true);
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("Index","Users",new { message = "Se envió su nombre de usuario y contraseña"});
        }

        /**
         * This method allow to set the name of the active user. If there is no user, then pass the Studios that exist for create an account
         **/
        private String SetActiveUser()
        {
            //To pass the username active
            string USER = HttpContext.Session.GetString(ACTIVE_USERNAME);
            if (string.IsNullOrEmpty(USER))
                ViewBag.Studios = AssistMe.GetSelectListStudios(_context);
            else
                GetNotificationsOfUser();
            ViewBag.ACTIVE_USER = USER;
            return USER;
            //End To pass the username active
        }

    }
}
