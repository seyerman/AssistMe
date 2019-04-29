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
        [Required(ErrorMessage = "Agregue un Titulo a su pregunta"), MaxLength(150), Display(Name = "Titulo")]
        public string Title { get; set; }

        public Boolean isArchived { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        [Display(Name = "Nombre")]
        public virtual List<QuestionLabel> QuestionLabels { get; set; }

        public bool AskAgain { get; set; }

        public string Username { get; set; }

        public Question()
        {
            Answers = new HashSet<Answer>();
            AskAgain = false;
            isArchived = false;
        }

        public override string GetDocumentText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Title);
            sb.Append(" ");
            sb.Append(Description);
            return sb.ToString();
        }

        public override int CompareTo(object obj)
        {
            Question other = (Question)obj;
            if(this.AskAgain && !other.AskAgain)
            {
                return -1;
            }
            if (!this.AskAgain && other.AskAgain)
            {
                return 1;
            }
            return base.CompareTo(obj);
        }

    }
}
