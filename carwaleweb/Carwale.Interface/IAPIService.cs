using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces
{
    public interface IAPIService<T, TResponse>
    {
        TResponse Request(T t);
        void UpdateResponse(T t, TResponse t2);
    }
}
