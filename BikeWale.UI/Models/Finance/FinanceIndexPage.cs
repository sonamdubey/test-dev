using System;
using System.Web;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using Newtonsoft.Json;
using Bikewale.Models.Shared;
using Bikewale.Models.Finance;

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

		public FinanceIndexPageVM GetPwaData()
        {
			FinanceIndexPageVM objData = new FinanceIndexPageVM();
            try
            {
				BindPageMetas(objData.PageMetaTags);
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

		private void BindPageMetas(PageMetaTags objPage)
		{
			try
			{
				objPage.Title = "EMI Calculator | Calculate Bike Loan EMI - BikeWale";
				objPage.Keywords = "calculate emi, emi calculator, calculate loan, loan calculator, indian emi calculator, used bike emi, new bike emi, new bike emi calculator, used bike emi calculator";
				objPage.Description = "EMI Calculator for new and used Bike loans. Calculate accurate Bike loan emi in advanced and arrears finance modes.";
				objPage.CanonicalUrl = "https://www.bikewale.com/bike-loan-emi-calculator/";
				objPage.AlternateUrl = "https://www.bikewale.com/m/bike-loan-emi-calculator/";
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "FinanceIndexPage.BindMetas()");
			}
		}
        #endregion

    }

}