using Carwale.BL.Elastic;
using Carwale.BL.Interface.Stock.Search;
using Carwale.BL.Stock;
using Carwale.DAL.Customers;
using Carwale.DTOs.Classified.Stock;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Search;
using Carwale.Entity.Dealers;
using Carwale.Entity.Geolocation;
using Carwale.Entity.IPToLocation;
using Carwale.Entity.Stock.Search;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.Search;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.IPToLocation;
using Carwale.Notifications;
using Carwale.Service;
using Carwale.UI.Common;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace Carwale.UI.Used
{
    public partial class Search : System.Web.UI.Page
    {

        #region constants
        private const string c_strURL = "URL";
        private const string c_strPageNumber = "pn";
        private const string c_strCity = "city";
        private const string c_strPlus = "+";
        private const string c_strUsedRedirectURL = "/used/";
        private const string c_strMumbaiCityCode1 = "1";
        private const string c_strMumbaiCityCode3000 = "3000";
        private const string c_strDelhiNCRCode3001 = "3001";
        private const string c_strModel = "model";
        private const string c_strIsSold = "issold";
        private const string c_strMake = "make";
        private const string c_strZero = "0";
        private const string c_strOneHyphen = "1-";
        private const string c_strOne = "1";
        private const string c_strNextURLFor1stPage = "{0}2/";
        private const string c_strNextPrevBaseURL = "{0}{1}/";

        private const string c_strRedirectURLValidCity = "/used/{0}-{1}-cars-in-{2}/{3}";
        private const string c_strRedirectURLNullCity = "/used/{0}-{1}-cars/";
        private const string c_strRedirectURLValidCityAndUnknownMake = "/used/cars-in-{0}/{1}";
        private const string c_strRedirectURLForSoldCars = "/used/cars-for-sale/{0}";
        #endregion

        #region static_readonly
        private readonly static string s_strElasticIndexName;
        private readonly IEnumerable<int> _ampCities = ConfigurationManager.AppSettings["UsedAmpCities"].Split(',').Select(int.Parse);

        private IIPToLocation _iPToLocation;
        private ICarModelCacheRepository _carModelCacheRepository;
        private ICarMakesCacheRepository _carMakesCacheRepository;
        private IElasticSearchManager _elasticSearchManager;
        private ICarModelRootsCacheRepository _modelRootsCacheRepository;
        private IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private ICommonOperationsRepository _commonOperationRepo;
        private ClassifiedSearchCommon _classifiedSearchCommon;
        private ISearchBL _searchBL;
        private ISearchUtility _searchUtility;
        private IGeoCitiesCacheRepository _cityRepo;
        private IStockSearchLogic<SearchResultDesktop> _stockSearchLogic;
        #endregion

        #region datamembers
        protected string cityName = string.Empty;
        protected string stateName = string.Empty;
        protected string cityMaskingName = string.Empty;
        protected string makeName = string.Empty;
        protected string modelName = string.Empty;
        protected string title = string.Empty;
        protected string pageKeywords = string.Empty;
        protected string maskingName = string.Empty;
        protected string pageTitle = string.Empty;
        protected string pageDescription = string.Empty;
        protected string canonicalUS = string.Empty;
        protected string nextPrevBaseUrl = string.Empty;
        protected string altURL = string.Empty;
        protected string ampURL = string.Empty;
        protected string redirectUrl = string.Empty;
        protected string jsonData = string.Empty;
        protected string recordRange = string.Empty;
        protected string soldOut = string.Empty;
        protected string prev = string.Empty;
        protected string next = string.Empty;
        protected string makeId = string.Empty;
        protected string modelId = string.Empty;
        protected string rootId = string.Empty;
        protected string nearByCity = string.Empty;
        protected bool redirect = false;
        protected int totalCount = 0;
        protected int pageNo = 1;
        protected int filtersCount = 0;
        protected int pageSize = 24;
        protected int indexFeatured = 0;
        protected int indexAbsolute = 0;
        protected int indexNonFeatured = 0;
        protected bool showCityWarning = true;
        protected bool fromCW = true;
        protected FilterInputs filters = new FilterInputs();
        protected string ipDetectedCityName = string.Empty;
        protected int ipDetectedCityId = 0;
        protected int appliedIpDetectedCityId = -1;
        private String qsParameters = String.Empty;
        protected int latestNonPremiumRank = 0;
        protected int latestPremiumDealerRank = 0;
        protected int latestPremiumIndividualRank = 0;
        protected string rootName = string.Empty;
        protected bool isRoot = false;
        protected static int certProgId = 0;
        protected int cityId;
        protected static string certProgLogoUrl = string.Empty;
        protected bool IsStockFranchise;
        protected bool IsOriginalImageAvailable;
        protected readonly int carTradeCertificationId = Convert.ToInt32(ConfigurationManager.AppSettings["CartradeCertificationId"].ToString());
        protected string excludeStocks = string.Empty;
        protected string UrlWithSlotAndRank;
        #endregion

        #region constructor
        static Search()
        {
            s_strElasticIndexName = ConfigurationManager.AppSettings["ElasticIndexName"];
        }
        #endregion

        #region methods
        protected void Page_Load(object sender, EventArgs e)
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _iPToLocation = container.Resolve<IIPToLocation>();
                _carModelCacheRepository = container.Resolve<ICarModelCacheRepository>();
                _carMakesCacheRepository = container.Resolve<ICarMakesCacheRepository>();
                _elasticSearchManager = container.Resolve<IElasticSearchManager>();
                _modelRootsCacheRepository = container.Resolve<ICarModelRootsCacheRepository>();
                _geoCitiesCacheRepo = container.Resolve<IGeoCitiesCacheRepository>();
                _commonOperationRepo = container.Resolve<ICommonOperationsRepository>();
                _searchBL = container.Resolve<ISearchBL>();
                _searchUtility = container.Resolve<ISearchUtility>();
                _cityRepo = container.Resolve<IGeoCitiesCacheRepository>();
                _stockSearchLogic = container.Resolve<IStockSearchLogic<SearchResultDesktop>>();
            }
            _classifiedSearchCommon = new ClassifiedSearchCommon();
            if (!IsPostBack)
            {
                DetectDevice();
                if (Request.QueryString[c_strModel] != null && Regex.IsMatch(Request.QueryString[c_strModel].ToString(), @"^\d+(,\d+)*$"))    //if qs contains model then redirect it as model base filter
                {
                    redirectUrl = $"/used/cars-for-sale/{ _searchBL.GetRedirectQsByModelIds(Request.QueryString) }";
                    _classifiedSearchCommon.RedirectToNewURL(redirectUrl, Response);
                }
                GetPageNo();
                ExtractQSParametersFromUrl();
                GetSetCityId();
                GetRootName();
                if (!String.IsNullOrEmpty(rootName))
                {
                    GetMakeDetailsByRootName(rootName);
                }
                if (!isRoot)
                {
                    GetModelId();
                }
                GetCompleteQS();
                ProcessRedirect();
                GetListingIndexesForCurrentPage();
                GetMetaKeywords();
                GetCompleteData();
                GetSoldOutQS();
                GenerateBreadCrumb();
                BindMakes();
                CheckUserCredentials();
                GetNextPrevUrl();
            }
        }

        private void GetMakeDetailsByRootName(string root)
        {
            CarModelMaskingResponse carModelMaskingResponse = null;
            try
            {
                carModelMaskingResponse = _classifiedSearchCommon.GetMakeDetailsByRootName(root);
                if (carModelMaskingResponse != null)
                {
                    makeName = carModelMaskingResponse.MakeName;
                    rootName = carModelMaskingResponse.RootName;
                    rootId = carModelMaskingResponse.RootId.ToString();
                    isRoot = true;
                }
            }
            catch (Exception ex)
            {
                SendErrorMail(ex, "GetMakeDetailsByRootName");
            }
        }

        private void GetRootName()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["root"]))
            {
                rootName = Request.QueryString["root"];
            }
        }

        private void ExtractQSParametersFromUrl()
        {
            if (Request.RawUrl.IndexOf('?') > -1)
                qsParameters = Request.RawUrl.Substring(Request.RawUrl.IndexOf('?') + 1);
        }

        protected void GenerateBreadCrumb()
        {
            string result = string.Empty;
            try
            {
                bool isCityNameNullOrEmpty = string.IsNullOrEmpty(cityName);
                bool isMakeNameNullOrEmpty = string.IsNullOrEmpty(makeName);
                bool isRootNameNullOrEmpty = string.IsNullOrEmpty(rootName);

                if (isCityNameNullOrEmpty && isMakeNameNullOrEmpty && isRootNameNullOrEmpty)
                {
                    result = "<li><a href='/'>Home</a></li><li> &nbsp <span class='fa fa-angle-right margin-right10'> </span> <a href='/used/'>Used Cars</a></li><li>&nbsp <span class='fa fa-angle-right margin-right10'> </span> <span>Search Used Cars </span></li>";
                }
                else if (isMakeNameNullOrEmpty && isRootNameNullOrEmpty)
                {
                    result = string.Format("<li><a href='/'>Home</a></li><li> &nbsp <span class='fa fa-angle-right margin-right10'> </span> <a href='/used/'>Used Cars</a></li><li>&nbsp <span class='fa fa-angle-right margin-right10'> </span> <span>Used Cars in {0}</span></li>", cityName);
                }
                else if (isCityNameNullOrEmpty && isRootNameNullOrEmpty)
                {
                    result = string.Format("<li><a href='/'>Home</a></li><li>&nbsp <span class='fa fa-angle-right margin-right10'> </span> <a href='/used/'>Used Cars</a></li><li> &nbsp<span class='fa fa-angle-right margin-right10'> </span> <span>Used  {0} Cars</span></li>", makeName);
                }
                else if (isCityNameNullOrEmpty)
                {
                    result = string.Format("<li><a href='/'>Home</a></li><li>&nbsp <span class='fa fa-angle-right margin-right10'> </span> <a href='/used/'>Used Cars</a></li><li> &nbsp<span class='fa fa-angle-right margin-right10'> </span> <a href='/used/{0}-cars/'>{1}</a></li><li> &nbsp <span class='fa fa-angle-right margin-right10'> </span><span>Used {1} {2} Cars</span></li>", UrlRewrite.FormatSpecial(makeName), makeName, rootName);
                }
                else if (isRootNameNullOrEmpty)
                {
                    result = string.Format("<li><a href='/'>Home</a></li><li>&nbsp <span class='fa fa-angle-right margin-right10'> </span> <a href='/used/'>Used Cars</a></li><li>&nbsp <span class='fa fa-angle-right margin-right10'> </span> <a href='/used/cars-in-{0}/'>{1}</a></li><li>  &nbsp <span class='fa fa-angle-right margin-right10'> </span><span>Used {2} Cars in {1}</span></li>", UrlRewrite.FormatSpecial(cityName), cityName, rootName);
                }
                else
                {
                    result = string.Format("<li><a href='/'>Home</a></li><li> &nbsp <span class='fa fa-angle-right margin-right10'> </span> <a href='/used/'>Used Cars</a></li><li>&nbsp <span class='fa fa-angle-right margin-right10'> </span> <a href='/used/cars-in-{0}/'>{1}</a></li><li>  &nbsp <span class='fa fa-angle-right margin-right10'> </span><span>Used {2} {3} in {1}</span></li>", UrlRewrite.FormatSpecial(cityName), cityName, makeName, rootName);
                }
            }
            catch (Exception ex)
            {
                SendErrorMail(ex, "BreadCrumb");
            }
            breadCrumb.InnerHtml = result;
        }

        protected void GetMetaKeywords()
        {
            try
            {
                MetaKeywordsUsed objKeywords = new MetaKeywordsUsed();
                if (!isRoot)
                {
                    if (!String.IsNullOrEmpty(modelId) && Convert.ToInt32(modelId) > 0)
                        GetMakeModelName();
                    else
                        GetMakeName();
                }

                string carFilter = filters.car;
                if (!String.IsNullOrEmpty(carFilter) && carFilter.Contains(c_strPlus))
                    objKeywords.GetMetaKeywordsSearchPage(string.Empty, cityName, pageNo.ToString(), string.Empty, stateName, cityMaskingName);
                else
                    objKeywords.GetMetaKeywordsSearchPage(makeName, cityName, pageNo.ToString(), rootName, stateName, cityMaskingName);

                pageTitle = objKeywords.PageTitle;
                pageDescription = objKeywords.PageDescription;
                title = objKeywords.SearchPageTitle;
                canonicalUS = objKeywords.Canonical;
                nextPrevBaseUrl = objKeywords.BaseURL;
                altURL = objKeywords.altUrL;
                ampURL = objKeywords.AmpURL;
                pageKeywords = objKeywords.PageKeywords;

                if (!_ampCities.Contains(cityId))
                {
                    ampURL = null;
                }
            }
            catch (Exception ex)
            {
                SendErrorMail(ex, "GetMetaKeywords");
            }
        }

        protected void GetPageNo()
        {
            string pgNo = Request.QueryString[c_strPageNumber];
            if (!String.IsNullOrEmpty(pgNo))
            {
                pageNo = Convert.ToInt32(pgNo);
                filters.pn = pgNo;
            }
            else
            {
                pageNo = 1;
                filters.pn = c_strOne;
            }
        }

        protected void GetSetCityId()
        {
            int tempCityId;
            Int32.TryParse(Request.QueryString["city"] ?? Request.QueryString["cityid"], out tempCityId);  //cityid for url like "ShowCarInCity.aspx?cityid=1"
            if (tempCityId > 0)
            {
                filters.city = tempCityId.ToString();
                if (filters.city.Contains(","))//in case url is of type /cars-in-mumbai/?city=12
                {
                    string[] cities = filters.city.Split(',');
                    filters.city = cities[cities.Length - 1];
                }
                if (IsFromCarwale())
                {
                    fromCW = true;
                    showCityWarning = false;
                }
                else
                {
                    fromCW = false;
                    int globalCityValue = 0;
                    if (GlobalCityCookiePresent(out globalCityValue))
                    {
                        Int32.TryParse(filters.city, out tempCityId);
                        if (globalCityValue == tempCityId)
                        {
                            showCityWarning = false;
                        }
                    }
                }
            }
            else
            {
                ProcessCity();
                if (CustomParser.parseIntObject(filters.city) <= 0)
                {
                    int globalCity = CustomerCookie.MasterCityId;
                    if (globalCity != -1 && globalCity != -2)
                    {
                        filters.city = globalCity.ToString();
                    }
                }
            }
            if (CustomParser.parseIntObject(filters.city) > 0)
            {
                if (filters.city == "1")
                {
                    filters.city = "3000";
                    cityName = "Mumbai";
                }
                else if (filters.city == "3000")
                    cityName = "Mumbai";
                else if (filters.city == "3001")
                    cityName = "Delhi NCR";
                else if(string.IsNullOrEmpty(cityName))
                {
                    Cities cityDetails = _geoCitiesCacheRepo.GetCityDetailsById(Convert.ToInt32(filters.city));
                    cityName = cityDetails.CityName;
                    cityMaskingName = cityDetails.CityMaskingName;
                    stateName = cityDetails.IsDuplicateCityName ? cityDetails.StateName : string.Empty;
                }
                drpCity.SelectedValue = filters.city;
                Int32.TryParse(filters.city, out cityId);
            }
        }

        private bool GlobalCityCookiePresent(out int cityValue)
        {
            HttpCookie globalCityCookie = Request.Cookies["_CustCityIdMaster"];
            if (globalCityCookie != null && globalCityCookie.Value != "-1" && globalCityCookie.Value != "-2")
            {
                cityValue = Convert.ToInt32(globalCityCookie.Value);
                return true;
            }
            cityValue = -1;
            return false;
        }

        private void ProcessCity()
        {
            cityName = Request.QueryString["cityname"];
            if (!string.IsNullOrEmpty(cityName))
            {
                var cityObj = _cityRepo.GetCityDetailsByMaskingName(cityName);
                if (cityObj == null || !cityName.Equals(cityObj.CityMaskingName, StringComparison.InvariantCultureIgnoreCase))
                {
                    cityName = cityObj?.CityName;
                    cityMaskingName = cityObj != null ? cityObj.CityMaskingName: string.Empty;
                    stateName = cityObj != null && cityObj.IsDuplicateCityName ? cityObj.StateName : string.Empty;
                    filters.city = cityObj == null ? string.Empty : cityObj.CityId.ToString();
                    redirect = true;
                }
                else
                {
                    filters.city = cityObj.CityId.ToString();
                    cityName = cityObj.CityName;
                    cityMaskingName = cityObj.CityMaskingName ;
                    stateName = cityObj.IsDuplicateCityName ? cityObj.StateName: string.Empty;
                }
            }
        }

        private void ProcessRedirect()
        {
            if (redirect)
            {
                if(string.IsNullOrEmpty(redirectUrl))
                {
                    string qs = _searchBL.GetCurrentPageQS(Request.QueryString.ToString(), new SearchParams {
                                                                                            Car = filters.car,
                                                                                            City = CustomParser.parseIntObject(filters.city)
                                                                                        });
                    redirectUrl = _searchUtility.GetURL(makeName, rootName, cityName, 0, qs);
                }
                _classifiedSearchCommon.RedirectToNewURL(redirectUrl, Response);
            }
        }

        private bool IsFromCarwale()
        {
            string referrer = "";
            if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] != null)
                referrer = (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"].ToString());
            if (referrer.Contains(ConfigurationManager.AppSettings["HostUrl"].ToString()))
                return true;
            return false;
        }

        protected void GetModelId()
        {
            try
            {
                Entity.CarModelMaskingResponse cmr = new Entity.CarModelMaskingResponse();
                if (!String.IsNullOrEmpty(Request.QueryString[c_strModel]))
                {
                    cmr = GetMaskingNameDetails();
                    redirect = false;
                    if (_classifiedSearchCommon.IsRedirect(cmr))
                    {
                        redirectUrl = _classifiedSearchCommon.GetRedirectURl(cityName, cmr, Request);
                        if (!string.IsNullOrEmpty(qsParameters))
                        {
                            redirectUrl += "?" + qsParameters;
                        }
                        redirect = true;
                    }
                    else if (cmr.ModelId <= 0)
                    {
                        int makeIdentifier = CustomParser.parseIntObject(Request.QueryString[c_strMake]);
                        string qs = (makeIdentifier > 0) ? $"car={ makeIdentifier }" : string.Empty;
                        redirectUrl = _searchUtility.GetURL(string.Empty, string.Empty, cityName, 0, qs);
                        redirect = true;
                    }
                }
                modelId = cmr.ModelId.ToString();
                GetRootIdByModel();
            }
            catch (Exception ex)
            {
                SendErrorMail(ex, "GetModelIdAndName");
            }
        }

        //Get RootId from ModelId obtained in GetModelId() method
        protected void GetRootIdByModel()
        {
            int intModelId;
            if (Int32.TryParse(modelId, out intModelId))
            {
                if (intModelId > 0)
                {
                    RootBase root = _modelRootsCacheRepository.GetRootByModel(intModelId);
                    rootId = root.RootId.ToString();
                    rootName = root.Name;
                }
                else
                    modelId = string.Empty;
            }
            else
                modelId = string.Empty;
        }

        //  Get Make and model names from database in case modelId is present. Used for meta info.
        protected void GetMakeModelName()
        {
            CarModelDetails carModelDetails = _carModelCacheRepository.GetModelDetailsById(Convert.ToInt32(modelId));
            modelId = makeId + "." + modelId;
            makeName = carModelDetails.MakeName;
            modelName = carModelDetails.ModelName;
            maskingName = carModelDetails.MaskingName;
        }

        //  Get Make name from database in case modelId is not present. Used for meta info.
        void GetMakeName()
        {
            string carFilter = filters.car;
            if (!String.IsNullOrEmpty(carFilter) && !carFilter.Contains(c_strPlus) && !carFilter.Contains("."))
            {
                makeId = carFilter;
                CarMakeEntityBase carMakes = _carMakesCacheRepository.GetCarMakeDetails(Convert.ToInt32(makeId));
                makeName = carMakes.MakeName;
            }
        }

        protected Entity.CarModelMaskingResponse GetMaskingNameDetails()
        {
            var arr = Request.QueryString[c_strModel].Split('.');
            Entity.CarModelMaskingResponse cmr = new Entity.CarModelMaskingResponse();
            try
            {
                cmr = _carModelCacheRepository.GetModelByMaskingName(arr[1]);
            }
            catch (Exception ex)
            {
                SendErrorMail(ex, "GetMaskingNameDetails");
                redirect = true;
                redirectUrl = c_strUsedRedirectURL + "?" + qsParameters;
            }
            return cmr;
        }

        //Fill FilterInputs 
        protected void GetCompleteQS()
        {
            filters.SetFilterInputsFromQueryString(HttpUtility.ParseQueryString(Request.QueryString.ToString()));
            makeId = Request.QueryString[c_strMake];
            
            //Conside make and model id passed in URL to form FilterInputs car parameter
            if (String.IsNullOrEmpty(filters.car))
            {
                if (!String.IsNullOrEmpty(makeId))
                {
                    if (!String.IsNullOrEmpty(rootId))
                        filters.car = makeId + "." + rootId;
                    else
                        filters.car = makeId;
                }
            }
        }

        protected void GetCompleteData()
        {
            IList<StockBaseEntity> results = null;
            IUnityContainer container = new UnityContainer();

            results = GetDataFromElasticSearch(container);

            BindRepeaterListings(results);

        }

        protected int GetRank(bool isPremium)
        {
            if (isPremium)
            {
                return ++indexFeatured;
            }
            else
            {
                return ++indexNonFeatured;
            }
        }

        protected int GetRankAbsolute()
        {
            return ++indexAbsolute;
        }
        protected IList<StockBaseEntity> GetDataFromElasticSearch(IUnityContainer container)
        {
            //ResultsFiltersPagerDesktop listingData = _elasticSearchManager.SearchIndex<ResultsFiltersPagerDesktop>(s_strElasticIndexName, filters);
            SearchResultDesktop listingData = _stockSearchLogic.Get(filters);
            try
            {
                filtersCount = listingData.FiltersData?.StockCount?.TotalStockCount ?? 0;
                nearByCity = JsonConvert.SerializeObject(listingData.NearByCitiesWithCount, Formatting.None);
                totalCount = filtersCount;
                jsonData = JsonConvert.SerializeObject(listingData.FiltersData, Formatting.None);
                latestNonPremiumRank = listingData.LastNonFeaturedSlotRank;
                latestPremiumDealerRank = listingData.LastDealerFeaturedSlotRank;
                latestPremiumIndividualRank = listingData.LastIndividualFeaturedSlotRank;
                excludeStocks = listingData.ExcludeStocks;
            }
            catch (Exception ex)
            {
                SendErrorMail(ex, "GetDataFromElasticSearch");
            }
                return listingData.ResultData;
        }


        protected void BindRepeaterListings(IList<StockBaseEntity> results)
        {
            if (results.Any())
            {
                rptStockListings.DataSource = results;
                rptStockListings.DataBind();
            }
        }
        //Redirect the user to M-Site if trying to access the site from mobile
        protected void DetectDevice()
        {
            (new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"])).DetectDevice();
        }
        //Check if user is already logged in the system 
        //If user is logged in pre fill his details in contact seller form
        /*check this, need to be removed*/
        void CheckUserCredentials()
        {
            // If used is loged in, Prefill all the available details
            if (!CurrentUser.Id.Equals("-1"))
            {
                CustomerDetails cdObj = new CustomerDetails(CurrentUser.Id);
            }
        }

        //Common method to send Exception generated mails
        void SendErrorMail(Exception ex, string methodName)
        {
            (new ErrorClass(ex, string.Format("{0} : {1}", HttpContext.Current.Request.ServerVariables[c_strURL], methodName))).SendMail();
        }

        //Get the query string parameters for car and city for old urls
        void GetSoldOutQS()
        {
            string isSoldInfo = Request.QueryString[c_strIsSold];
            if (!string.IsNullOrEmpty(isSoldInfo) && isSoldInfo.Equals(c_strOne))
                soldOut = c_strOne;
        }

        void BindMakes()
        {
            try
            {
                rptMakes.DataSource = _elasticSearchManager.GetAllMakes(ConfigurationManager.AppSettings["ElasticIndexName"]);
                rptMakes.DataBind();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables[c_strURL] + " : BindMakes");
                objErr.SendMail();
            }
        }

        void GetNextPrevUrl()
        {
            double totalPages = Math.Ceiling((double)filtersCount / pageSize);
            int currentPage = Convert.ToInt32(filters.pn);            
            if (currentPage == 1 && currentPage < totalPages)
            {
                next = string.Format(c_strNextURLFor1stPage, nextPrevBaseUrl);
            }
            else if (currentPage < totalPages)
            {
                prev = string.Format(c_strNextPrevBaseURL, nextPrevBaseUrl, currentPage - 1);
                next = string.Format(c_strNextPrevBaseURL, nextPrevBaseUrl, currentPage + 1);
            }
            else if (currentPage == totalPages && currentPage != 1)
                prev = string.Format(c_strNextPrevBaseURL, nextPrevBaseUrl, currentPage - 1);
            else if (currentPage > totalPages && currentPage !=1)
            {
                try
                {
                    redirectUrl = Regex.Replace(Request.Url.PathAndQuery, @"pn=[\d]*", "pn=" + (totalPages != 0 ? (Convert.ToInt32(totalPages)):1));
                }
                catch (Exception ex)
                {
                    SendErrorMail(ex, "GetNextPrevUrl");
                }
                Response.RedirectPermanent(redirectUrl);

            }
        }
        protected void GetIPLocatedCity()
        {
            try
            {
                IPToLocationEntity objIPToLocationEntity = _iPToLocation.GetCity();
                if (objIPToLocationEntity != null)
                {
                    ipDetectedCityName = objIPToLocationEntity.CityName;
                    ipDetectedCityId = objIPToLocationEntity.CityId;
                }
            }
            catch (Exception ex)
            { SendErrorMail(ex, "GetIPLocatedCity"); }

        }
        private void GetListingIndexesForCurrentPage() // function is used only in case  of pageload
        {
            int maxFeaturedLitings = 4; // for desktop max is 4
            int minldr = 3; // assumption that each page will have ldr :3 to maxFeaturedLitings
            int minlcr = 20; // assumption that each page will have lcr :20 to pageSize
            int minlir = 1;  // assumption that each page will have lir :1 to maxFeaturedLitings
            int pazeSize = 24; // for desktop it is 24
            _classifiedSearchCommon.GetListingIndexesForCurrentPage(filters, minlcr, minldr, minlir, pazeSize, maxFeaturedLitings);
        }
        #endregion
    }

}