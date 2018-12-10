using Carwale.Entity.Classified;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.BL.Classified.Stock;
using System.Configuration;
using Carwale.Interfaces.Elastic;
using Carwale.Entity.Elastic;
using Carwale.Entity.Enum;
using Carwale.Notifications.Logs;

namespace Carwale.BL.Elastic
{
    public class ProcessFilters : IProcessFilters
    {
        /// <summary>
        /// Class to process filters in Used car search page || Added By Jugal || 28 Oct 2014
        /// Elastic Search
        /// </summary>

        FilterInputs _filters;

        private ElasticOuptputs filterOutput;
        private const int _defaultPageNo = 1;

        /// <summary>
        /// Process All Filters of used car Search page selected by User || Added By Jugal On 28 Oct 2014
        /// </summary>
        /// <param name="_filters"></param>
        /// <returns></returns>
        public ElasticOuptputs ProcessFilterParams(FilterInputs inputs)
        {
            try
            {
                filterOutput = new ElasticOuptputs();
                _filters = inputs;
                //For filterBy
                if (!string.IsNullOrEmpty(_filters.filterby) || !String.IsNullOrEmpty(_filters.filterbyadditional))
                {
                    string[] filters;

                    //Added Condition for filterby and filterbyaddition entities
                    if (!String.IsNullOrEmpty(_filters.filterby))
                        filters = _filters.filterby.Split(' ');
                    else
                        filters = _filters.filterbyadditional.Split(' ');

                    for (int i = 0; i < filters.Length; i++)
                    {
                        if (filters[i] == ((int)FilterByEnum.CarsWithPhotos).ToString())
                        {
                            filterOutput.carsWithPhotos = "1";
                        }
                        else if (filters[i] == ((int)FilterByEnum.CarTradeCertifiedCars).ToString())
                        {
                            filterOutput.IsCarTradeCertifiedCars = true;
                        }
                        else if(filters[i] == FilterByEnum.FranchiseCars.ToString("D"))
                        {
                            filterOutput.IsFranchiseCars = true;
                        }
                    }
                }

                //For BodyType
                if (!string.IsNullOrEmpty(_filters.bodytype))
                {
                    string[] tempBodyStyle = null;
                    string temp = _filters.bodytype.Replace(" ", ",");
                    if (RegExValidations.ValidateCommaSeperatedNumbers(temp))
                    {
                        tempBodyStyle = _filters.bodytype.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                        if (tempBodyStyle.Contains<string>(Convert.ToString((int)CarBodyStyle.Sedan)))
                        {
                            List<string> lstTempBodyStyleId = tempBodyStyle.ToList<string>();
                            lstTempBodyStyleId.Add(Convert.ToString((int)CarBodyStyle.CompactSedan));
                            filterOutput.bodytypes = lstTempBodyStyleId.ToArray();
                        }
                        else
                        {
                            filterOutput.bodytypes = tempBodyStyle;
                        }
                    }
                }

                //For MakeId and ModelId
                if (!string.IsNullOrWhiteSpace(_filters.car))
                {
                    ProcessCar();
                }

                //For FuelType
                if (!string.IsNullOrEmpty(_filters.fuel))
                {
                    string temp = _filters.fuel.Replace(" ", ",");
                    if (RegExValidations.ValidateCommaSeperatedNumbers(temp))
                        filterOutput.fuels = _filters.fuel.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                }

                //For City
                //Modified By : Sadhana Upadhyay on 13 Apr 2015
                if (!string.IsNullOrEmpty(_filters.city))
                {
                    string tempCity = string.Empty;

                    if(_filters.city == "3000")
                    {
                        tempCity = ConfigurationManager.AppSettings["MumbaiAroundCityIds"];
                        filterOutput.multiCityId = 1;
                        filterOutput.multiCityName = "Mumbai";
                    }
                    else if (_filters.city == "3001")
                    {
                        tempCity = ConfigurationManager.AppSettings["DelhiNCRCityIds"];
                        filterOutput.multiCityId = 10;
                        filterOutput.multiCityName = "Delhi NCR";
                    }
                    else
                    {
                        
                        tempCity = _filters.city;
                    }

                    if (RegExValidations.ValidateCommaSeperatedNumbers(tempCity))
                    {
                        filterOutput.cities = tempCity.Split(',');
                    }
                }

                //For SellerType
                if (!string.IsNullOrEmpty(_filters.seller))
                {
                    if (_filters.seller.Contains("2"))      //To not show any results if individual filter is present (competitors follwing up with ind. sellers)
                    {
                        filterOutput.sellers = new string[] { "0" };
                        Logger.LogInfo($"Got individual seller filter. seller = {_filters.seller}");
                    }
                    else
                    {
                        string temp = _filters.seller.Replace(" ", ",");
                        if (RegExValidations.ValidateCommaSeperatedNumbers(temp))
                            filterOutput.sellers = _filters.seller.Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }

                //For OwnerType
                if (!string.IsNullOrEmpty(_filters.owners))
                {
                    if (_filters.owners.Contains("3"))
                        _filters.owners += " 4 5 7";
                    string temp = _filters.owners.Replace(" ", ",");

                    if (RegExValidations.ValidateCommaSeperatedNumbers(temp))
                        filterOutput.owners = _filters.owners.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                    
                }

                // For Transmission
                if (!string.IsNullOrEmpty(_filters.trans))
                {
                    string temp = _filters.trans.Replace(" ", ",");

                    if (RegExValidations.ValidateCommaSeperatedNumbers(temp))
                        filterOutput.transmissions = _filters.trans.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                }

                //For ColorType
                if (!string.IsNullOrEmpty(_filters.color))
                {
                    string temp = _filters.color.Replace(" ", ",");

                    if (RegExValidations.ValidateCommaSeperatedNumbers(temp))
                        filterOutput.colors = _filters.color.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                    if (filterOutput.colors != null)
                    {
                        List<string> listFilterOutputColors = filterOutput.colors.ToList();
                        if (listFilterOutputColors != null && listFilterOutputColors.Contains("13"))
                        {
                            listFilterOutputColors.Add("0");
                        }
                        filterOutput.colors = listFilterOutputColors.ToArray();                        
                    }
                }

                //For Budget Range
                if (!string.IsNullOrEmpty(_filters.budget))
                {
                    ProcessBudget(_filters.budget);
                }

                // For Kilometer Range
                if (!string.IsNullOrEmpty(_filters.kms))
                {
                    ProcessKms(_filters.kms);
                }

                //For Year Range
                if (!string.IsNullOrEmpty(_filters.year))
                {
                    ProcessMakeYear(_filters.year);
                }

                if (!string.IsNullOrEmpty(_filters.sc) && _filters.sc != "-1" && !string.IsNullOrEmpty(_filters.so) && _filters.so != "-1")
                {
                    filterOutput.sc = _filters.sc;
                    filterOutput.so = _filters.so;
                }

                filterOutput.pn = RegExValidations.IsPositiveNumber(_filters.pn) ? Convert.ToInt32(_filters.pn) : _defaultPageNo;
                
                //Added By : Sadhana Upadhyay on 11 Mar 2015 to check page size specified in query string 
                if (!String.IsNullOrEmpty(_filters.ps))
                {
                    filterOutput.ps = _filters.ps;
                }

                if (!String.IsNullOrEmpty(_filters.subSegmentID))
                {
                    filterOutput.subSegmentID = _filters.subSegmentID;
                }

                if (!String.IsNullOrEmpty(_filters.sessionId))
                {
                    filterOutput.sessionId = _filters.sessionId;
                }

                if (!String.IsNullOrEmpty(_filters.profileId))
                {
                    filterOutput.profileId = _filters.profileId;
                }

                if (_filters.lcr >= 0)
                {
                    filterOutput.lcr = _filters.lcr;
                }
                if (_filters.ldr >= 0)
                {
                    filterOutput.ldr = _filters.ldr;
                }
                if (_filters.lir >= 0)
                {
                    filterOutput.lir = _filters.lir;
                }               
                filterOutput.nearbyCityId = _filters.nearbyCityId;
                filterOutput.nearbyCityIds = _filters.nearbyCityIds;
                filterOutput.nearbyCityIdsStockCount = _filters.nearbyCityIdsStockCount;
                filterOutput.userPreferredRootId = _filters.userPreferredRootId;
                filterOutput.ShouldFetchNearbyCars = inputs.ShouldFetchNearbyCars;
                filterOutput.Latitude = inputs.Latitude;
                filterOutput.Longitude = inputs.Longitude;
                filterOutput.ExcludeStocks = inputs.ExcludeStocks?.Split(' ');
            }
            catch (SqlException ex)
            {
                var objErr = new ExceptionHandler(ex, "UsedSearch.ProcessFiltersParams()");
                objErr.LogException();
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "UsedSearch.ProcessFiltersParams()");
                objErr.LogException();
            }
            return filterOutput;
        }

