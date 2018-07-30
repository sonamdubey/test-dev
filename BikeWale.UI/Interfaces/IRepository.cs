using System.Collections.Generic;

namespace Bikewale.Interfaces
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Base Interface for all the other interfaces.
    /// </summary>
    /// <typeparam name="T">Generic type (need to specify type while implementing this interface)</typeparam>
    /// <typeparam name="U">Generic type (need to specify type while implementing this interface)</typeparam>
    public interface IRepository<T, U>
    {
        U Add(T t);
        bool Update(T t);
        bool Delete(U id);
        List<T> GetAll();
        T GetById(U id);
    }
}