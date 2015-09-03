using Bikewale.Common;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.New;
using System.Data;

namespace Bikewale.Mobile.Controls
{
    public class CompareBikeMin_old : System.Web.UI.UserControl
    {
        DataSet ds = null;
        protected Repeater rptCompareList;

        protected UInt16 _topCount = 4;
        public UInt16 TopCount
        {
            get { return _topCount; }
            set { value = _topCount; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComparison();
            }
        }//pageload

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : To bind featured Compare Bike list
        /// </summary>
        private void BindComparison()
        {
            try
            {
                CompareBikes cb = new CompareBikes();

                ds = cb.GetComparisonBikeList(TopCount);

                rptCompareList.DataSource = ds;
                rptCompareList.DataBind();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of BindComparison
    }
}