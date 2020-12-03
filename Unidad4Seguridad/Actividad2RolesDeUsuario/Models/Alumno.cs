using System;
using System.Collections.Generic;

namespace Actividad2RolesDeUsuario.Models
{
    public partial class Alumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int? IdMaestro { get; set; }

        public virtual Docente IdMaestroNavigation { get; set; }
    }
}
