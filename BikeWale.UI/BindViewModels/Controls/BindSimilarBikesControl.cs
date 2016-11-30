using Bikewale.BAL.Compare;
using Bikewale.DAL.Compare;
using Bikewale.Entities.BikeData;
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
    /// </summary>
    public class BindSimilarCompareBikesControl
    {
        public uint FetchedRecordsCount { get; set; }
        public int cityid { get; set; }
        private Int64 SponsoredVersionId { get; set; }


        /// <summary>
        /// Created by:-Subodh Jain 12 sep 2016
        /// Description :- For comparison of popular bikes at model page
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
                    container.RegisterType<IBikeCompare, BikeCompareRepository>();
                    IBikeCompare objCompare = container.Resolve<IBikeCompare>();

                    CheckSponsoredBikeForAnyVersion(versionList);

                    if (SponsoredVersionId > 0)
                    {
                        objSimilarBikes = objCompare.GetSimilarCompareBikeSponsored(versionList, count, cityid, (uint)SponsoredVersionId);
                    }
                    else
                    {
                        objSimilarBikes = objCompare.GetSimilarCompareBikes(versionList, count, cityid);
                    }


                    if (objSimilarBikes != null)
                        FetchedRecordsCount = (uint)objSimilarBikes.Count();

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objSimilarBikes;
        }

        private void CheckSponsoredBikeForAnyVersion(string versionList)
        {

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeCompare, BikeComparison>();
                IBikeCompare objCompare = container.Resolve<IBikeCompare>();
                SponsoredVersionId = objCompare.GetFeaturedBike(versionList);
            }

        }
    }
}