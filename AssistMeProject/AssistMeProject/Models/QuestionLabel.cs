using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class QuestionLabel
    {
        public int QuestionId { get; set; }
        public int LabelId { get; set; }
        public Question Question { get; set; }
        public Label Label { get; set; }

    }
}
