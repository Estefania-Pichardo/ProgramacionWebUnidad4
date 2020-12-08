using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actividad2RolesDeUsuario.Models;
using Microsoft.EntityFrameworkCore;

namespace Actividad2RolesDeUsuario.Repositories
{
    public class AlumnosRepository:Repository<Alumno>
    {
        public AlumnosRepository(rolesusuarioContext context):base(context)
        {

        }
        public Alumno GetAlumnoByNumControl(string numControl)
        {
            return Context.Alumno.FirstOrDefault(x => x.NumControl.ToUpper() == numControl.ToUpper());
        }

        public override bool Validar(Alumno entidad)
        {
            if (string.IsNullOrEmpty(entidad.NumControl))
                throw new Exception("El numero de control no puede estar vacio");
            if(string.IsNullOrEmpty(entidad.Nombre))
                throw new Exception("El Nombre no puede estar vacio");
            if(entidad.IdMaestro<=0||entidad.IdMaestro==null)
                throw new Exception("El alumno debe de estar asignado a un maestro");

            return true;
        }

    }
}
