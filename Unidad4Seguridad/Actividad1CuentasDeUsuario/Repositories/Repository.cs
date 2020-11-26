using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actividad1CuentasDeUsuario.Models;

namespace Actividad1CuentasDeUsuario.Repositories
{
    public class Repository<T> where T:class
    {
        public controlusuariosContext Context { get; set; }

        public Repository(controlusuariosContext ctx)
        {
            Context = ctx;
        }
        public T Get(object id)
        {
            return Context.Find<T>(id);
        }

        public virtual void Insert(T entidad)
        {
            if (Validar(entidad))
            {
                Context.Add(entidad);
                Context.SaveChanges();
            }
        }
        public virtual void Update(T entidad)
        {
            if (Validar(entidad))
            {
                Context.Update<T>(entidad);
                Context.SaveChanges();
            }
        }

        public bool Validar(T entidad)
        {

            return true;
        }
    }
}
