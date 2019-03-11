using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AssistMeProject.Models
{
    public class AssistMeProjectContext : DbContext
    {
        public AssistMeProjectContext (DbContextOptions<AssistMeProjectContext> options)
            : base(options)
        {
        }

        public DbSet<AssistMeProject.Models.Question> Question { get; set; }
    }
}
