
using Bikewale.Entities;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
namespace Bikewale.Models
{
    public class DealerShowroomIndexPage
    {
        private readonly IDealer _objDealer = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;

        protected string makeMaskingName = string.Empty, redirectUrl = string.Empty;
        public uint MakeId;
        public StatusCodes status;
        public DealerShowroomIndexPage(IDealer objDealer, IBikeMakesCacheRepository<int> bikeMakesCache)
        {
            _objDealer = objDealer;
            _bikeMakesCache = bikeMakesCache;

        }

    }
}