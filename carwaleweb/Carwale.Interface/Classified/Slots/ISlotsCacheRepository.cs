using Carwale.Entity.Classified;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.Slots
{
    public interface ISlotsCacheRepository
    {
        IEnumerable<Slot> GetSlotsByCityId(int cityId);
    }
}
