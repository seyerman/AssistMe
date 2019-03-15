using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Comment : Element
    {

        public int AnswerId { get; set; }
        public virtual Answer Answer { get; set; }


    }
}
