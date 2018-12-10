using Carwale.Entity.ES;
using System.Collections.Generic;

namespace Carwale.Interfaces.ES
{
    public interface ISponsoredNavigationCache
    {
        List<SponsoredNavigation> GetSponsoredNavigationData(int sectionId, int platformId);
    }
}
