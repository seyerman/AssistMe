using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssistMeProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AssistMeProject.ViewComponents
{
    public class CommentListViewComponent:ViewComponent
    {

        private readonly AssistMeProjectContext _context;

        public CommentListViewComponent(AssistMeProjectContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int AnswerId)
        {
            var comments =  _context.Comment.Where(c => c.AnswerId == AnswerId).Include(c => c.User).Include(c=>c.Answer);
            return View( comments);
        }
    }
}
