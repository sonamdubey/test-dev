using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Models.Compare;
using Bikewale.Notifications;
using Bikewale.Utility;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 09 May 2017
    /// Summary :- Model for Comparison Landing Page
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance for comparison list
    /// </summary>
    public class CompareIndex
    {
        private readonly IBikeCompare _compare = null;
        private readonly ICMSCacheContent _compareTest = null;

        private uint _cityId;
        public CompareIndex(IBikeCompare compare, ICMSCacheContent compareTest)
        {
            _compare = compare;
            _compareTest = compareTest;

        }
        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for get data for the whole page
        /// </summary>
        /// <returns></returns>
        public CompareVM GetData()
        {
            GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
            if (location != null)
                _cityId = location.CityId;
            CompareVM objVM = new CompareVM();
            BindCompareTest(objVM);
            BindCompareBike(objVM);
            BindMetas(objVM);
            objVM.Page = GAPages.CompareBikes_Landing;
            return objVM;

        }
        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for Bind metas
        /// </summary>
        /// <returns></returns>
        public void BindMetas(CompareVM objVM)
        {
            try
            {
                objVM.PageMetaTags.Title = "Compare Bikes | New Bikes Comparisons in India";
                objVM.PageMetaTags.Description = "Comparing Indian bikes was never this easy. BikeWale presents you the easiest way of comparing bikes. Choose two or more bikes to compare them head-to-head.";
                objVM.PageMetaTags.Keywords = "bikes compare, compare bikes, compare bikes, bike comparison, bikes comparison india";
                objVM.PageMetaTags.CanonicalUrl = "https://www.bikewale.com/comparebikes/";
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.CompareIndex.BindMetas()");
            }

        }
        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for Bind Compare Test
        /// </summary>
        /// <returns></returns>
        public void BindCompareTest(CompareVM objVM)
        {
            ComparisonTestWidget objCompare = new ComparisonTestWidget(_compareTest);
            try
            {
                objCompare.topCount = 3;
                objVM.ArticlesList = objCompare.GetData();
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.CompareIndex.BindCompareTest()");
            }

        }
        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :- Function for Bind Compare Bike
        /// </summary>
        /// <returns></returns>
        public void BindCompareBike(CompareVM objVM)
        {
            CompareWidget objCompare = new CompareWidget(_compare);
            try
            {
                objCompare.topCount = 9;
                objCompare.cityId = _cityId;
                objVM.CompareBikes = objCompare.GetData();
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.CompareIndex.BindCompareBike()");
            }
        }
    }
}