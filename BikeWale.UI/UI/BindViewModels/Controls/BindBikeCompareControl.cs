using Bikewale.Cache.Compare;
using Bikewale.Cache.Core;
using Bikewale.DAL.Compare;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Bike Compare View Model
    /// Author  :   Sumit Kate
    /// Date    :   02 Sept 2015
    /// </summary>
    public class BindBikeCompareControl
    {
        /// <summary>
        /// Total records requested
        /// </summary>    
        public uint TotalRecords { get; set; }

        /// <summary>
        /// Total Fetched records
        /// </summary>    
        public int FetchedRecordCount { get; set; }

        public IEnumerable<TopBikeCompareBase> CompareList { get; set; }


        public TopBikeCompareBase FetchTopRecord()
        {
            if (FetchedRecordCount > 0)
            {
                if (CompareList != null)
                    return CompareList.First();
            }

            return null;
        }

        /// <summary>
        /// Bind the repeater to the Repeater
        /// </summary>
        /// <param name="repeater">Repeater object</param>
        public void BindBikeCompare(Repeater repeater, int skipCount = 0)
        {
            try
            {
                if (FetchedRecordCount > 0 && repeater != null && CompareList != null)
                {
                    repeater.DataSource = CompareList.Skip(skipCount);
                    repeater.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }

        public void FetchBikeCompares()
        {
            IEnumerable<TopBikeCompareBase> topBikeCompares = null;

            string apiUrl = String.Empty;
            FetchedRecordCount = 0;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeCompareCacheRepository, BikeCompareCacheRepository>();
                    container.RegisterType<IBikeCompare, BikeCompareRepository>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    var _objCompareCache = container.Resolve<IBikeCompareCacheRepository>();

                    topBikeCompares = _objCompareCache.CompareList(TotalRecords);
                }

                if (topBikeCompares != null && topBikeCompares.Any())
                {
                    FetchedRecordCount = topBikeCompares.Count();
                    CompareList = topBikeCompares;
                }
                else
                {
                    FetchedRecordCount = 0;
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }
        }
    }
}