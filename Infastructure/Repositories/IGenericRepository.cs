using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(object id);
        Task Insert(T obj);
        Task Delete(object id);
        Task<T> Update(T obj, object key);
        Task Save();
        IQueryable<T> List();
        IQueryable<T> GetAllIncluding(params Expression<Func<T,object>>[] includeProperties);
    }
}
