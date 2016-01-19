using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotification.Interfaces
{
    public interface IRequestManager<T>
    {
        void ProcessRequest(T t);
    }
}
