
using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 30th Aug 2016
    /// Summary: Bind view model for binding Other used bikes in a city
    /// </summary>
    public class BindOtherUsedBikesForCity
    {

        public uint InquiryId { get; set; }
        public string CityName { get; set; }
        public string CityMaskingName { get; set; }

        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }

        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }

        public string BikeName { get; set; }

        public IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint InquiryId, uint CityId, ushort TopCount)
        {
            IEnumerable<OtherUsedBikeDetails> otherBikesinCity = default(IEnumerable<OtherUsedBikeDetails>);
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                    .RegisterType<IUsedBikeDetails, IUsedBikeDetailsRepository>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                var objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();
                otherBikesinCity = objCache.GetOtherBikesByCityId(InquiryId, CityId, TopCount);
            }

            this.CityName = otherBikesinCity.FirstOrDefault().RegisteredAt;
            this.CityMaskingName = otherBikesinCity.FirstOrDefault().CityMaskingName;
            return otherBikesinCity;
        }
    }
}