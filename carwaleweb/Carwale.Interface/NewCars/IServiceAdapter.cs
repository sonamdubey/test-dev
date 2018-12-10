using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.NewCars
{
    [Obsolete("This Interface is deprecated .Use IServiceAdapterV2 instead")]
    public interface IServiceAdapter
    {
        [Obsolete("This function is deprecated .Use T Get<T,U>(U input) of IServiceAdapterV2  instead")]
        T Get<T>();       
    }  
}
