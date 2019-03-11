using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Question : Element
    {
        //private  adj; 

        public Boolean IsArchived { get; set; }

        private List<Answer> ListRespuesta { get; set; }

    }
}