        /// <summary>
        /// Processes selected car filters.
        /// Written By : Jugal Singh On 28-Oct-2014        
        /// </summary>
        private void ProcessCar()
        {
            if (_filters.car != null)
            {
                filterOutput.cars = _filters.car.Trim().Split(' ');
            }
            //_filters.car = "";
            string cars = string.Empty;
            string newRoots = string.Empty, newMakes = string.Empty;

            foreach (string str in filterOutput.cars)
            {
                if (str.Contains('.'))
                {
                    _filters.root += str.Split('.')[1] + " ";
                    newRoots += str.Split('.')[1] + " ";
                }
                else
                {
                    newMakes += str.Split('.')[0] + " ";
                }

                cars += str.Split('.')[0] + " ";
            }
            filterOutput.cars = cars.Split(' ');
            if (!string.IsNullOrEmpty(_filters.root))
            {
                filterOutput.roots = _filters.root.Split(' ');
                filterOutput.roots = filterOutput.roots.Take<string>(filterOutput.roots.Length - 1).ToArray<string>();
            }
            filterOutput.cars = filterOutput.cars.Take<string>(filterOutput.cars.Length - 1).ToArray<string>();

            if(!String.IsNullOrEmpty(newMakes))
            {
                filterOutput.NewMakes = newMakes.Split(' ');
                filterOutput.NewMakes = filterOutput.NewMakes.Take<string>(filterOutput.NewMakes.Length - 1).ToArray<string>();
            }

            if (!String.IsNullOrEmpty(newRoots))
            {
                filterOutput.NewRoots = newRoots.Split(' ');
                filterOutput.NewRoots = filterOutput.NewRoots.Take<string>(filterOutput.NewRoots.Length - 1).ToArray<string>();
            }
        }

