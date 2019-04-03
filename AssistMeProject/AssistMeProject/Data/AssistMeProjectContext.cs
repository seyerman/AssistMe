using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;

namespace AssistMeProject.Models
{
    public class AssistMeProjectContext : DbContext
    {
        public AssistMeProjectContext (DbContextOptions<AssistMeProjectContext> options)
            : base(options)
        {

        }

        public DbSet<AssistMeProject.Models.Question> Question { get; set; }
        public DbSet<AssistMeProject.Models.Answer> Answer { get; set; }
        public DbSet<AssistMeProject.Models.Comment> Comment { get; set; }
        public DbSet<AssistMeProject.Models.User> User { get; set; }
        public DbSet<AssistMeProject.Models.Studio> Studio { get; set; }
        public DbSet<AssistMeProject.Models.Label>Label { get; set; }
        public DbSet<AssistMeProject.Models.PositiveVote> PositiveVote { get; set; }
        public DbSet<AssistMeProject.Models.InterestingVote> InterestingVote { get; set; }
    }
}
