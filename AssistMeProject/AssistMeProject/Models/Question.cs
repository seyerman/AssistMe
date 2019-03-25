using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Question : Element
    {
        [Required(ErrorMessage ="Agregue un Titulo a su pregunta"), MaxLength(50),Display(Name ="Titulo")]
        [Required(ErrorMessage ="Agregue un Titulo a su pregunta"), MaxLength(150),Display(Name ="Titulo")]
        public string Title { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public override string GetDocumentText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Title);
            sb.Append(" ");
            sb.Append(Description);
            return sb.ToString();
        }

    }
}
