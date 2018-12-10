using Carwale.UI.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace Carwale.UI.Common
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 22 Feb 2013
    /// Summary : Class will hold the methods to return meta keywords for used cars section.
    /// </summary>
    public class MetaKeywordsUsed
    {
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string CityId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string City { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }
        public string SearchPageTitle { get; set; }
        public string Canonical { get; set; }
        public string altUrL { get; set; }
        public string BaseURL { get; set; }
        public string AmpURL { get; set; }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 22 Feb 2013
        ///     Summary : Method will set meta info for the used cars search page.
        /// </summary>
        /// <param name="makeId">Not mandetory. Send empty if not available.</param>
        /// <param name="modelId">Not mandetory. Send empty if not available.</param>
        /// <param name="cityId">Not mandetory. Send empty if not available.</param>
        /// <param name="make">Not mandetory. Send empty if not available.</param>
        /// <param name="model">Not mandetory. Send empty if not available.</param>
        /// <param name="city">Not mandetory. Send empty if not available.</param>

        public void GetMetaKeywordsSearchPage(string make, string city, string pageNo, string rootName, string stateName, string cityMaskingName)
        {
            string makeLowercase = (!string.IsNullOrEmpty(make) ? UrlRewrite.FormatSpecial(make) : string.Empty);
            string cityLowercase = (!string.IsNullOrEmpty(city) ? UrlRewrite.FormatSpecial(city) : string.Empty);
            string stateLowercase = (!string.IsNullOrEmpty(stateName) ? UrlRewrite.FormatSpecial(stateName) : string.Empty);
            string rootLowercase = (!string.IsNullOrEmpty(rootName) ? rootName.ToLower().Replace(" ", string.Empty) : string.Empty);
            string cityOrStateName = string.IsNullOrEmpty(stateName) ? city : $"{city} {stateName}";
            string cityOrStateForKeywords = GetCityAndStateValue(cityLowercase, stateLowercase, " ");
            string cityOrStateWithComma = GetCityAndStateValue(city, stateName, ", ");
            string cityOrStateWithSpace = GetCityAndStateValue(city, stateName, " ");

            if (!String.IsNullOrEmpty(make) && !String.IsNullOrEmpty(rootName) && !String.IsNullOrEmpty(cityOrStateName))
            {
                ///used/bmw-x3-cars-in-mumbai/
                PageTitle = ConcatenateStrings("Used ", make, " ", rootName, " Cars in ", cityOrStateWithComma);
                PageDescription = ConcatenateStrings("Used ", make, " ", rootName, " Cars in ", cityOrStateWithSpace, ". Find good condition second hand ", make, " ", rootName, " in ", cityOrStateWithSpace, " for sale. Great deals on old ", rootName, " cars in ", cityOrStateWithSpace, " at CarWale.");
                SearchPageTitle = ConcatenateStrings("Used ", make, " ", rootName, " in ", cityOrStateWithComma, ", Second Hand ", make, " ", rootName, " in ", cityOrStateWithComma);
                PageKeywords = ConcatenateStrings("used ", makeLowercase, " ", rootLowercase, " cars in ",
                    cityOrStateForKeywords
                    , ", second hand ", makeLowercase, " ", rootLowercase, " cars for sale in ",
                    cityOrStateForKeywords, 
                    ", pre owned ", makeLowercase, " ", rootLowercase, " cars in ",
                    cityOrStateForKeywords, 
                    ", old  ", makeLowercase, " ", rootLowercase, " cars in ",
                    cityOrStateForKeywords,
                    ", carwale");
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-", rootLowercase, "-cars-in-", cityMaskingName, "/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-", rootLowercase, "-cars-in-", cityMaskingName, "/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", makeLowercase, "-", rootLowercase, "-cars-in-", cityMaskingName, "/");
                    AmpURL = ConcatenateStrings("https://www.carwale.com/amp/used/", makeLowercase, "-", rootLowercase, "-cars-in-", cityMaskingName, "/");
                }
                else
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-", rootLowercase, "-cars-in-", cityMaskingName, "/", "page-", pageNo, "/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-", rootLowercase, "-cars-in-", cityMaskingName, "/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", makeLowercase, "-", rootLowercase, "-cars-in-", cityMaskingName, "/");
                }
            }
            else if (!String.IsNullOrEmpty(make) && !String.IsNullOrEmpty(cityOrStateName))
            {
                ///used/bmw-cars-in-mumbai/
                PageTitle = ConcatenateStrings("Used ", make, " Cars in ", cityOrStateWithComma);
                PageDescription = ConcatenateStrings("Used ", make, " Cars in ", cityOrStateWithSpace, ". Find genuine second hand ", make, " cars in ", cityOrStateWithSpace, ". Largest stock of old ", make, " cars for sale in ", cityOrStateWithSpace, " at CarWale.");
                SearchPageTitle = ConcatenateStrings("Used ", make, " Cars in ", cityOrStateWithComma, ", Old ", make, " Cars in ", cityOrStateWithComma);
                PageKeywords = ConcatenateStrings("used ", makeLowercase, " cars in ", cityOrStateForKeywords, ", second hand ", makeLowercase, " cars for sale in ", cityOrStateForKeywords, ", pre owned ", makeLowercase, " cars in ", cityOrStateForKeywords, ", old ", makeLowercase, " cars in ", cityOrStateForKeywords, ", carwale");

                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-cars-in-", cityMaskingName, "/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-cars-in-", cityMaskingName, "/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", makeLowercase, "-cars-in-", cityMaskingName, "/");
                    AmpURL = ConcatenateStrings("https://www.carwale.com/amp/used/", makeLowercase, "-cars-in-", cityMaskingName, "/");
                }
                else
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-cars-in-", cityMaskingName, "/", "page-", pageNo, "/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-cars-in-", cityMaskingName, "/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", makeLowercase, "-cars-in-", cityMaskingName, "/");
                }

            }
            else if (!String.IsNullOrEmpty(make) && !String.IsNullOrEmpty(rootName))
            {
                ///used/bmw-x3-cars/
                PageTitle = ConcatenateStrings("Used ", make, " ", rootName, " Cars");
                PageDescription = ConcatenateStrings("Used ", make, " ", rootName, " in India. Find good condition second hand ", make, " ", rootName, " cars for sale. Get genuine old ", make, " ", rootName, " at CarWale.");
                SearchPageTitle = ConcatenateStrings("Used ", make, " ", rootName, ", ", make, " ", rootName, " Second Hand Cars");
                PageKeywords = ConcatenateStrings("used ", makeLowercase, " ", rootLowercase, " cars in india, second hand ", makeLowercase, " ", rootLowercase, " cars for sale, pre owned ", makeLowercase, " ", rootLowercase, " cars, old ", makeLowercase, " ", rootLowercase, " cars, carwale");
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-", rootLowercase, "-cars/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-", rootLowercase, "-cars/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", makeLowercase, "-", rootLowercase, "-cars/");
                }

                else
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-", rootLowercase, "-cars/", "page-", pageNo, "/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-", rootLowercase, "-cars/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", makeLowercase, "-", rootLowercase, "-cars/");
                }


            }
            else if (!String.IsNullOrEmpty(make))
            {
                ///used/bmw-cars/
                PageTitle = ConcatenateStrings("Used ", make, " Cars");
                PageDescription = ConcatenateStrings("Used ", make, " Cars. Largest stock of second hand ", make, " cars in India. Genuine old ", make, " cars for sale at CarWale.");
                SearchPageTitle = ConcatenateStrings("Used ", make, " Cars in India, ", make, " Cars for Sale");
                PageKeywords = ConcatenateStrings("used ", makeLowercase, " cars in india, second hand ", makeLowercase, " cars for sale, pre owned ", makeLowercase, " cars, old ", makeLowercase, " cars, carwale");
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-cars/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-cars/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", makeLowercase, "-cars/");
                }
                else
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-cars/", "page-", pageNo, "/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", makeLowercase, "-cars/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", makeLowercase, "-cars/");
                }

            }
            else if (!String.IsNullOrEmpty(cityOrStateName))
            {
                ///used/cars-in-mumbai/
                PageTitle = ConcatenateStrings("Used Cars in ", cityOrStateWithComma);
                PageDescription = ConcatenateStrings("Used Cars in ", cityOrStateWithSpace, 
                    ". Find good condition second hand cars in ", cityOrStateWithSpace, 
                    ". Great prices on old cars for sale in ", cityOrStateWithSpace, 
                    " at CarWale.");
                SearchPageTitle = ConcatenateStrings(" Used Cars in ", cityOrStateWithComma, ", Second Hand Cars in ", cityOrStateWithComma);
                PageKeywords = ConcatenateStrings("used cars in ", cityOrStateForKeywords, 
                    ", second hand cars for sale in ", cityOrStateForKeywords, 
                    ", pre owned cars in ", cityOrStateForKeywords, 
                    ", ", cityOrStateForKeywords, 
                    " used cars, old cars in ", cityOrStateForKeywords, 
                    ", carwale");
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", "cars-in-", cityMaskingName, "/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", "cars-in-", cityMaskingName, "/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", "cars-in-", cityMaskingName, "/");
                    AmpURL = ConcatenateStrings("https://www.carwale.com/amp/used/", "cars-in-", cityMaskingName, "/");
                }
                else
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/", "cars-in-", cityMaskingName, "/", "page-", pageNo, "/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/", "cars-in-", cityMaskingName, "/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/", "cars-in-", cityMaskingName, "/");
                }

            }
            else
            {
                //cars-for-sale/
                //- Search used cars available for sale in India
                PageTitle = ConcatenateStrings("Used Cars - Search used cars available for sale in India");
                PageDescription = ConcatenateStrings("Search Used Cars available for sale in India. Searching is very fast and easy, its highly sophisticated but still very simple to use.");
                SearchPageTitle = ConcatenateStrings("Search Used Cars");
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/cars-for-sale/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/cars-for-sale/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/cars-for-sale/");
                }
                else
                {
                    Canonical = ConcatenateStrings("https://www.carwale.com/used/cars-for-sale/", "page-", pageNo, "/");
                    BaseURL = ConcatenateStrings("https://www.carwale.com/used/cars-for-sale/", "page-");
                    altUrL = ConcatenateStrings("https://www.carwale.com/m/used/cars-for-sale/", "page-", pageNo, "/");
                }
            }
        }// End of GetMetaKeywordsSearchPage method

        private static String ConcatenateStrings(params string[] listOfString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String s in listOfString)
                sb.Append(s);
            return sb.ToString();

        }

        private static string GetCityAndStateValue(string city, string state , string delimiter)
        {
            return string.IsNullOrEmpty(state) ? city : $"{city}{delimiter}{state}";
        }
    }   // End of class
}   // End of namespace