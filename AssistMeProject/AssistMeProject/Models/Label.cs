using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Label
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Tag { get; set; }
        public virtual Question Question { get; set; }
        public int NumberOfTimes { get; set; }
        public Label()
        {

        }
    }
}
