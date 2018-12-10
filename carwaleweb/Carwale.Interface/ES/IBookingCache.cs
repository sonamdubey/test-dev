using Carwale.Entity.ES;
using System.Collections.Generic;

namespace Carwale.Interfaces.ES
{
    public interface IBookingCache
    {
        List<ESVersionColors> GetBookingModelData(int modelId);
    }
}
