using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces
{
   public  interface ISecurity<T>
    {
        bool RsaDecrypt(T t);
         
    }
}
