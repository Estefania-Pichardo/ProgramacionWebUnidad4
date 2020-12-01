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
        public IActionResult Registrar(Usuario u, string contraseña1, string contraseña2)
        {
            controlusuariosContext Context = new controlusuariosContext();
            //Para agregar el usuario a la base de datos
            try
            {
                //Revisar que no exista una cuenta con ese correo
                if (Context.Usuario.Any(x => x.Correo == u.Correo))
                {
                    ModelState.AddModelError("", "Ya existe una cuenta registrada con este correo");
                    return View(u);
                }
                else
                {
                    if (contraseña1 == contraseña2)
                    {
                        Repository<Usuario> repos = new Repository<Usuario>(Context);
                        u.Contraseña = HashingHelpers.GetHash(contraseña1);
                        u.Codigo = CodigoHelper.GetCodigo();
                        u.Activo = 0;
                        repos.Insert(u);

                        //Esto es para enviar el correo
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("sistemascomputacionales7g@gmail.com", "Music+");
                        message.To.Add(u.Correo);
                        message.Subject = "Confirma tu correo";

                        //Leo el documento html para enviarlo
                        string mensaje = System.IO.File.ReadAllText(Environment.WebRootPath + "/Correo.html");
                        message.IsBodyHtml = true;
                        message.Body = mensaje.Replace("{##Codigo##}", u.Codigo.ToString());

                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("sistemascomputacionales7g@gmail.com", "sistemas7g");
                        client.Send(message);


                        List<Claim> informacion = new List<Claim>();
                        informacion.Clear();
                        informacion.Add(new Claim("CorreoActivar", u.Correo));

                        return RedirectToAction("ActivarCuenta");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Las contraseñas no coinciden");
                        return View(u);
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(u);
            }



        }

        [AllowAnonymous]
        public IActionResult ActivarCuenta()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ActivarCuenta(int codigo)
        {
            controlusuariosContext context = new controlusuariosContext();
            UsuarioRepository repos = new UsuarioRepository(context);
            var user = context.Usuario.FirstOrDefault(x=>x.Codigo==codigo);
            
            if(user!=null)
            {
                var cod = user.Codigo;

                if(codigo==cod)
                {
                    user.Activo = 1;
                    repos.Update(user);
                    
                    return RedirectToAction("IniciarSesion");
                }
                else
                {
                    ModelState.AddModelError("", "El codigo no coincide");
                    return View((object)codigo);
                }
            }
            else
            {
                ModelState.AddModelError("", "No se encontro el usuario");
                return View((object)codigo);
            }
        }

        [AllowAnonymous]
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(Usuario u, bool persistant)
        {
            controlusuariosContext Context = new controlusuariosContext();

            UsuarioRepository repos = new UsuarioRepository(Context);
           // Repository<Usuario> repos = new Repository<Usuario>(Context);

            var usuario = repos.GetUsuarioByCorreo(u.Correo);

            if (usuario != null && HashingHelpers.GetHash(u.Contraseña) == usuario.Contraseña)
            {
                if(usuario.Activo==1)
                {
                    List<Claim> informacion = new List<Claim>();

                    informacion.Add(new Claim(ClaimTypes.Name, "Usuario" + usuario.NombreUsuario));
                    informacion.Add(new Claim("Correo electronico", usuario.Correo));
                    informacion.Add(new Claim("Nombre Completo", usuario.NombreUsuario));
                    informacion.Add(new Claim("Fecha Ingreso", DateTime.Now.ToString()));

                    //Hay que instanciar un claims principal al que se le asigna un claims identity
                    var claimsIdentity = new ClaimsIdentity(informacion, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                    new AuthenticationProperties { IsPersistent = false });


                    return RedirectToAction("Entrada");
                }
                else
                {
                    ModelState.AddModelError("", "La cuenta registrada con este correo electronico no esta activa");
                    return View(u);
                }
               
            }
            else
            {
                ModelState.AddModelError("", "El usuario o la contraseña son incorrectas");
                return View(u);
            }



        }

        [Authorize]
        public IActionResult CambiarContraseña()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult CambiarContraseña(string contraseñaNueva1, string correo, string contraseñaNueva2)
        {
            controlusuariosContext Context = new controlusuariosContext();
            UsuarioRepository repos = new UsuarioRepository(Context);
           //Repository<Usuario> repos = new Repository<Usuario>(Context);

            var user = repos.GetUsuarioByCorreo(correo);
            try
            {
                if (contraseñaNueva1 == contraseñaNueva2)
                {
                    user.Contraseña = HashingHelpers.GetHash(contraseñaNueva1);
                    if (user.Contraseña == contraseñaNueva1)
                    {
                        ModelState.AddModelError("", "La nueva contraseña no puede ser igual a la ya registrada");
                        return View(contraseñaNueva1);
                    }
                    else
                    {
                        repos.Update(user);

                        return RedirectToAction("Entrada");
                    }
                }
                else
                {
                    ModelState.AddModelError("","Las contraseñas no coinciden");
                    return View();

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(contraseñaNueva1, contraseñaNueva2);
            }
           
        }

        [AllowAnonymous]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult RecuperarContraseña()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RecuperarContraseña(string correo)
        {
            try
            {
                controlusuariosContext Context = new controlusuariosContext();
                UsuarioRepository repos = new UsuarioRepository(Context);
                //Repository<Usuario> repos = new Repository<Usuario>(Context);

                var user = repos.GetUsuarioByCorreo(correo);

                if (user != null)
                {

                    var contraseñaTemporal = CodigoHelper.GetCodigo();

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("sistemascomputacionales7g@gmail.com", "Music+");
                    message.To.Add(correo);
                    message.Subject = "Recupera tu contraseña";

                    message.Body = $"Utiliza esta contraseña temporal para ingresar a tu cuenta, recuerda que una vez que ingreses deberas cambiarla: {contraseñaTemporal} ";

                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sistemascomputacionales7g@gmail.com", "sistemas7g");
                    client.Send(message);

                    user.Contraseña = HashingHelpers.GetHash(contraseñaTemporal.ToString());
                    repos.Update(user);
                    return RedirectToAction("IniciarSesion");
                }
                else
                {
                    ModelState.AddModelError("", "El correo no esta registrado");
                    return View();
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View((object)correo);
            }
           
        }

        [Authorize]
        public IActionResult Entrada()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Eliminar(string correo)
        {
            controlusuariosContext Context = new controlusuariosContext();

            UsuarioRepository repos = new UsuarioRepository(Context);
            var user = repos.GetUsuarioByCorreo(correo);

            if(user!=null)
            {
                repos.Delete(user);
            }
            else
            {
                ModelState.AddModelError("", "Ha ocurrido un error");
                return View("Entrada");
            }
            return RedirectToAction("Index");
        }
    }
}
