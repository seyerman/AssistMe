using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;
using System.Security.Claims;
using System.Threading;

namespace AssistMeProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly AssistMeProjectContext _context;
        public AssistMe model;

        public UsersController(AssistMeProjectContext context)
        {
            _context = context;
            model = new AssistMe(context);
        }

        //A method that allows charge uder data information for a single use
        public void ChargeUserData()
        {
            var i = ((ClaimsIdentity)Thread.CurrentPrincipal);
            var c = i.Claims.Where(x => x.Type == CustomClaims.ID).FirstOrDefault();
            ViewData["LOCATION"] = i.Claims.FirstOrDefault(a => a.Type==CustomClaims.LOCATION);
            ViewData["LEVEL"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.LEVEL);
            ViewData["PASSWORD"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.PASSWORD);
            ViewData["USERNAME"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.USERNAME);
            ViewData["QUESTIONS_ASKED"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.QUESTIONS_ASKED);
            ViewData["QUESTIONS_ANSWERED"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.QUESTIONS_ANSWERED);
            ViewData["POSITIVE_VOTES_RECEIVED"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.POSITIVE_VOTES_RECEIVED);
            ViewData["PHOTO"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.PHOTO);
            ViewData["ID"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.ID);
            ViewData["DESCRIPTION"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.DESCRIPTION);
            ViewData["INTERESTS_OR_KNOWLEDGE"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.INTERESTS_OR_KNOWLEDGE);
            ViewData["INTERESTING_VOTES_RECEIVED"] = i.Claims.FirstOrDefault(a => a.Type == CustomClaims.INTERESTING_VOTES_RECEIVED);
            ViewData["EMAIL"] = i.Claims.FirstOrDefault(a => a.Type==CustomClaims.EMAIL);
        }

        public void ChargeUserData(User user)
        {
            ViewData["LOCATION"] = user.COUNTRY + "," + user.CITY;
            ViewData["LEVEL"] = user.LEVEL;
            ViewData["PASSWORD"] = user.PASSWORD;
            ViewData["USERNAME"] = user.USERNAME;
            ViewData["QUESTIONS_ASKED"] = user.QUESTIONS_ASKED;
            ViewData["QUESTIONS_ANSWERED"] = user.QUESTIONS_ANSWERED;
            ViewData["POSITIVE_VOTES_RECEIVED"] = user.POSITIVE_VOTES_RECEIVED;
            ViewData["PHOTO"] = user.PHOTO;
            ViewData["ID"] = user.ID;
            ViewData["DESCRIPTION"] = user.DESCRIPTION;
            ViewData["INTERESTS_OR_KNOWLEDGE"] = user.INTERESTS_OR_KNOWLEDGE;
            ViewData["INTERESTING_VOTES_RECEIVED"] = user.INTERESTING_VOTES_RECEIVED;
            ViewData["EMAIL"] = user.EMAIL;
        }

        //A method that charge (at cookies for a time) all needed information of an user if it's password it's correct, otherwise returns null
        public IActionResult Login(string username, string password, string method)
        {
            User found = model.FindUser(username,password,method);
            if (found == null)
                return null;
            ViewData["ALLOWED"] = "TRUE";
            ClaimsIdentity list = new ClaimsIdentity();
            list.AddClaim(new Claim(CustomClaims.LOCATION, found.COUNTRY+","+found.CITY));
            list.AddClaim(new Claim(CustomClaims.LEVEL, found.LEVEL.ToString()));
            list.AddClaim(new Claim(CustomClaims.PASSWORD, found.PASSWORD));
            list.AddClaim(new Claim(CustomClaims.USERNAME, found.USERNAME));
            list.AddClaim(new Claim(CustomClaims.QUESTIONS_ASKED, found.QUESTIONS_ASKED.ToString()));
            list.AddClaim(new Claim(CustomClaims.QUESTIONS_ANSWERED, found.QUESTIONS_ANSWERED.ToString()));
            list.AddClaim(new Claim(CustomClaims.POSITIVE_VOTES_RECEIVED, found.POSITIVE_VOTES_RECEIVED.ToString()));
            list.AddClaim(new Claim(CustomClaims.PHOTO, found.PHOTO));
            list.AddClaim(new Claim(CustomClaims.ID, found.ID.ToString()));
            list.AddClaim(new Claim(CustomClaims.EMAIL, found.EMAIL));
            list.AddClaim(new Claim(CustomClaims.DESCRIPTION, found.DESCRIPTION));
            list.AddClaim(new Claim(CustomClaims.INTERESTS_OR_KNOWLEDGE, found.INTERESTS_OR_KNOWLEDGE));
            list.AddClaim(new Claim(CustomClaims.INTERESTING_VOTES_RECEIVED, found.INTERESTING_VOTES_RECEIVED.ToString()));
            //HttpContext.User.AddIdentity(identity);
            ClaimsIdentity identity = new ClaimsIdentity(list);
            //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false}, identity);
            ChargeUserData(found);
            //Consulting claims
            //var i = ((ClaimsIdentity)Thread.CurrentPrincipal);
            //var c = i.Claims.Where(x => x.Type == CustomClaims.ID).FirstOrDefault();
            //Throwing Excpetion
            //throw new System.Security.Authentication.InvalidCredentialException();
            return View();
        }



        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GOOGLE_KEY,LEVEL,USERNAME,PASSWORD,EMAIL,PHOTO,QUESTIONS_ANSWERED,POSITIVE_VOTES_RECEIVED,QUESTIONS_ASKED,INTERESTING_VOTES_RECEIVED,DESCRIPTION,INTERESTS_OR_KNOWLEDGE,COUNTRY,CITY")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GOOGLE_KEY,LEVEL,USERNAME,PASSWORD,EMAIL,PHOTO,QUESTIONS_ANSWERED,POSITIVE_VOTES_RECEIVED,QUESTIONS_ASKED,INTERESTING_VOTES_RECEIVED,DESCRIPTION,INTERESTS_OR_KNOWLEDGE,COUNTRY,CITY")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

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
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }

        public IActionResult Profile(string username, string password)
        {
            if (username != null && !username.Equals(""))
                ChargeUserData(model.FindUser(username, password, "N"));
            return View();
        }

    }
}
