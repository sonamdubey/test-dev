using Bikewale.BindViewModels.Webforms.Compare;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.SEO;
using Bikewale.Mobile.Controls;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created By :  Sushil kumar on 2nd Feb 2017 
    /// Description : Bind compare bikes page with new logic and design
    /// </summary>
    public class CompareBike : System.Web.UI.Page
    {
        protected PageMetaTags pageMetas = null;
        protected CompareBikeMin ctrlCompareBikes;
        protected IEnumerable<BikeMakeEntityBase> objMakes = null;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            BindCompareBikes();
        }

        /// <summary>
        /// Created By :  Sushil kumar on 9nd Feb 2017 
        /// Description : Bind viewmodel data to page level variables for compare bikes section    
        /// </summary>
        private void BindCompareBikes()
        {
            CompareBikes objCompare = null;
            try
            {
                objCompare = new CompareBikes();
                objCompare.GetCompareBikeMakes();
                objMakes = objCompare.makes;
                pageMetas = objCompare.PageMetas;
                BindPageWidgets();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Mobile.New.CompareBike.BindCompareBikes");
                objCompare.isPageNotFound = true;
            }
            finally
            {
                if (objCompare.isPageNotFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }

        }

        /// <summary>
        /// Created By :  Sushil kumar on 9nd Feb 2017 
        /// Description : Bind page related widgets
        /// </summary>
        private void BindPageWidgets()
        {
            if (ctrlCompareBikes != null)
            {
                ctrlCompareBikes.TotalRecords = 3;
            }
        }

    }
}