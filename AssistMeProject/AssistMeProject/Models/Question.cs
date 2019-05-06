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
        public virtual ICollection<View> Views { get; set; }
        public Question()
        {
            Answers = new HashSet<Answer>();
            InterestingVotes = new HashSet<InterestingVote>();
            Views = new HashSet<View>();
 
        }

        public override string GetDocumentText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Title);
            sb.Append(" ");
            sb.Append(Description);
            return sb.ToString();
        }


        
        //Method to know if the user already vote interesting
        public bool UserVote(int userId) {

            return InterestingVotes.Any(x => x.UserID == userId); 
        }


        public bool UserView(int userId)
        {

            return Views.Any(x => x.UserID == userId);
        }
    }
}
