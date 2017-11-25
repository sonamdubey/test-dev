
using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public int FetchedRecordsCount { get; set; }

        /// <summary>
        /// Created by : Sangram Nandkhile on 30th Aug 2016
        /// Summary: To get similar used bikes by cityid
        /// Modified By :Sushil Kumar on 1st Sep 2016
        /// Description : Added null check and try catck block
        /// </summary>
        /// <param name="InquiryId"></param>
        /// <param name="cityId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint inquiryId, uint cityId, ushort topCount)
        {
            IEnumerable<OtherUsedBikeDetails> otherBikesinCity = null;
            try
            {
                if (inquiryId > 0 && cityId > 0)
                {
                    otherBikesinCity = default(IEnumerable<OtherUsedBikeDetails>);
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                            .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                        var objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();
                        otherBikesinCity = objCache.GetOtherBikesByCityId(inquiryId, cityId, topCount);

                        if (otherBikesinCity != null && otherBikesinCity.Any())
                        {
                            CityName = otherBikesinCity.FirstOrDefault().RegisteredAt;
                            CityMaskingName = otherBikesinCity.FirstOrDefault().CityMaskingName;
                            FetchedRecordsCount = otherBikesinCity.Count();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return otherBikesinCity;
        }
    }
}