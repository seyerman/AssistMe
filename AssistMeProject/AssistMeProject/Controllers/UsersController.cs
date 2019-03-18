using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AssistMeProject.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

<<<<<<< HEAD

=======
        public IActionResult Profile()
        {
            return View();
        }
>>>>>>> dd2fecdf8d085f2f478d2f4754399b5ec0f3d768
    }
}