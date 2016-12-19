﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 16 Dec 2016
    /// Summary    : Widget to show number of service centers of all brands
    /// </summary>
    public class ServiceCentersByBrand : System.Web.UI.UserControl
    {
        protected IEnumerable<BrandServiceCenters> AllServiceCenters;
        protected string WidgetTitle;
        public int makeId;
        public int FetchedRecordsCount;
        public string ClientIP { get { return Bikewale.Common.CommonOpn.GetClientIP(); } }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BindOtherBrandsServiceCenters servicecentViewModel = new BindOtherBrandsServiceCenters();
                AllServiceCenters = servicecentViewModel.GetAllServiceCentersbyMake();
                if (AllServiceCenters != null)
                {
                    FetchedRecordsCount = AllServiceCenters.Count();
                    AllServiceCenters = AllServiceCenters.Where(v => v.MakeId != makeId);
                }
                WidgetTitle = "Find Service Centers for other brands";
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersByBrand.Page_Load()");
                objErr.SendMail();
            }
        }
    }
}