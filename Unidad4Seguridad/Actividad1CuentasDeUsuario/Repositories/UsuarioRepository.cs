using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actividad1CuentasDeUsuario.Models;

namespace Actividad1CuentasDeUsuario.Repositories
{
    public class UsuarioRepository: Repository<Usuario>
    {
        public UsuarioRepository(controlusuariosContext context) : base(context) { }

        public Usuario GetUsuarioByCorreo(string correo)
        {
            return Context.Usuario.FirstOrDefault(x => x.Correo == correo);
        }

        public override bool Validar(Usuario entidad)
        {
            if (string.IsNullOrEmpty(entidad.Correo))
                throw new Exception("Ingrese su correo electronico");
            if(string.IsNullOrEmpty(entidad.Contraseña))
                throw new Exception("Ingrese su contraseña");

            return true;
        }
    }
}
