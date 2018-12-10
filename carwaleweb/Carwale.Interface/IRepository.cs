using Carwale.Entity;
using System.Collections.Generic;

namespace Carwale.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllById(int id);
        PagedResult<T> Find(SearchQuery<T> query, int pageSize, int pageNumber);
        T GetById(int id);
        int Create(T entity);
        bool Update(T entity);
        bool Delete(int id);
        bool Delete(T entity);
        
    }
}