        /// <summary>
        /// Processes budget filter. || Added By Jugal on 28 Oct 2014
        /// Modified By : Sadhana Upadhyay on 24 March 2015
        /// Summary : Increased Range as many listing get excluded during filtering
        /// </summary>
        /// <param name="budgetFilter"></param>
        private void ProcessBudget(string budgetFilter)
        {
            if (RegExValidations.ValidateDecimalRange(budgetFilter))
            {
                string[] budgetRange = budgetFilter.Split('-');
                filterOutput.budgetMax = "999999999999";
                if ((!string.IsNullOrEmpty(budgetRange[0])) && (!string.IsNullOrEmpty(budgetRange[1])))
                {
                    if (Convert.ToDouble(budgetRange[0]) <= Convert.ToDouble(budgetRange[1]))
                    {
                        filterOutput.budgetMin = (Convert.ToDouble(budgetRange[0]) * 100000).ToString();
                        filterOutput.budgetMax = (Convert.ToDouble(budgetRange[1]) * 100000).ToString();
                    }
                }

                if ((!string.IsNullOrEmpty(budgetRange[0])) && (string.IsNullOrEmpty(budgetRange[1])))
                {
                    filterOutput.budgetMin = (Convert.ToDouble(budgetRange[0]) * 100000).ToString();
                }
            }
        }

        /// <summary>
        /// Process Km filters selected by user. || Added By Jugal On 28 Oct 2014
        /// Modified By : Sadhana Upadhyay on 24 March 2015 
        /// Summary : increased km range as many listings has more range than this so those listing get excluded by this default range.
        /// </summary>
        /// <param name="kmsFilter"></param>
        private void ProcessKms(string kmsFilter)
        {
            if (RegExValidations.ValidateDecimalRange(kmsFilter))
            {
                string[] kmRange = kmsFilter.Split('-');
                filterOutput.kmMax = int.MaxValue.ToString();
                if ((!string.IsNullOrEmpty(kmRange[0])) && (!string.IsNullOrEmpty(kmRange[1])))
                {
                    if (Convert.ToDouble(kmRange[0]) <= Convert.ToDouble(kmRange[1]))
                    {
                        filterOutput.kmMin = (Convert.ToDouble(kmRange[0]) * 1000).ToString();
                        filterOutput.kmMax = (Convert.ToDouble(kmRange[1]) * 1000).ToString();
                    }
                }

                if ((!string.IsNullOrEmpty(kmRange[0])) && (string.IsNullOrEmpty(kmRange[1])))
                {
                    filterOutput.kmMin = (Convert.ToDouble(kmRange[0]) * 1000).ToString();
                }
            }

        }

        /// <summary>
        /// Process year filter selected by User. || Added By Jugal on 28 Oct 2014
        /// </summary>
        /// <param name="yearFilter"></param>
        private void ProcessMakeYear(string yearFilter)
        {
            if (RegExValidations.ValidateDecimalRange(yearFilter))
            {
                string[] yearRange = yearFilter.Split('-');
                filterOutput.yearMin = "1947";

                if ((!string.IsNullOrEmpty(yearRange[0])) && (!string.IsNullOrEmpty(yearRange[1])))
                {
                    if (Convert.ToInt32(yearRange[0]) <= Convert.ToInt32(yearRange[1]))
                    {
                        filterOutput.yearMin = Convert.ToInt32(GetMakeYear(yearRange[1])).ToString();
                    }
                }

                filterOutput.yearMax = Convert.ToInt32(GetMakeYear(yearRange[0])).ToString();
            }
        }

        private static string GetMakeYear(string yearRange)
        {
            return (DateTime.Today.Year - Convert.ToInt32(yearRange)).ToString();
        }

        public bool isStringExists(string subString, string parentString)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(parentString))
            {
                string[] arrStrings = parentString.Split(',');
                for (int i = 0; i < arrStrings.Length; i++)
                {
                    if (String.Compare(subString, arrStrings[i], true) == 0)
                    {
                        isValid = true;
                    }
                }
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }
    }

}
