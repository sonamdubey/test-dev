﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 12 May 2016
    /// Desc       : Created control to show similar Bike links below compare bikes
    /// Modified By : Sushil Kumar on 2nd Dec 2016
    /// Description : Removed repeater logic and dind data using list object
    /// </summary>

    public class SimilarCompareBikes : System.Web.UI.UserControl
    {
        public string versionsList { get; set; }
        private ushort _topCount = 0;
        public uint fetchedCount { get; set; }
        public Int64 SponsoredVersionId { get; set; }
        public String FeaturedBikeLink { get; set; }

        public IEnumerable<SimilarCompareBikeEntity> objSimilarBikes = null;

        public ushort TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindSimilarCompareBikes();
        }
        /// <summary>
        /// Created by : Sangram Nandkhile on 12 May 2016
        /// Desc       : To bind similar bikes
        /// Modified By : Sushil Kumar on 2nd Dec 2016
        /// Description : MOved value into object of similar bikes
        /// </summary>
        private void BindSimilarCompareBikes()
        {

            try
            {
                BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();
                objSimilarBikes = objAlt.BindPopularCompareBikes(versionsList, TopCount);
                fetchedCount = objAlt.FetchedRecordsCount;

                if (fetchedCount > 0)
                {
                    var source = from bike in objSimilarBikes
                                 select new { VersionId = bike.VersionId1, BikeName = string.Format("{0} {1} {2}", bike.Make1, bike.Model1, bike.Version1) };
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Added By Vivek on 13-05-2016
        /// To bind child repeater
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IEnumerable<SimilarCompareBikeEntity> getChildData(string versionId)
        {

            IEnumerable<SimilarCompareBikeEntity> obj = null;

            try
            {
                obj = objSimilarBikes.Where(ss => ss.VersionId1 == versionId).Take(Convert.ToInt32(TopCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return obj;
        }
    }
}