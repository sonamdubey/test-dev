using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Bikewale.Mobile.Service
{
    /// <summary>
    /// Created By  : Subodh Jain 08 nov 2016
    /// Summary :- Locate new bike dealers  
    /// </summary>
    public class LocateServiceCenter : Page
    {
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected BikeCare ctrlBikeCare;
        protected IEnumerable<BikeMakeEntityBase> TopMakeList;
        protected IEnumerable<BikeMakeEntityBase> OtherMakeList;
        protected IEnumerable<BikeMakeEntityBase> makes;
        protected IEnumerable<CityEntityBase> cities;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindMakes();
            ctrlBikeCare.TotalRecords = 3;
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            cityId = currentCityArea.CityId;
        }
        /// <summary>
        /// Created By:-Subodh Jain 8 nov 2016
        /// Submmary:- Bind Make for service center
        /// </summary>
        private void BindMakes()
        {

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    makes = objCache.GetMakesByType(EnumBikeType.ServiceCenter);
                    if (makes != null && makes.Count() > 0)
                    {

                        TopMakeList = makes.Take(6);
                        OtherMakeList = makes.Skip(6);

                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "LocateServiceCenter.BindMakes");
                objErr.SendMail();
            }
        }
    }
}