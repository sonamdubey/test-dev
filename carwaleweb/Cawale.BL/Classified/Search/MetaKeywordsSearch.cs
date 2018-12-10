using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified.Search;
using Carwale.Utility;
using Carwale.Utility.Classified;
using System;
using System.Configuration;

namespace Carwale.BL.Classified.Search
{
    public class MetaKeywordsSearch : IMetaKeywordsSearch
    {
        private string hostUrl = string.Format("https://{0}",ConfigurationManager.AppSettings["HostUrl"]);
        private readonly ISearchUtility _searchUtility;

        public MetaKeywordsSearch(ISearchUtility searchUtility)
        {
            _searchUtility = searchUtility;
        }

        public MetaKeywords GetMetaKeywordsSearchPage(string makeName, string rootName, string cityName, int pageNo, int totalPages, bool isMsite = false)
        {
            MetaKeywords _metaKeywords = new MetaKeywords();
            string makeLowercase = Format.FormatURL(makeName);
            string cityLowercase = Format.FormatURL(cityName);
            string rootLowercase = Format.FormatURL(rootName);

            string baseUrl = _searchUtility.GetURL(makeLowercase, rootLowercase, cityLowercase);
            _metaKeywords.Canonical = $"{ hostUrl }{ baseUrl }{ (pageNo <= 1 ? string.Empty : $"page-{ pageNo }/")}";

            if (isMsite && pageNo == 1)
            {
                _metaKeywords.AmpUrl = $"{ hostUrl }/amp{ baseUrl }";
            }
            
            if (isMsite)
            {
                _metaKeywords.AltUrl = $"{ hostUrl }{ baseUrl }";
                hostUrl = $"{ hostUrl }/m";
            }
            else
            {
                _metaKeywords.AltUrl = $"{ hostUrl }/m{ baseUrl }";
            }

            if (pageNo > 1)
            {
                _metaKeywords.PreviousPageUrl = $"{ hostUrl }{ baseUrl }page-{ pageNo - 1 }/";
            }
            if (pageNo < totalPages)
            {
                _metaKeywords.NextPageUrl = $"{ hostUrl }{ baseUrl }page-{ pageNo + 1 }/";
            }


            if (!string.IsNullOrEmpty(makeName) && !string.IsNullOrEmpty(rootName) && !string.IsNullOrEmpty(cityName))
            {
                ///used/bmw-x3-cars-in-mumbai/
                _metaKeywords.PageTitle = string.Format("Used {0} {1} Cars in {2}", makeName, rootName, cityName);
                _metaKeywords.PageDescription = string.Format("Used {0} {1} Cars in {2}. Find good condition second hand {0} {1} in {2} for sale. Great deals on old {1} cars in {2} at CarWale.", makeName, rootName, cityName);
                _metaKeywords.SearchPageTitle = string.Format("Used {0} {1} in {2}, Second Hand {0} {1} in {2}", makeName, rootName, cityName);
                _metaKeywords.PageKeywords = string.Format("used {0} {1} cars in {2}, second hand {0} {1} cars for sale in {2}, pre owned {0} {1} cars in {2}, old {0} {1} cars in {2}, carwale", makeLowercase, rootLowercase, cityLowercase);
            }
            else if (!string.IsNullOrEmpty(makeName) && !string.IsNullOrEmpty(cityName))
            {
                ///used/bmw-cars-in-mumbai/
                _metaKeywords.PageTitle = string.Format("Used {0} Cars in {1}", makeName, cityName);
                _metaKeywords.PageDescription = string.Format("Used {0} Cars in {1}. Find genuine second hand {0} cars in {1}. Largest stock of old {0} cars for sale in {1} at CarWale.", makeName, cityName);
                _metaKeywords.SearchPageTitle = string.Format("Used {0} Cars in {1}, Old {0} Cars in {1}", makeName, cityName);
                _metaKeywords.PageKeywords = string.Format("used {0} cars in {1}, second hand {0} cars for sale in {1}, pre owned {0} cars in {1}, old {0} cars in {1}, carwale", makeLowercase, cityLowercase);
            }
            else if (!string.IsNullOrEmpty(makeName) && !string.IsNullOrEmpty(rootName))
            {
                ///used/bmw-x3-cars/
                _metaKeywords.PageTitle = string.Format("Used {0} {1} Cars", makeName, rootName);
                _metaKeywords.PageDescription = string.Format("Used {0} {1} in India. Find good condition second hand {0} {1} cars for sale. Get genuine old {0} {1} at CarWale.", makeName, rootName);
                _metaKeywords.SearchPageTitle = string.Format("Used {0} {1}, {0} {1} Second Hand Cars", makeName, rootName);
                _metaKeywords.PageKeywords = string.Format("used {0} {1} cars in india, second hand {0} {1} cars for sale, pre owned {0} {1} cars, old {0} {1} cars, carwale", makeLowercase, rootLowercase);
            }
            else if (!string.IsNullOrEmpty(makeName))
            {
                ///used/bmw-cars/
                _metaKeywords.PageTitle = string.Format("Used {0} Cars", makeName);
                _metaKeywords.PageDescription = string.Format("Used {0} Cars. Largest stock of second hand {0} cars in India. Genuine old {0} cars for sale at CarWale.", makeName);
                _metaKeywords.SearchPageTitle = string.Format("Used {0} Cars in India, {0} Cars for Sale", makeName);
                _metaKeywords.PageKeywords = string.Format("used {0} cars in india, second hand {0} cars for sale, pre owned {0} cars, old {0} cars, carwale", makeLowercase);
            }
            else if (!string.IsNullOrEmpty(cityName))
            {
                ///used/cars-in-mumbai/
                _metaKeywords.PageTitle = string.Format("Used Cars in {0}", cityName);
                _metaKeywords.PageDescription = string.Format("Used Cars in {0}. Find good condition second hand cars in {0}. Great prices on old cars for sale in {0} at CarWale.", cityName);
                _metaKeywords.SearchPageTitle = string.Format("Used Cars in {0}, Second Hand Cars in {0}", cityName);
                _metaKeywords.PageKeywords = string.Format("used cars in {0}, second hand cars for sale in {0}, pre owned cars in {0}, {0} used cars, old cars in {0}, carwale", cityLowercase);
            }
            else
            {
                //cars-for-sale/
                //- Search used cars available for sale in India
                _metaKeywords.PageTitle = "Used Cars - Search used cars available for sale in India";
                _metaKeywords.PageDescription = "Search Used Cars available for sale in India. Searching is very fast and easy, its highly sophisticated but still very simple to use.";
                _metaKeywords.SearchPageTitle = "Search Used Cars";
            }
            return _metaKeywords;
        }
    }
}
