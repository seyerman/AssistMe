using AssistMeProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.ViewComponents
{
    public class StatsViewComponent : ViewComponent
    {
        private readonly AssistMeProjectContext _context;

        public StatsViewComponent(AssistMeProjectContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["Questions"] =_context.Question.Count();
            ViewData["Answers"] = _context.Answer.Count();
            ViewData["Users"] = _context.User.Count();
            return View();
        }

    }
}
