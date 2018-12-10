using Carwale.Entity.Classified;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.Slots
{
    public interface ISlotsRepository
    {
        IEnumerable<Slot> GetSlotsByCityId(int cityId);
    }
}
