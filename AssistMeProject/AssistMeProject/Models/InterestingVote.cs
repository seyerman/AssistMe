using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class InterestingVote
    {
        public int ID { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }
    }
}
