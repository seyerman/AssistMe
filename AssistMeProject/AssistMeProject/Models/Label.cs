using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Label
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public virtual List<QuestionLabel> QuestionLabels { get; set; }
        public int NumberOfTimes { get; set; }
    }
}
