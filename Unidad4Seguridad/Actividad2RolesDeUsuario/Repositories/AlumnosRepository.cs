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

    }
}
