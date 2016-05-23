using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Cache.Core;
using Bikewale.Controls;
using Bikewale.Interfaces.Cache.Core;
using Enyim.Caching;
using Microsoft.Practices.Unity;

namespace Bikewale
{
    public partial class test : System.Web.UI.Page
    {        
        private bool _isMemcachedUsed;
		protected static MemcachedClient _mc = null;
        protected ModelPriceInNearestCities ctrlTopCityPrices;

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlTopCityPrices.ModelId = Convert.ToUInt32(99);
            ctrlTopCityPrices.CityId = 1;
            ctrlTopCityPrices.TopCount = 8;

            _isMemcachedUsed = bool.Parse(ConfigurationManager.AppSettings.Get("IsMemcachedUsed"));
			
            if (_mc == null)
			{
				InitializeMemcached();
			}

            if (_isMemcachedUsed)
            {
                string key = "";

                var cacheObject = _mc.Get(key);

                if (cacheObject != null)
                {
                    Response.Write("Given key '" + key + "' - object exists in the memcache.");
                }
                else
                {
                    Response.Write("Given key '" + key + "' - object do not exists in the memcache.");
                }
            }
            else
            {
                Response.Write("Memcached is not used. Please check the flag 'IsMemcachedUsed' in webconfig file.");
            }
        }

        private void InitializeMemcached()
        {
            _mc = new MemcachedClient("memcached");
        }
    }
}