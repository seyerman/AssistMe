using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{






    public class View : IComparable
    {

        public int ID { get; set; }
    
        public virtual User User{get;set;}

        public int UserID { get; set; }
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }
        public DateTime Date { get; set; }


        public int CompareTo(object obj)
        {
            throw new NotImplementedException();

        }
    }
}
