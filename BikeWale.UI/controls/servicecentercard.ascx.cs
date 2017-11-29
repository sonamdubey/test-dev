using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.Location;
using Bikewale.Entities.ServiceCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By :  Sangram Nandkhile on 09-Nov-2016
    /// Description : Service centers in a city card
    /// Modified By :-Subodh Jain on 1 Dec 2016
    /// Summary :- Added headerText and biLinetext
    /// </summary>
    public class ServiceCenterCard : UserControl
    {
        public uint ServiceCenterId { get; set; }
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public ushort TopCount { get; set; }
        public uint CityId { get; set; }
        public string widgetHeading { get; set; }
        public string biLineText { get; set; }
        public string makeName = string.Empty, cityName = string.Empty, cityMaskingName = string.Empty, makeMaskingName = string.Empty, widgetTitle = string.Empty;
        public string pageName { get; set; }
        public bool showWidget = false;


        public ServiceCenterData centerData;
        public IEnumerable<ServiceCenterDetails> ServiceCenteList;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (MakeId > 0)
                BindDealers();
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 09-Nov-2016
        /// Description : Function to bind service centers
        /// </summary>
        protected void BindDealers()
        {
            try
            {
                BindServiceCenter serviceViewModel = new BindServiceCenter();
                centerData = serviceViewModel.GetServiceCenterList((int)MakeId, CityId);
                if (centerData != null && centerData.ServiceCenters != null)
                {
                    IEnumerable<ServiceCenterDetails> totalList = centerData.ServiceCenters.Where(x => x.ServiceCenterId != ServiceCenterId);
                    if (totalList != null)
                    {
                        CityEntityBase cityObj = new CityHelper().GetCityById(CityId);
                        if(cityObj!=null)
                        cityMaskingName = cityObj.CityMaskingName;
                        ServiceCenteList = totalList.Take(TopCount);
                    }

                    if (centerData.Count > 0)
                        showWidget = true;

                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "ServiceCenterCard.BindDealers()");
                
            }
        }
    }
}