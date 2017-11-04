using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.DAL.Compare;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 12 May 2016
    /// Desc       : View Model to bind and pass repeater data to control
    /// Modified by :Subodh Jain on 21 oct 2016
    /// Desc : Added cityid as parameter
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Removed unused methods and merged methods related to similar compare bikes,Added check to handle sponsored bikes
    /// </summary>
    public class BindSimilarCompareBikesControl
    {
        public uint FetchedRecordsCount { get; set; }
        public int cityid { get; set; }
        public Int64 SponsoredVersionId { get; set; }
        public String FeaturedBikeLink { get; set; }


        /// <summary>
        /// Created by:-Subodh Jain 12 sep 2016
        /// Description :- For comparison of popular bikes at model page
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : To check for sponsord bike for version and if sponsored use another cache method
        /// </summary>
        /// <param name="rptPopularCompareBikes"></param>
        /// <param name="versionList"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ICollection<SimilarCompareBikeEntity> BindPopularCompareBikes(string versionList, ushort count)
        {
            ICollection<SimilarCompareBikeEntity> objSimilarBikes = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IBikeCompareCacheRepository, BikeCompareCacheRepository>();
                    container.RegisterType<IBikeCompare, BikeCompareRepository>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    var objCompare = container.Resolve<IBikeCompareCacheRepository>();

                    if (SponsoredVersionId > 0)
                    {
                        objSimilarBikes = objCompare.GetSimilarCompareBikeSponsored(versionList, count, cityid, (uint)SponsoredVersionId);
                    }
                    else
                    {
                        objSimilarBikes = objCompare.GetSimilarCompareBikes(versionList, count, cityid);
                    }


                    if (objSimilarBikes != null)
                    {

                        if (SponsoredVersionId > 0)
                        {
                            var objFeaturedComparision = objSimilarBikes.FirstOrDefault(f => f.VersionId2 == SponsoredVersionId.ToString());
                        }

                        FetchedRecordsCount = (uint)objSimilarBikes.Count();
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("{0}_BindPopularCompareBikes_{1}_Cnt_{2}", HttpContext.Current.Request.ServerVariables["URL"], versionList, count));
                
            }

            return objSimilarBikes;
        }

    }
}