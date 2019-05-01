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
        public AssistMeProjectContext(DbContextOptions<AssistMeProjectContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionLabel>().HasKey(x => new { x.QuestionId, x.LabelId });
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AssistMeProject.Models.Question> Question { get; set; }
        public DbSet<AssistMeProject.Models.Answer> Answer { get; set; }
        public DbSet<AssistMeProject.Models.Comment> Comment { get; set; }
        public DbSet<AssistMeProject.Models.User> User { get; set; }
        public DbSet<AssistMeProject.Models.Studio> Studio { get; set; }
        public DbSet<AssistMeProject.Models.Label> Label { get; set; }
        public DbSet<AssistMeProject.Models.QuestionLabel> QuestionLabels { get; set; }
    }
}