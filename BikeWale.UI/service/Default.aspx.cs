using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;


namespace Bikewale.Service
{
    /// <summary>
    /// Created By:-Subodh Jain 16 nov 2016
    /// Submmary:- Landing Page service center
    /// </summary>
    public class Default : Page
    {
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected IEnumerable<BikeMakeEntityBase> TopMakeList;
        protected IEnumerable<BikeMakeEntityBase> OtherMakeList;
        protected IEnumerable<BikeMakeEntityBase> makes;

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

            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();
            BindMakes();
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

                        TopMakeList = makes.Take(10);
                        OtherMakeList = makes.Skip(10);

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