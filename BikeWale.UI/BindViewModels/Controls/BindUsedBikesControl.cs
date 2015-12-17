using Bikewale.DTO.UsedBikes;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.DAL.UsedBikes;
using Bikewale.Entities.UsedBikes;
using Bikewale.Cache.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;

namespace Bikewale.BindViewModels.Controls
{
    public class BindUsedBikesControl
    {
        /// <summary>
        /// Total records requested
        /// </summary>
        public uint TotalRecords { get; set; }
        /// <summary>
        /// Total Fetched records
        /// </summary>
        public int FetchedRecordsCount { get; set; }               
        public int? CityId { get; set; }

        IEnumerable<PopularUsedBikesEntity> popularUsedBikes = null;

        public void BindRepeater(Repeater repeater)
        {
            FetchedRecordsCount = 0;

            try
            {
                FetchPopularUsedBike(TotalRecords, CityId);
                
                if (repeater != null && FetchedRecordsCount > 0)
                {
                    repeater.DataSource = popularUsedBikes;
                    repeater.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void FetchPopularUsedBike(uint topCount, int? cityId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {                    
                    container.RegisterType<IPopularUsedBikesCacheRepository, PopularUsedBikesCacheRepository>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IUsedBikes, UsedBikesRepository>();

                    IPopularUsedBikesCacheRepository _objUsedBikes = container.Resolve<IPopularUsedBikesCacheRepository>();

                    popularUsedBikes = _objUsedBikes.GetPopularUsedBikes(topCount, cityId);
                }

                if (popularUsedBikes != null)
                {
                    FetchedRecordsCount = popularUsedBikes.Count();
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }            
        }
    }
}