using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified
{
    public interface ICWCTCityMappingRepositiory
    {
        bool IsCarTradeCity(int cityId);
    }
}
