using Bikewale.BindViewModels.Controls;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By :  Sangram Nandkhile on 09-Nov-2016
    /// Description : Service centers in a city card
    /// Modified By :-Subodh Jain on 5 Dec 2016 
    /// Summary:-added headertext and biLine Text
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
            if (CityId > 0 && string.IsNullOrEmpty(cityMaskingName))
                GetCityMaskingName(CityId);
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
                        ServiceCenteList = totalList.Take(TopCount);
                    if (centerData.Count > 0)
                        showWidget = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "ServiceCenterCard.BindDealers()");
                
            }
        }
        /// <summary>
        /// Created By:- Subodh Jain 22 Dec 2016
        /// Summary :- To get citymasking name
        /// </summary>
        /// <param name="maskingName"></param>
        /// <returns></returns>
        private void GetCityMaskingName(uint cityId)
        {
            IEnumerable<CityEntityBase> objCityList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>();
                    container.RegisterType<ICacheManager, MemcacheManager>(); ;
                    container.RegisterType<ICityCacheRepository, CityCacheRepository>();
                    ICityCacheRepository cityCacheRepository = container.Resolve<ICityCacheRepository>();
                    objCityList = cityCacheRepository.GetAllCities(EnumBikeType.All);


                    CityEntityBase SelectedCity = objCityList.FirstOrDefault(c => c.CityId == cityId);
                    if (SelectedCity != null)
                    {
                        cityMaskingName = SelectedCity.CityName;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, " GetCityMaskingName - model");
                
            }

        }
    }
}