using Carwale.Entity.Accessories.Tyres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Accessories.Tyres
{
    public interface IAccessoryCache
    {
        ItemData GetAccessoryDataByItemId(int itemId);
    }
}
