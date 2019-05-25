using AssistMeProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.ViewComponents
{
    public class TopReplyersViewComponent : ViewComponent
    {
        private readonly AssistMeProjectContext _context;

        public TopReplyersViewComponent(AssistMeProjectContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topUsers = _context.User.Include(u => u.Answers).OrderByDescending(u => u.Answers.Count).Take(5);//cambiar por la logica para seleccionar topusers
            return View(topUsers);
        }

    }
}
