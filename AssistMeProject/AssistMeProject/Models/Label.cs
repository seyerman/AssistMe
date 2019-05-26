using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Label : IComparable<Label>
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public virtual List<QuestionLabel> QuestionLabels { get; set; }
        public int NumberOfTimes { get; set; }

        public int CompareTo(Label other)
        {
            int comp1 = -(NumberOfTimes - other.NumberOfTimes);
            if (comp1 != 0) return comp1;
            return Tag.CompareTo(other.Tag);
        }
    }
}
