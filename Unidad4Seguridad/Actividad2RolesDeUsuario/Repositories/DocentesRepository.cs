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

        public override bool Validar(Docente entidad)
        {
            if(string.IsNullOrEmpty(entidad.Nombre))
                throw new Exception("El nombre no puede estar vacio");
            if(string.IsNullOrWhiteSpace(entidad.Contraseña))
                throw new Exception("La contraseña no puede estar vacía");
            if(entidad.Contraseña.Length<=8)
                throw new Exception("La contraseña debe ser mayor a 5 caracteres");
            if(entidad.Clave<=0)
                throw new Exception("Escriba la clave de docente");
            return true;
        }

    }
}
