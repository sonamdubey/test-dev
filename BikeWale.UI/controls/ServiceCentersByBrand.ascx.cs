﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 15 Dec 2016
    /// Summary    : Widget to show number of service centers of all brands
    /// </summary>
    public class ServiceCentersByBrand : System.Web.UI.UserControl
    {
        public IEnumerable<BrandServiceCenters> AllServiceCenters;
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
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersByBrand.Page_Load()");
                objErr.SendMail();
            }
        }


    }
}