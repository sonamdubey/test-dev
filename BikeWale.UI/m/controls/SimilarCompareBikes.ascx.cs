using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 12 May 2016
    /// Desc       : Created control to show similar Bike links below compare bikes 
    /// </summary>
    [System.Runtime.InteropServices.GuidAttribute("1B6B1D4A-0BE5-4276-881D-5CFA6E53B261")]
    public class SimilarCompareBikes : System.Web.UI.UserControl
    {
        public Repeater rptSimilarBikes, rptSimilarBikesInner;
        public string versionsList { get; set; }
        private uint _topCount = 0;
        public uint fetchedCount { get; set; }

        public IEnumerable<SimilarCompareBikeEntity> objSimilarBikes = null;

        public uint TopCount
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
        /// </summary>
        private void BindSimilarCompareBikes()
        {
            BindSimilarCompareBikesControl objAlt = new BindSimilarCompareBikesControl();

            try
            {
                objSimilarBikes = objAlt.BindAlternativeBikes(versionsList, TopCount);

                fetchedCount = (uint)objSimilarBikes.Count();

                if (fetchedCount > 0)
                {
                    var source = from bike in objSimilarBikes
                                 select new { VersionId = bike.VersionId1, BikeName = bike.Make1 + " " + bike.Model1 + " " + bike.Version1 };

                    rptSimilarBikes.DataSource = source.Distinct();
                    rptSimilarBikes.DataBind();
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

        public override void Dispose()
        {
            rptSimilarBikes.DataSource = null;
            rptSimilarBikes.Dispose();
            base.Dispose();
        }
    }
}