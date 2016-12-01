﻿using Bikewale.BAL.Compare;
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
        public Int64 SponsoredVersionId { get; set; }
        public String FeaturedBikeLink { get; set; }


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
                        FetchedRecordsCount = (uint)objSimilarBikes.Count();

                        if (SponsoredVersionId > 0)
                        {
                            var objFeaturedComparision = objSimilarBikes.FirstOrDefault(f => f.VersionId2 == SponsoredVersionId.ToString());
                            if (objFeaturedComparision != null)
                                FeaturedBikeLink = Bikewale.Utility.SponsoredComparision.FetchValue(objFeaturedComparision.ModelId2.ToString());
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objSimilarBikes;
        }

        public Int64 CheckSponsoredBikeForAnyVersion(string versionList)
        {
            Int64 featuredBikeId = -1;
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeCompare, BikeComparison>();
                    IBikeCompare objCompare = container.Resolve<IBikeCompare>();
                    featuredBikeId = objCompare.GetFeaturedBike(versionList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "CheckSponsoredBikeForAnyVersion");
                objErr.SendMail();
            }

            return featuredBikeId;
        }
    }
}