using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces
{
    public interface IRequestManager<T>
    {
        U ProcessRequest<U>(T t);
    }
}
