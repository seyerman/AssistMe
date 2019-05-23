using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Notification
    {

        public int Id { get; set; }
        public String Description { get; set; }
        public bool Read { get; set; }
        public int  UserID { get; set; }
        public int QuestionId { get; set; }
        public DateTime TimeAnswer { get; set; }




    }
}
