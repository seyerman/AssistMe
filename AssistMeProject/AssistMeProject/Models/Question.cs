using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Question : Element
    {
        [Required(ErrorMessage ="Agregue un Titulo a su pregunta"), MaxLength(50),Display(Name ="Titulo")]
        public string Title { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public Question()
        {
            Answers = new HashSet<Answer>();
        }


    }
}
