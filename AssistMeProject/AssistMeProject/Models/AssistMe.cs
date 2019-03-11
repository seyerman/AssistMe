using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class AssistMe
    {

        public const string NORMAL_LOGIN = "N";
        public const string GOOGLE_LOGIN = "G";
        private Dictionary<string, User> listUsers;

        public AssistMe()
        {

            listUsers = new Dictionary<string, User>();

        }

        /*
         * Create a number (pass by parameter) of user for test
         */
        private void createTestUsers(int number)
        {
            for (int i = 1; i <= number; i++)
            {
                string username = "Test" + i;
                string email = i % 2 == 0 ? username + ".gaming@globant.com" : username + ".consulting@globant.com";
                User user = new User(i, email, "../data/photos/" + username, i % 6, i % 10, i % 30, i % 20, "I like to help people", "Play guitar, play piano, food lover", "Colombia", "Medellin");
                listUsers.Add(username, user);
            }
        }

        /*
         * Returns the information found by a username received (if exist) and if it's password it's correct. If it doesn´t exist or
         * password it´s not correct, returns null
         */
        private string[] findUser(string username, string password, string method)
        {
            string[] userFound = null;
            User found = null;
            if (method.Equals(GOOGLE_LOGIN))
            {
                if (listUsers[username] != null)
                    found = listUsers[username];
            } else if (listUsers.Equals(NORMAL_LOGIN))
            {
                if (listUsers[username] != null && listUsers[username].PASSWORD.Equals(password))
                    found = listUsers[username];
            }
            if (found!=null)
            {
                userFound = found.getStringData();
            }
            return userFound;
        }

    }
}