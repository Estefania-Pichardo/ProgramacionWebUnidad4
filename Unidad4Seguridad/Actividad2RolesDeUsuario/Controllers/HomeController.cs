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
using System.Security.Claims;

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

        [HttpPost]
        public async Task<IActionResult> IniciarSesionDirector(Director d)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            Repository<Director> repos = new Repository<Director>(context);

            var director = context.Director.FirstOrDefault(x=>x.Clave==d.Clave);

            try
            {
                if (director != null && director.Contraseña == d.Contraseña)
                {
                    List<Claim> informacion = new List<Claim>();

                    informacion.Add(new Claim(ClaimTypes.Name, "Usuario" + director.Nombre));
                    informacion.Add(new Claim("Clave", director.Clave.ToString()));
                    informacion.Add(new Claim(ClaimTypes.Role, "Director"));
                    informacion.Add(new Claim("Nombre Completo", director.Nombre));
                    informacion.Add(new Claim("Fecha Ingreso",  DateTime.Now.ToString()));

                    var claimsIdentity = new ClaimsIdentity(informacion, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                    new AuthenticationProperties { IsPersistent = true });

                    return RedirectToAction("Principal");
                }
                else
                {
                    ModelState.AddModelError("", "La clave de docente o la contraseña es incorrecta");
                    return View(d);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(d);
            }
        }

        [AllowAnonymous]
        public IActionResult IniciarSesionDocente()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesionDocente(Docente d)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);

            var docente =repos.GetDocenteByClave(d.Clave);

            try
            {
                if (docente != null && docente.Contraseña == d.Contraseña && docente.Activo == 1)
                {
                    List<Claim> informacion = new List<Claim>();

                    informacion.Add(new Claim(ClaimTypes.Name, "Usuario" + docente.Nombre));
                    informacion.Add(new Claim("Clave", docente.Clave.ToString()));
                    informacion.Add(new Claim(ClaimTypes.Role, "Docente"));
                    informacion.Add(new Claim("Nombre Completo", docente.Nombre));
                    informacion.Add(new Claim("Fecha Ingreso", DateTime.Now.ToString()));

                    var claimsIdentity = new ClaimsIdentity(informacion, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                    new AuthenticationProperties { IsPersistent = true });

                    return RedirectToAction("Principal",docente.Clave);
                }
                else
                {
                    ModelState.AddModelError("", "La clave de docente o la contraseña es incorrecta");
                    return View(d);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(d);
            }
        }

        [Authorize(Roles ="Docente, Director")]
        public IActionResult Principal(int clave)
        {

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }


    }
}
