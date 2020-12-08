using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actividad2RolesDeUsuario.Models.ViewModels
{
    public class AgregarAlumnoViewModel
    {
        public Alumno Alumno { get; set; }

        public Docente Docente { get; set; }
        public IEnumerable<Docente> Docentntes { get; set; }
    }
}
