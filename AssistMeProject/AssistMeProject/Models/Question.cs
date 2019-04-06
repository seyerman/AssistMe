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
        [Required(ErrorMessage ="Agregue un Titulo a su pregunta"), MaxLength(150),Display(Name ="Titulo")]
        public string Title { get; set; }
   //     public List<Label> labels { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<InterestingVote> InterestingVotes { get; set; }

        public Question()
        {
            Answers = new HashSet<Answer>();
            InterestingVotes = new HashSet<InterestingVote>();
        }

        public override string GetDocumentText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Title);
            sb.Append(" ");
            sb.Append(Description);
            return sb.ToString();
        }

        public bool UserVote(int userId) {

            return InterestingVotes.Any(x => x.UserID == userId); 
        }

    }
}
