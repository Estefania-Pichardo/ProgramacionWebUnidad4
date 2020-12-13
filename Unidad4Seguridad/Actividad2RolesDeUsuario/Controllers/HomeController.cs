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
using Actividad2RolesDeUsuario.Models.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


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

            var director = context.Director.FirstOrDefault(x => x.Clave == d.Clave);

            try
            {
                if (director != null && director.Contraseña == HashingHelpers.GetHash(d.Contraseña))
                {
                    List<Claim> informacion = new List<Claim>();

                    informacion.Add(new Claim(ClaimTypes.Name, "Usuario" + director.Nombre));
                    informacion.Add(new Claim("Clave", director.Clave.ToString()));
                    informacion.Add(new Claim(ClaimTypes.Role, "Director"));
                    informacion.Add(new Claim("Nombre Completo", director.Nombre));
                    informacion.Add(new Claim("Fecha Ingreso", DateTime.Now.ToString()));

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

            var docente = repos.GetDocenteByClave(d.Clave);

            try
            {
                if (docente != null && docente.Contraseña == HashingHelpers.GetHash(d.Contraseña))
                {
                    if (docente.Activo == 1)
                    {
                        List<Claim> informacion = new List<Claim>();

                        informacion.Add(new Claim(ClaimTypes.Name, "Usuario" + docente.Nombre));
                        informacion.Add(new Claim("Clave", docente.Clave.ToString()));
                        informacion.Add(new Claim(ClaimTypes.Role, "Docente"));
                        informacion.Add(new Claim("Nombre Completo", docente.Nombre));
                        informacion.Add(new Claim("IdDocente", docente.Id.ToString()));

                        var claimsIdentity = new ClaimsIdentity(informacion, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                        new AuthenticationProperties { IsPersistent = true });

                        return RedirectToAction("Principal", docente.Clave);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lo sentimos, su usuario esta desactivado, hable con su superior para activar la cuenta");
                        return View(d);
                    }

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

        [Authorize(Roles = "Docente, Director")]
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

        public IActionResult AccesoDenegado()
        {
            return View();
        }

        //CRUD y cambiar contraseña de docentes

        [Authorize(Roles = "Director")]
        public IActionResult VerDocentes()
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);
            var listaDocentes = repos.GetAll();

            return View(listaDocentes);
        }

        [Authorize(Roles = "Director")]
        public IActionResult AgregarDocente()
        {
            return View();
        }
        [Authorize(Roles = "Director")]
        [HttpPost]
        public IActionResult AgregarDocente(Docente d)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);
            try
            {
                var existe = repos.GetDocenteByClave(d.Clave);
                if (existe != null)
                {
                    ModelState.AddModelError("", "Ya existe un docente con esta clave");
                    return View(d);
                }
                else
                {
                    d.Activo = 1;
                    d.Contraseña = HashingHelpers.GetHash(d.Contraseña);
                    repos.Insert(d);
                    return RedirectToAction("VerDocentes");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(d);
            }

        }

        [Authorize(Roles = "Director")]
        public IActionResult EditarDocente(int id)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);
            var docente = repos.Get(id);

            if (docente == null)
            {
                return RedirectToAction("VerDocentes");
            }

            return View(docente);
        }

        [Authorize(Roles = "Director")]
        [HttpPost]
        public IActionResult EditarDocente(Docente d)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);
            var docente = repos.Get(d.Id);
            try
            {
                if (docente != null)
                {
                    docente.Nombre = d.Nombre;

                    repos.Update(docente);
                }

                return RedirectToAction("VerDocentes");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(docente);
            }
        }

        [Authorize(Roles = "Director")]
        public IActionResult CambiarContraseña(int id)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);
            var docente = repos.Get(id);

            if (docente == null)
            {
                return RedirectToAction("VerDocentes");
            }

            return View(docente);
        }

        [Authorize(Roles = "Director")]
        [HttpPost]
        public IActionResult CambiarContraseña(Docente d, string contraseñaNueva1, string contraseñaNueva2)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);
            var docente = repos.Get(d.Id);
            try
            {
                if (docente != null)
                {
                    if (contraseñaNueva1 == contraseñaNueva2)
                    {
                        docente.Contraseña = contraseñaNueva1;
                        docente.Contraseña = HashingHelpers.GetHash(contraseñaNueva1);
                        repos.Update(docente);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Las contraseñas no coinciden");
                        return View(docente);
                    }

                }
                return RedirectToAction("VerDocentes");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(docente);
            }
        }

        [HttpPost]
        public IActionResult DesactivarDocente(Docente d)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);

            var docenteDesactivar = repos.Get(d.Id);
            if (docenteDesactivar != null && docenteDesactivar.Activo == 1)
            {
                docenteDesactivar.Activo = 0;
                repos.Update(docenteDesactivar);
            }
            else
            {
                docenteDesactivar.Activo = 1;
                repos.Update(docenteDesactivar);
            }
            return RedirectToAction("VerDocentes");
        }

        [Authorize(Roles = "Director, Docente")]
        public IActionResult Alumnos(int id)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);
            var docente = repos.GetAlumnosPorDocente(id);

            if (docente != null)
            {
                if (User.IsInRole("Docente"))
                {
                    if (User.Claims.FirstOrDefault(x => x.Type == "IdDocente").Value == docente.Id.ToString())
                    {
                        return View(docente);
                    }
                    else
                    {
                        return RedirectToAction("AccesoDenegado");
                    }
                }
                else if (docente.Activo != 1)
                    return RedirectToAction("VerDocentes");
                else    
                    return View(docente);
            }
            else
                return RedirectToAction("VerDocentes");
        }

        //Empieza CRUD de Alumnos

        [Authorize(Roles ="Director, Docente")]
        public IActionResult AgregarAlumno(int id)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos= new DocentesRepository(context);
            AgregarAlumnoViewModel vm = new AgregarAlumnoViewModel();
            vm.Docente = repos.Get(id);
            if (vm.Docente != null)
            {
                if (User.IsInRole("Docente"))
                {
                    if (User.Claims.FirstOrDefault(x => x.Type == "IdDocente").Value == vm.Docente.Id.ToString())
                    {
                        return View(vm);
                    }
                    else
                    {
                        return RedirectToAction("AccesoDenegado");
                    }
                }
                else if (vm.Docente.Activo != 1)
                    return RedirectToAction("VerDocentes");
                else
                    return View(vm);
            }

            return View(vm);
        }

        [Authorize(Roles = "Director, Docente")]
        [HttpPost]
        public IActionResult AgregarAlumno(AgregarAlumnoViewModel vm)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            DocentesRepository repos = new DocentesRepository(context);
            AlumnosRepository reposAlumno = new AlumnosRepository(context);

            try
            {
                // vm.Docente = repos.Get(vm.Docente.Id);
                //vm.Docentntes = repos.GetAll();
                var idDocente = repos.GetDocenteByClave(vm.Docente.Clave).Id;
                vm.Alumno.IdMaestro = idDocente;
                reposAlumno.Insert(vm.Alumno);
                return RedirectToAction("Alumnos", new {id=idDocente});
            } 
            catch(Exception ex)
            {
                vm.Docente = repos.Get(vm.Docente.Id);
                vm.Docentntes = repos.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }  
        }

        [Authorize(Roles ="Director, Docente")]
        public IActionResult EditarAlumno(int id)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            AlumnosRepository reposAlumno = new AlumnosRepository(context);
            DocentesRepository reposDocente = new DocentesRepository(context);
            AgregarAlumnoViewModel vm = new AgregarAlumnoViewModel();
            vm.Alumno = reposAlumno.Get(id);
            vm.Docentntes = reposDocente.GetAll();

            if (vm.Alumno != null)
            {
                vm.Docente = reposDocente.Get(vm.Alumno.IdMaestro);
                if (User.IsInRole("Docente"))
                {
                    vm.Docente = reposDocente.Get(vm.Alumno.IdMaestro);

                    if (User.Claims.FirstOrDefault(x => x.Type == "Clave").Value == vm.Docente.Clave.ToString())
                    {
                        return View(vm);
                    }
                    else
                    {
                        return RedirectToAction("AccesoDenegado");
                    }
                }
                else if (vm.Docente.Activo != 1)
                    return RedirectToAction("VerDocentes");
                else
                    return View(vm);
            }
            else return RedirectToAction("Principal");

        }

        [Authorize(Roles = "Director, Docente")]
        [HttpPost]
        public IActionResult EditarAlumno(AgregarAlumnoViewModel vm)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            AlumnosRepository reposAlumno = new AlumnosRepository(context);
            DocentesRepository reposDocente = new DocentesRepository(context);
            try
            {
                var alumno = reposAlumno.Get(vm.Alumno.Id);
                if(alumno!=null)
                {
                    alumno.Nombre = vm.Alumno.Nombre;
                    if(User.IsInRole("Director"))
                    {
                        alumno.IdMaestro = vm.Alumno.IdMaestro;
                    }     
                    reposAlumno.Update(alumno);
                    return RedirectToAction("Alumnos", new { id = alumno.IdMaestro });
                }
                else
                {
                    ModelState.AddModelError("", "No se encontro el alumno a editar");
                    vm.Docente = reposDocente.Get(vm.Alumno.IdMaestro);
                    vm.Docentntes = reposDocente.GetAll();
                    return View(vm);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                vm.Docente = reposDocente.Get(vm.Alumno.IdMaestro);
                vm.Docentntes = reposDocente.GetAll();
                return View(vm);
            }

        }

        [Authorize(Roles = "Director, Docente")]
        [HttpPost]
        public IActionResult EliminarAlumno(Alumno a)
        {
            rolesusuarioContext context = new rolesusuarioContext();
            AlumnosRepository repos = new AlumnosRepository(context);

            var alumnoEliminar = repos.Get(a.Id);
            if (alumnoEliminar != null)
            {
                repos.Delete(alumnoEliminar);
            }
            else
            {
                ModelState.AddModelError("","No se encontro el alumno a eliminar");
            }
            return RedirectToAction("Alumnos", new { id = alumnoEliminar.IdMaestro });
        }
    }
}
