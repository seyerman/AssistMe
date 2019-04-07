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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        public DbSet<Label> Label { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Studio> Studio { get; set; }
     
        public static object Configuration { get; internal set; }
    }
}
