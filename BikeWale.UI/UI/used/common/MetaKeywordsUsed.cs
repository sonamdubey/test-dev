using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Used
{
    /// <summary>
    /// Created By  : Ashwini Todkar on 16 April 2014
    /// Summary     : Manages meta info for Used Bike Page (SEO)
    /// </summary>
    public class MetaKeywordsUsed
    {
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string CityId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string CityMaskingName { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }
        public string SearchPageTitle { get; set; }
        public string Canonical { get; set; }
        public string altUrL { get; set; }
        public string BaseURL { get; set; }


        /// <summary>
        ///     Written By : Ashwini Todkar on 16 April 2014
        ///     Summary : PopulateWhere will set meta info for the used bike search page.
        /// </summary>
        /// <param name="makeId">Not mandetory. Send empty if not available.</param>
        /// <param name="modelId">Not mandetory. Send empty if not available.</param>
        /// <param name="cityId">Not mandetory. Send empty if not available.</param>
        /// <param name="makeMaskingName">Not mandetory. Send empty if not available.</param>
        /// <param name="modelMaskingName">Not mandetory. Send empty if not available.</param>
        /// <param name="cityMaskingName">Not mandetory. Send empty if not available.</param>
        public void GetMetaKeywordsSearchPage(string makeId, string modelId, string cityId, string makeMaskingName, string modelMaskingName, string cityMaskingName, string pageNo)
        {

            if (!String.IsNullOrEmpty(makeId) && !String.IsNullOrEmpty(modelId) && !String.IsNullOrEmpty(cityId))
            {
                ///used/ktm-duke-bikes-in-mumbai/
                PageTitle = "Used " + makeMaskingName + " " + modelMaskingName + " bikes in " + cityMaskingName + " - Second Hand " + makeMaskingName + " " + modelMaskingName + " in " + cityMaskingName;
                PageDescription = "Used " + makeMaskingName + " " + modelMaskingName + " bikes in " + cityMaskingName + ". Find good condition, well maintained second-hand " + makeMaskingName + " " + modelMaskingName + " bikes for sale in " + cityMaskingName + ". Largest stock of genuine old bikes in " + cityMaskingName + ".";
                SearchPageTitle = "Used " + makeMaskingName + " " + modelMaskingName + " bikes in " + cityMaskingName;
                PageKeywords = "used bikes in " + cityMaskingName + ", find used bikes in " + cityMaskingName + ", buy used bikes " + cityMaskingName + ", search bikes, find bikes, bike listing, bike sale, bike sale in india, india bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki";
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = "https://www.bikewale.com/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-" + cityMaskingName + "/";
                    BaseURL = "https://www.bikewale.com/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-" + cityMaskingName + "/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-" + cityMaskingName + "/";
                }
                else
                {
                    Canonical = "https://www.bikewale.com/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-" + cityMaskingName + "/" + "page-" + pageNo + "/";
                    BaseURL = "https://www.bikewale.com/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-" + cityMaskingName + "/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-" + cityMaskingName + "/";
                }
            }
            else if (!String.IsNullOrEmpty(makeId) && !String.IsNullOrEmpty(cityId))
            {
                ///used/bmw-bikes-in-mumbai/
                PageKeywords = "used bikes in " + cityMaskingName + ", find used bikes in " + cityMaskingName + ", buy used bikes " + cityMaskingName + ", search bikes, find bikes, bike listing, bike sale, bike sale in india, india bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki";
                PageTitle = "Used " + makeMaskingName + " bikes in " + cityMaskingName + " - Second Hand " + makeMaskingName + " in " + cityMaskingName;
                PageDescription = "Used " + makeMaskingName + " bikes in " + cityMaskingName + ". Find good condition, well maintained second-hand " + makeMaskingName + " bikes for sale in " + cityMaskingName + ". Largest stock of genuine old bikes in " + cityMaskingName + ".";
                SearchPageTitle = "Used " + makeMaskingName + " bikes in " + cityMaskingName;
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = "https://www.bikewale.com/used/" + makeMaskingName + "-bikes-in-" + cityMaskingName + "/";
                    BaseURL = "https://www.bikewale.com/used/" + makeMaskingName + "-bikes-in-" + cityMaskingName + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + makeMaskingName + "-bikes-in-" + cityMaskingName + "/";
                }
                else
                {
                    Canonical = "https://www.bikewale.com/used/" + makeMaskingName + "-bikes-in-" + cityMaskingName + "/" + "page-" + pageNo + "/";
                    BaseURL = "https://www.bikewale.com/used/" + makeMaskingName + "-bikes-in-" + cityMaskingName + "/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + makeMaskingName + "-bikes-in-" + cityMaskingName + "/";
                }

            }
            else if (!String.IsNullOrEmpty(makeId) && !String.IsNullOrEmpty(modelId))
            {
                ///used/ktm-duke-bikes/
                PageKeywords = "used bikes in india, buy used bikes, search bikes, find bikes, bike listing, bike sale, bike sale in india, india bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki";
                PageTitle = "Used " + makeMaskingName + " " + modelMaskingName + " bikes in India - Second Hand " + makeMaskingName + " " + modelMaskingName + " bikes";
                PageDescription = "Used " + makeMaskingName + " " + modelMaskingName + " bikes in India. Find good condition, well maintained, second-hand " + makeMaskingName + " " + modelMaskingName + " bikes for sale. Largest stock of genuine old bikes in India.";
                SearchPageTitle = "Used " + makeMaskingName + " " + modelMaskingName + " bikes";
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = "https://www.bikewale.com/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-india/";
                    BaseURL = "https://www.bikewale.com/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-india/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-india/";
                }

                else
                {
                    Canonical = "https://www.bikewale.com/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-india/" + "page-" + pageNo + "/";
                    BaseURL = "https://www.bikewale.com/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-india/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + makeMaskingName + "-" + modelMaskingName + "-bikes-in-india/";
                }


            }
            else if (!String.IsNullOrEmpty(makeId))
            {
                ///used/bmw-bikes/
                PageKeywords = "used bikes in india, find used bikes in, buy used bikes, search bikes, find bikes, bike listing, bike sale, bike sale in india, india bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki";
                PageTitle = "Used " + makeMaskingName + " bikes - Second Hand " + makeMaskingName + " bikes in India";
                PageDescription = "Used " + makeMaskingName + " bikes in India. Find good condition, well maintained, second-hand " + makeMaskingName + " bikes for sale. Largest stock of genuine old bikes in India.";
                SearchPageTitle = "Used " + makeMaskingName + " bikes";
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = "https://www.bikewale.com/used/" + makeMaskingName + "-bikes-in-india/";
                    BaseURL = "https://www.bikewale.com/used/" + makeMaskingName + "-bikes-in-india/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + makeMaskingName + "-bikes-in-india/";
                }
                else
                {
                    Canonical = "https://www.bikewale.com/used/" + makeMaskingName + "-bikes-in-india/" + "page-" + pageNo + "/";
                    BaseURL = "https://www.bikewale.com/used/" + makeMaskingName + "-bikes-in-india/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + makeMaskingName + "-bikes-in-india/";
                }

            }
            else if (!String.IsNullOrEmpty(cityId))
            {
                ///used/bikes-in-mumbai/
                
                PageKeywords = "used bikes in " + cityMaskingName + ", find used bikes in " + cityMaskingName + ", buy used bikes " + cityMaskingName + ", search bikes, find bikes, bike listing, bike sale, bike sale in india, india bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki";
                PageTitle = "Used bikes in " + cityMaskingName + " - Second Hand bikes in " + cityMaskingName;
                PageDescription = "Used bikes in " + cityMaskingName + ". Find good condition, well maintained, second-hand bikes for sale in " + cityMaskingName + ". Largest stock of genuine old bikes in " + cityMaskingName + ".";
                SearchPageTitle = "Used bikes in " + cityMaskingName;
                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = "https://www.bikewale.com/used/" + "bikes-in-" + cityMaskingName + "/";
                    BaseURL = "https://www.bikewale.com/used/" + "bikes-in-" + cityMaskingName + "/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + "bikes-in-" + cityMaskingName + "/";
                }
                else
                {
                    Canonical = "https://www.bikewale.com/used/" + "bikes-in-" + cityMaskingName + "/" + "page-" + pageNo + "/";
                    BaseURL = "https://www.bikewale.com/used/" + "bikes-in-" + cityMaskingName + "/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/" + "bikes-in-" + cityMaskingName + "/";
                }

            }
            else
            {
                //bikes in all over india/
                PageKeywords = "used bikes in india, find used bikes in, buy used bikes, search bikes, find bikes, bike listing, bike sale, bike sale in india, india bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki";
                PageTitle = "Used bikes - Search used bikes available for sale in India";
                PageDescription = "Search Used bikes available for sale in India. Searching is very fast and easy, its highly sophisticated but still very simple to use.";
                SearchPageTitle = "Search Used bikes";

                if (pageNo == "" || pageNo == "1")
                {
                    Canonical = "https://www.bikewale.com/used/bikes-in-india/";
                    BaseURL = "https://www.bikewale.com/used/bikes-in-india/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/bikes-in-india/";
                }
                else
                {
                    Canonical = "https://www.bikewale.com/used/bikes-in-india/" + "page-" + pageNo + "/";
                    BaseURL = "https://www.bikewale.com/used/bikes-in-india/" + "page-";
                    altUrL = "https://www.bikewale.com/m/used/bikes-in-india/" + "page-" + pageNo + "/";
                }
            }
        }
    }
}//End of MetaKeywordsUsed