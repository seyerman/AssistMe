using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Answer : Element
    {

        public virtual ICollection<Comment> Comments { get; set; }

        public int QuestionID { get; set; }

        public virtual Question Question { get; set; }

        public virtual ICollection<PositiveVote> PositiveVotes { get; set; }

        public Boolean correctAnswer { get; set; }

        [Url(ErrorMessage ="En este campo debe insertar una url a la pregunta que cosidera original")]

        public string UrlOriginalQuestion { get; set; }

        public Answer()
        {
            this.Comments = new HashSet<Comment>();
            this.PositiveVotes = new HashSet<PositiveVote>();

        }

        
        public bool UserVote(int userId)
        {
            if (userId == -1) return false;
            return PositiveVotes.Any(x => x.UserID == userId);
        }



    }
}
