using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Repository
{
    public interface IRepository<T>
    {
        IList<T> GetAll();

        T GetById(string id);

        void Insert(T value);

        void Update(T value);

        void Delete(T value);
    }
}
