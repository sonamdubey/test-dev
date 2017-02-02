using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 25 Jan 2017
    /// Summary    : Bind list of top popular bikes by category
    /// </summary>
    public class BindPopularBikesByBodyStyle
    {
        public int TopCount { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public int FetchedRecordsCount { get; set; }

        public ICollection<MostPopularBikesBase> GetPopularBikesByCategory()
        {
            ICollection<MostPopularBikesBase> popularBikesList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();

                    IBikeModelsCacheRepository<int> modelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                    if (ModelId > 0)
                    {
                        BodyStyle = modelCache.GetBikeBodyType(ModelId);
                        popularBikesList = modelCache.GetPopularBikesByBodyStyle((int)BodyStyle, TopCount, CityId);
                        if (popularBikesList != null && popularBikesList.Count() > 0)
                            FetchedRecordsCount = popularBikesList.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BindPopularBikesByBodyStyle.GetPopularBikesByCategory ModelId: {0}", ModelId));
            }
            return popularBikesList;
        }
    }
}