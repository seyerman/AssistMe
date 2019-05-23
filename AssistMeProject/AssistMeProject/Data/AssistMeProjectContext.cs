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
            modelBuilder.Entity<QuestionStudio>().HasKey(x => new { x.QuestionId, x.StudioId });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                .HasOne<User>(c => c.User)
                        .WithMany(a => a.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Answer>()
                .HasOne<User>(c => c.User)
                        .WithMany(a => a.Answers)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InterestingVote>()
                .HasOne<User>(c => c.User)
                        .WithMany()
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PositiveVote>()
               .HasOne<User>(c => c.User)
                       .WithMany()
               .HasForeignKey(c => c.UserID)
               .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<AssistMeProject.Models.Question> Question { get; set; }
        public DbSet<AssistMeProject.Models.Answer> Answer { get; set; }
        public DbSet<AssistMeProject.Models.Comment> Comment { get; set; }
        public DbSet<AssistMeProject.Models.User> User { get; set; }
        public DbSet<AssistMeProject.Models.Studio> Studio { get; set; }
        public DbSet<AssistMeProject.Models.Notification> Notification { get; set; }

        public DbSet<AssistMeProject.Models.Label> Label { get; set; }
        public DbSet<AssistMeProject.Models.PositiveVote> PositiveVote { get; set; }
        public DbSet<AssistMeProject.Models.InterestingVote> InterestingVote { get; set; }
        public DbSet<AssistMeProject.Models.View> View { get; set; }//


        public DbSet<AssistMeProject.Models.QuestionLabel> QuestionLabels { get; set; }

    }
}