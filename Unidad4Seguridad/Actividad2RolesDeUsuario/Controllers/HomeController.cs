using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Actividad2RolesDeUsuario.Models;
using Actividad2RolesDeUsuario.Repositories;

namespace Actividad2RolesDeUsuario.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult IniciarSesionDirector()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult IniciarSesionDocente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(Docente d, Director dir)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);

            
            return RedirectToAction("Principal");
        }

        [Authorize(Roles ="Docente, Director")]
        public IActionResult Principal(Docente d, Director dir)
        {
            return View();
        }


    }
}
