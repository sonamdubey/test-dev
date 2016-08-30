
using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
namespace Bikewale.BindViewModels.Controls
{
    public class BindSimilarUsedBikes
    {
        public ushort TotalRecords { get; set; }
        public uint InquiryId { get; set; }
        public uint CityId { get; set; }
        public string CityName { get; set; }
        public string CityMaskingName { get; set; }

        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }

        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }

        public string BikeName { get; set; }

        public IEnumerable<BikeDetailsMin> BindUsedSimilarBikes()
        {
            IEnumerable<BikeDetailsMin> similarBikeList = default(IEnumerable<BikeDetailsMin>);
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                    .RegisterType<IUsedBikeDetails, IUsedBikeDetailsRepository>()
                    .RegisterType<ICacheManager, MemcacheManager>();

                var objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();
                similarBikeList = objCache.GetSimilarBikes(InquiryId, CityId, ModelId, TotalRecords);
            }
            return similarBikeList;
        }

    }
}