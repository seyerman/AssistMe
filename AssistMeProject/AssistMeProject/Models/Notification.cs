using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Notification
    {

        public int Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public bool Read { get; set; }
        public int  UserID { get; set; }//dueño de la notificación, quien la ve
      //  public string UserActorName { get; set; } // quien disparó la notificación 
        public int QuestionId { get; set; }
        public DateTime TimeAnswer { get; set; }




    }
}
