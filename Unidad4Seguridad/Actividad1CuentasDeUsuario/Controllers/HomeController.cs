using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Actividad1CuentasDeUsuario.Controllers
{
    public class HomeController : Controller
    {
        public IWebHostEnvironment Environment { get; set; }

        public HomeController(IWebHostEnvironment env)
        {
            Environment = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(string email)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("noreply@sistemas171.com","Cuenta automatizada de Sistemas171");
            message.To.Add(email);
            message.Subject = "Prueba de email";

            //Leo el documento html para enviarlo
            string mensaje = System.IO.File.ReadAllText(Environment.WebRootPath + "/Correo.html");
            message.IsBodyHtml = true;
            message.Body = mensaje;

            SmtpClient client = new SmtpClient("mail.sistemas171.com",2525);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("noreply@sistemas171.com", "##ITESRC2020");
            client.Send(message);

            return RedirectToAction("Entrada");
        }
        public IActionResult IniciarSesion()
        {
            return View();
        }
        public IActionResult CambiarContraseña()
        {
            return View();
        }

        public IActionResult RecuperarContraseña()
        {
            return View();
        }


        public IActionResult Entrada()
        {
            return View();
        }
    }
}
