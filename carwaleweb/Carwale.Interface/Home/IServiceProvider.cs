using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Home
{
    public interface IServiceAdapter
    {
        T Get<T>();
        T Get<T>(string param);
    }
}
