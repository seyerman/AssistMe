using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class AssistMe
    {
        public const string DOMINIO = "http://localhost:50144";

        private readonly AssistMeProjectContext _context;
        
        public const string NORMAL_LOGIN = "N";
        public const string GOOGLE_LOGIN = "G";

        public AssistMe(AssistMeProjectContext _context)
        {
            this._context = _context;
        }

        /*
         * Create a number (pass by parameter) of user for test
         */
        public void CreateTestUsers(int number)
        {
            for (int i = 1; i <= number; i++)
            {
                string username = "Test" + i;
                string email = i % 2 == 0 ? username + ".gaming@globant.com" : username + ".consulting@globant.com";
                User user = new User(i, "",User.LEVEL_NORMAL, username, username, email, "../data/photos/" + username, i % 6, i % 10, i % 30, i % 20, "I like to help people", "Play guitar, play piano, food lover", "Colombia", "Medellin",1);
                _context.User.Add(user);
            }
            _context.SaveChanges();
        }

        /*
         * Returns the information found by a username received (if exist) and if it's password it's correct. If it doesn´t exist or
         * password it´s not correct, returns null
         */
        public User FindUser(string username, string password, string method)
        {
            User found = null;

            if (method.Equals("N"))
                method = NORMAL_LOGIN;
            else if (method.Equals("G"))
                method = GOOGLE_LOGIN;
            else
                return null;

            if (method.Equals(GOOGLE_LOGIN))
            {
                found = _context.User.FirstOrDefault(a => a.GOOGLE_KEY.Equals(username));
            }
            else if (method.Equals(NORMAL_LOGIN))
            {
                found = _context.User.FirstOrDefault(a => a.USERNAME.Equals(username));
                if (found != null && !found.PASSWORD.Equals(password))
                    found = null;
            }

            return found;
        }

        public User GetUser(string username)
        {
            User found = (User)_context.User.FirstOrDefault(a => a.USERNAME.Equals(username)).Clone();
            return found;
        }

        public static SelectList GetSelectListStudios(AssistMeProjectContext _context)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var studios = _context.Studio.ToList();
            foreach (Studio s in studios)
            {
                list.Add(new SelectListItem() { Text = s.Name, Value = s.Name });
            }
            return new SelectList(list, "Value", "Text");
        }

    }
}