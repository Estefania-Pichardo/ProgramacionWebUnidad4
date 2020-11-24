using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ejemplo1.Controllers
{
   
    public class HomeController : Controller
    {
        [Authorize(Roles = "Alumno, Maestro")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Maestro")]
        public IActionResult Vista2()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(string username, string password)
        {
            if ((username == "Hector" || username == "Juan"||username=="Estefania") && password == "2020")
            {
                List<Claim> informacion = new List<Claim>();

               

                if (username == "Hector")
                {
                    informacion.Add(new Claim(ClaimTypes.Role, "Maestro"));
                }
                //Esta info se guarda en la cookie
                informacion.Add(new Claim(ClaimTypes.Name, "Alumno del ITESRC " + username));
                informacion.Add(new Claim(ClaimTypes.Role, "Alumno"));
                informacion.Add(new Claim("Nombre Completo", username));
                informacion.Add(new Claim("Fecha Ingreso", DateTime.Now.ToString()));

                //Hay que instanciar un claims principal al que se le asigna un claims identity
                var claimsIdentity = new ClaimsIdentity(informacion, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync( CookieAuthenticationDefaults.AuthenticationScheme,claimsPrincipal,
                    new AuthenticationProperties { IsPersistent=true});

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "El usuario o la contraseña son incorrectas");
                return View();

            }

        }
        [AllowAnonymous]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public IActionResult Denegado()
        {
            return View();
        }
    }
}
