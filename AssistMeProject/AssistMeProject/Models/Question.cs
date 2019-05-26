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

        [Required(ErrorMessage = "Agregue etiquetas a la pregunta")]
        [MaxLength(300)]
        [Display(Name = "Etiquetas")]
        [RegularExpression("^[^,]+(,[^,]+){0,4}$", ErrorMessage = "Puedes introducir hasta cinco etiquetas")]
        public string question_tags { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }


        public virtual ICollection<InterestingVote> InterestingVotes { get; set; }
        public virtual ICollection<View> Views { get; set; }
        public string Insignia { get; set; }
     
 

        [Display(Name = "Nombre")]
        public virtual List<QuestionLabel> QuestionLabels { get; set; }

        public bool AskAgain { get; set; }

        public bool isArchived { get; set; }

        public virtual ICollection<QuestionStudio> QuestionStudios { get; set; }

        public User User { get; set; }

        public int? UserId { get; set; }

        public Question()
        {
            Answers = new HashSet<Answer>();
            InterestingVotes = new HashSet<InterestingVote>();
            Views = new HashSet<View>();
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




        //Method to know if the user already vote interesting
         public bool UserVote(int userId) {

            Console.WriteLine("____________");
            Console.WriteLine("____________");
            Console.WriteLine("____________");
            Console.WriteLine("____________");
            Console.WriteLine("____________");
            Console.WriteLine("____________");
            Console.WriteLine("____________");
            Console.WriteLine("____________");
            Console.WriteLine(userId);
            return InterestingVotes.Any(x => x.UserID == userId); 
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

        //
        public bool UserView(int userId)
        {

            return Views.Any(x => x.UserID == userId);
        }

        public bool HasTag(string tag)
        {
            if (String.IsNullOrWhiteSpace(tag)) return true;
            foreach (QuestionLabel ql in QuestionLabels)
            {
                if (ql.Label.Tag == tag)
                    return true;
            }
            return false;
        }

        public bool HasStudio(string studio)
        {
            if (String.IsNullOrWhiteSpace(studio)) return true;
            foreach (QuestionStudio qs in QuestionStudios)
            {
                if (qs.Studio.Name == studio)
                    return true;
            }
            return false;
        }

        public bool IsUser(string username)
        {
            return String.IsNullOrWhiteSpace(username) || User.USERNAME.ToLower() == username.ToLower();
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
