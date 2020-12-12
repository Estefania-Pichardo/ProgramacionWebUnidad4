using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actividad1CuentasDeUsuario
{
    public static class CodigoHelper
    {
        public static int GetCodigo()
        {
            Random r = new Random();
            return r.Next(1000000,2000000);
        }
    }
}
