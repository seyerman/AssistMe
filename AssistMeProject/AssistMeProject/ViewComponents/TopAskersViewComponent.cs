using AssistMeProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.ViewComponents
{
    public class TopAskersViewComponent : ViewComponent
    {


        private readonly AssistMeProjectContext _context;

        public TopAskersViewComponent (AssistMeProjectContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topUsers = _context.User.OrderByDescending(u=>u.Questions.Count).Take(5);//cambiar por la logica para seleccionar topusers
            return View(topUsers);
        }
    }
}
