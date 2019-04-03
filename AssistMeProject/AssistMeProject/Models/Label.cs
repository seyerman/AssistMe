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

        private string Icon { get; set; }

        private int NumberOfTimes { get; set; }
    }
}
