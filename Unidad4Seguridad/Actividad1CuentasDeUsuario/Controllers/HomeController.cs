using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Actividad1CuentasDeUsuario.Models;
using Actividad1CuentasDeUsuario.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Actividad1CuentasDeUsuario.Controllers
{
    public class HomeController : Controller
    {
        public IWebHostEnvironment Environment { get; set; }

        //controlusuariosContext Context;

        public HomeController(IWebHostEnvironment env/*, controlusuariosContext ctx*/)
        {
            Environment = env;
            //Context = ctx;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Registrar()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Registrar(Usuario u)
        {

            //Esto es para enviar el correo
            MailMessage message = new MailMessage();
            message.From = new MailAddress("noreply@sistemas171.com","Cuenta automatizada de Sistemas171");
            message.To.Add(u.Correo);
            message.Subject = "Prueba de email";

            //Leo el documento html para enviarlo
            string mensaje = System.IO.File.ReadAllText(Environment.WebRootPath + "/Correo.html");
            message.IsBodyHtml = true;
            message.Body = mensaje;

            SmtpClient client = new SmtpClient("mail.sistemas171.com",2525);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("noreply@sistemas171.com", "##ITESRC2020");
            client.Send(message);

            //Para agregar el usuario a la base de datos
            try
            {
                controlusuariosContext Context = new controlusuariosContext();
                Repository<Usuario> repos = new Repository<Usuario>(Context);
                repos.Insert(u);
                return RedirectToAction("Espere");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(u);
            }

            
        }

        [AllowAnonymous]
        public IActionResult Espere()
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
        public async Task<IActionResult> IniciarSesion(Usuario u)
        {
            controlusuariosContext Context = new controlusuariosContext();
            Repository<Usuario> repos = new Repository<Usuario>(Context);
            var usuario = Context.Usuarios.FirstOrDefault(x => x.Correo == u.Correo);

            if (usuario != null && u.Contraseña == usuario.Contraseña)
            {
                List<Claim> informacion = new List<Claim>();

                informacion.Add(new Claim(ClaimTypes.Name, "Usuario" + usuario.NombreUsuario));
                informacion.Add(new Claim("Correo electronico", usuario.Correo));
                informacion.Add(new Claim("Fecha Ingreso", DateTime.Now.ToString()));

                //Hay que instanciar un claims principal al que se le asigna un claims identity
                var claimsIdentity = new ClaimsIdentity(informacion, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                    new AuthenticationProperties { IsPersistent = true });

                return RedirectToAction("Entrada");
            }
            else
            {
                ModelState.AddModelError("", "El usuario o la contraseña son incorrectas");
                return View();
            }
            

            
        }

        [Authorize]
        public IActionResult CambiarContraseña()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult RecuperarContraseña()
        {
            return View();
        }

        [Authorize]
        public IActionResult Entrada()
        {
            return View();
        }
    }
}
