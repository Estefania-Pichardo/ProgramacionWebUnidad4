using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Actividad2RolesDeUsuario.Models;

namespace Actividad2RolesDeUsuario.Repositories
{
    public class DocentesRepository: Repository<Docente>
    {
        public DocentesRepository(rolesusuarioContext contex):base(contex)
        {

        }

        public Docente GetDocenteByClave(int clave)
        {
            return Context.Docente.FirstOrDefault(x => x.Clave == clave);
        }

        public Docente GetAlumnosPorDocente(int id)
        {
            return Context.Docente.Include(x => x.Alumno).FirstOrDefault(x => x.Id == id);
        }

    }
}
