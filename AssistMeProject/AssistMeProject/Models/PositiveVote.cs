using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class PositiveVote
    {

        public int ID { get; set; }
        public User User { get; set; }
        public Answer Answer { get; set; }

    }
}
