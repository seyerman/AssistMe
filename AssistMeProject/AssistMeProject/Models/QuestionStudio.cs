using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class QuestionStudio
    {
        public int QuestionId { get; set; }
        public int StudioId { get; set; }
        public Question Question { get; set; }
        public Studio Studio { get; set; }
    }
}
