using Carwale.Entity.Classified.SellCarUsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.SellCar
{
    public interface ICarDetailsBL
    {
        int ProcessCarDetails(SellCarInfo sellCarInfo);
    }
}
