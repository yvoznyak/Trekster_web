using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.ServiceInterfaces
{
    public interface IBaseService<T>
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        void Save(T entity);

        void Delete(int id);
    }
}
