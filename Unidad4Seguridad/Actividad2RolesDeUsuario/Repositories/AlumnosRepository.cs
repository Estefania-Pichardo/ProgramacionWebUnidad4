using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actividad2RolesDeUsuario.Models;

namespace Actividad2RolesDeUsuario.Repositories
{
    public class AlumnosRepository:Repository<Alumno>
    {
        public AlumnosRepository(rolesusuarioContext context):base(context)
        {

        }
    }
}
