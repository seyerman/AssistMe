using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class AssistMe
    {

        private Dictionary<string, User> listUsers;

        public AssistMe()
        {

            listUsers = new Dictionary<string, User>();

        }

        private void createTestUsers(int number)
        {
            for (int i = 1; i <= number; i++)
            {
                string username = "Test"+i;
                string email = i%2==0 ? username+".gaming@globant.com": username + ".consulting@globant.com";
                User user = new User(i, email, "../data/photos/" + username, i%6,i%10,i%30,i%20,"I like to help people","Play guitar, play piano, food lover","Colombia","Medellin");
                listUsers.Add(username,user);
            }
        }

    }
}
