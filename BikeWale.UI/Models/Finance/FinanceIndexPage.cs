using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Models.BestBikes;
using Bikewale.Models.BikeModels;
using Bikewale.Models.Scooters;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using Newtonsoft.Json;
using Bikewale.Models.Shared;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  : Rajan Chauhan on 28 Mar 2018
    /// Description : Created FinanceIndexPage
    /// </summary>
    public class FinanceIndexPage
    {
        #region Variables for dependency injection and constructor
        private readonly IPWACMSCacheRepository _renderedArticles;
        #endregion

        #region Page level variables
        private GlobalCityAreaEntity currentCityArea = null;
        private string CityName;
        private uint CityId;
        public string redirectUrl;
        public StatusCodes status;
        #endregion

        #region Constructor
        /// <summary>
        /// Created by : Rajan Chauhan on 28 Mar 2018
        /// </summary>
        public FinanceIndexPage(IPWACMSCacheRepository renderedArticles)
        {
            _renderedArticles = renderedArticles;
            ProcessCityArea();
        }

        #endregion

        #region Functions
        /// <summary>
        /// Created by : Rajan Chauhan on 28 Mar 2018
        /// Description : Method to get global city Id and Name from cookie.
        /// </summary>
        private void ProcessCityArea()
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                {
                    CityId = currentCityArea.CityId;
                    CityName = currentCityArea.City;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.News.NewsIndexPage.ProcessCityArea");
            }
        }

        public PwaBaseVM GetPwaData()
        {
            PwaBaseVM objData = new PwaBaseVM();
            try
            {

                objData.ReduxStore = new PwaReduxStore();
                var storeJson = JsonConvert.SerializeObject(objData.ReduxStore);

                objData.ServerRouterWrapper = _renderedArticles.GetNewsListDetails(PwaCmsHelper.GetSha256Hash(storeJson), objData.ReduxStore.News.NewsArticleListReducer,
                    "/m/finance/", "root", "ServerRouterWrapper");
                objData.WindowState = storeJson;
                objData.Page = Entities.Pages.GAPages.Other;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Finance.FinanceIndexPage.GetPwaData");
            }
            return objData;
        }
        #endregion

    }

}