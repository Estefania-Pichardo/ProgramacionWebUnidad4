using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
