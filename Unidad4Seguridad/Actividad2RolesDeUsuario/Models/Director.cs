using System;
using System.Collections.Generic;

namespace Actividad2RolesDeUsuario.Models
{
    public partial class Director
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public int Clave { get; set; }
    }
}
