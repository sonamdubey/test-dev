using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 15 Dec 2016
    /// Summary    : Widget to show number of service centers of all brands
    /// </summary>
    public class ServiceCentersByBrand : System.Web.UI.UserControl
    {
        protected IEnumerable<BrandServiceCenters> AllServiceCenters;
        public int makeId { get; set; }
        public int FetchedRecordsCount;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetAllServiceCenters();
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 15 Dec 2016
        /// Summary     : Function to get number of service centers of all brands
        /// </summary>
        private void GetAllServiceCenters()
        {
            try
            {
                BindOtherBrandsServiceCenters servicecentViewModel = new BindOtherBrandsServiceCenters();
                AllServiceCenters = servicecentViewModel.GetAllServiceCentersbyMake();
                if (AllServiceCenters != null && AllServiceCenters.Any())
                {
                    FetchedRecordsCount = AllServiceCenters.Count();
                    AllServiceCenters = AllServiceCenters.Where(v => v.MakeId != makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCentersByBrand.GetAllServiceCenters()");
                
            }
        }


    }
}