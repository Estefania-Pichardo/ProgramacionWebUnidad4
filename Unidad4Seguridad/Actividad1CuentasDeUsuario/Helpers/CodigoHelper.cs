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
            int cod1 = r.Next();
            int cod2 = r.Next();

            return cod1 + cod2;
        }
    }
}
