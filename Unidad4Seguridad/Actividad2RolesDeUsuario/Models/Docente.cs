using System;
using System.Collections.Generic;

namespace Actividad2RolesDeUsuario.Models
{
    public partial class Docente
    {
        public Docente()
        {
            Alumno = new HashSet<Alumno>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public ulong? Activo { get; set; }
        public int Clave { get; set; }

        public virtual ICollection<Alumno> Alumno { get; set; }
    }
}
