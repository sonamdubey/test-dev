using Bikewale.DAL.UsedBikes;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace Bikewale.BAL.UsedBikes
{
    /// <summary>
    /// Created By : Sajal Gupta on 14/09/2016
    /// Description : Business logic to get used bikes for model/make page.
    /// </summary>
    public class UsedBikes : IUsedBikes
    {
        public IEnumerable<MostRecentBikes> GetPopularUsedBikes(uint makeId, uint modelId, uint cityId, uint totalCount)
        {
            IUsedBikesRepository _usedBikesRepository = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
                _usedBikesRepository = container.Resolve<IUsedBikesRepository>();
            }

            if (modelId != 0 && cityId != 0)
            {
                return _usedBikesRepository.GetUsedBikesbyModelCity(modelId, cityId, totalCount);
            }
            else if (modelId != 0 && cityId == 0)
            {
                return _usedBikesRepository.GetUsedBikesbyModel(modelId, totalCount);
            }
            else if (cityId != 0)
            {
                return _usedBikesRepository.GetUsedBikesbyMakeCity(makeId, cityId, totalCount);
            }
            else
            {
                return _usedBikesRepository.GetUsedBikesbyMake(makeId, totalCount);
            }
        }
    }
}


