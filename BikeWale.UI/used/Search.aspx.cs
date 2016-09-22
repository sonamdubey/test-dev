using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Mobile.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Bikewale.Used
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 9/8/2012
    /// </summary>
    public class Search : Page
    {

        SearchUsedBikes objUsedBikesPage = null;
        protected string pageTitle = string.Empty, pageDescription = string.Empty, pageKeywords = string.Empty, pageCanonical = string.Empty
                 , heading = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty, redirectUrl = string.Empty, alternateUrl = string.Empty;
        protected IEnumerable<UsedBikeBase> usedBikesList = null;
        protected IEnumerable<CityEntityBase> citiesList = null;
        protected IEnumerable<BikeMakeModelBase> makeModelsList = null;
        protected LinkPagerControl ctrlPager;
        protected ushort makeId;
        protected uint modelId, cityId;


        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadUsedBikesList();

        }

        private void LoadUsedBikesList()
        {
            objUsedBikesPage = new SearchUsedBikes();
            if (!objUsedBikesPage.IsPageNotFound)
            {
                objUsedBikesPage.BindSearchPageData();
                objUsedBikesPage.CreateMetas();
                pageTitle = objUsedBikesPage.pageTitle;
                pageDescription = objUsedBikesPage.pageDescription;
                pageKeywords = objUsedBikesPage.pageKeywords;
                pageCanonical = objUsedBikesPage.pageCanonical;
                alternateUrl = objUsedBikesPage.alternateUrl;
                citiesList = objUsedBikesPage.Cities;
                makeId = objUsedBikesPage.MakeId;
                modelId = objUsedBikesPage.ModelId;
                cityId = objUsedBikesPage.CityId;
                makeModelsList = objUsedBikesPage.MakeModels;
                usedBikesList = objUsedBikesPage.UsedBikes.Result;

                BindPagination();
            }
            else
            {
                Response.Redirect("/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }

        private void BindPagination()
        {
            // for RepeaterPager
            ctrlPager.MakeId = Convert.ToString(makeId);
            ctrlPager.CityId = cityId;
            ctrlPager.ModelId = Convert.ToString(modelId);
            //ctrlPager.PagerOutput = _pagerOutput;
            //ctrlPager.CurrentPageNo = _pageNo;
            //ctrlPager.TotalPages = objPager.GetTotalPages((int)objUsedBikesPage.TotalBikes, objUsedBikesPage._pa);
            //ctrlPager.BindPagerList();
        }

        #endregion
    }   // End of class
}   // End of namespace