using System;
using System.Collections.Generic;
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


        public Answer()
        {
            this.Comments = new HashSet<Comment>();
            this.PositiveVotes = new HashSet<PositiveVote>();

        }

        
        public bool UserVote(int userId)
        {
            return PositiveVotes.Any(x => x.UserID == userId);
        }



    }
}
