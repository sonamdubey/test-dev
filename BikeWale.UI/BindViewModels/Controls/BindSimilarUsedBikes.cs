
using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 30th Aug 2016
    /// Summary: Bind view model for binding similar used bikes
    /// </summary>
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
        public int FetchedRecordsCount { get; set; }
        public string BikeName { get; set; }

        /// <summary>
        /// Created by : Sangram Nandkhile on 30th Aug 2016
        /// Summary: To get similar used bikes for particular model
        /// </summary>
        /// <param name="InquiryId"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeDetailsMin> BindUsedSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount)
        {
            IEnumerable<BikeDetailsMin> similarBikeList = null;
            try
            {
                similarBikeList = default(IEnumerable<BikeDetailsMin>);
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                        .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();
                    similarBikeList = objCache.GetSimilarBikes(inquiryId, cityId, modelId, topCount);
                    if (similarBikeList != null)
                    {
                        FetchedRecordsCount = similarBikeList.Count();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return similarBikeList;
        }


    }
}