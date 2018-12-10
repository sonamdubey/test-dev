using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Cache.Geolocation;
using Carwale.Cache.IPToLocation;
using Carwale.DAL.Geolocation;
using Carwale.DAL.IPToLocation;
using Carwale.Entity.IPToLocation;
using Carwale.Interfaces;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.IPToLocation;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carwale.UI.m.research
{
    public class PopUpCityMobile : System.Web.UI.Page
    {
        protected string CitySelectedbyIp = string.Empty, CityNameSelectedbyIp;

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
            GetCityByIP();
        }
        private void GetCityByIP()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IIPToLocation, Carwale.BL.IPToLocation.IPToLocation>()
                     .RegisterType<IIPToLocationRepository, IPToLocationRepository>()
                     .RegisterType<IIPToLocationCacheRepository, IPToLocationCacheRepository>()
                     .RegisterType<IGeoCitiesRepository, GeoCitiesRepository>()
                    .RegisterType<IGeoCitiesCacheRepository, GeoCitiesCacheRepository>()
                    .RegisterType<ICacheManager, CacheManager>();

            IIPToLocation iPToLocation = container.Resolve<IIPToLocation>();
            try
            {
                //CitySelectedbyIp = "1";
                //CityNameSelectedbyIp = "Mumbai";
                IPToLocationEntity objIPToLocationEntity = iPToLocation.GetCity();
                CitySelectedbyIp = objIPToLocationEntity.CityId.ToString();
                CityNameSelectedbyIp = objIPToLocationEntity.CityName;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetUserCity.GetCityByIP()");
                objErr.LogException();
            }
        }
    }
}