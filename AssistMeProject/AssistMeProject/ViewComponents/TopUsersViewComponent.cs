using AssistMeProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.ViewComponents
{
    public class TopUsersViewComponent : ViewComponent
    {

        private readonly AssistMeProjectContext _context;

        public TopUsersViewComponent(AssistMeProjectContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topUsers = _context.User.Take(10).Select(u => u.USERNAME).ToList();//cambiar por la logica para seleccionar topusers
            return View(topUsers);
        }

    }
}
