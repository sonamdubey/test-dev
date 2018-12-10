using Carwale.Interfaces.Classified.MyListings;
using Carwale.Interfaces.Classified.Leads;
using AEPLCore.Security;
using System;
using Carwale.Notifications.Logs;
using Carwale.Entity.Enum;
using System.Collections.Generic;
using Carwale.DTOs.Classified.MyListings;
using System.Text;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CompareCars;
using System.Linq;
using Carwale.DTOs.CarData;
using Carwale.Utility;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.Classified.Leads;
using Carwale.Interfaces.Template;
using Carwale.Interfaces.Notifications;
using System.Configuration;
using Newtonsoft.Json;
using Carwale.Interfaces.Classified.UsedCarPhotos;
using System.Web;
using Microsoft.Practices.Unity;

namespace Carwale.BL.Classified.MyListings
{
    public class MyListings : IMyListings
    {
        private readonly ISellerCacheRepository _sellerCacheRepo;
        private readonly ICarDetailsCache _carDetailsCache;
        private readonly ICarVersions _carVersion;
        private readonly ICompareCarsBL _compareCar;
        private readonly ITemplateRender _templateRenderer;
        private readonly IEmailNotifications _emailNotifications;
        private readonly IMyListingsRepository _mylistingsRepo;
        private readonly ILeadRepository _leadRepo;
        private readonly IMyListingsCacheRepository _myListingsCacheRepo;
        private readonly ICarPhotosRepository _carphotosRepo;
        private readonly string _desktopImagePage = "/users/getauthid.aspx?authid={0}&login=true&returl=/mycarwale/myinquiries/addcarphotos.aspx?car=S{1}";
        private readonly string _desktopListingPage = "/users/getauthid.aspx?authid={0}&login=true&returl=/mycarwale/myinquiries/mysellinquiry.aspx";

        public MyListings
            (
                ISellerCacheRepository sellerCacheRepo,
                ICarDetailsCache carDetailsCache,
                ICarVersions carVersion,
                ICompareCarsBL compareCar,
                ITemplateRender templateRenderer,
                IEmailNotifications emailNotifications,
                IMyListingsRepository mylistingsRepo,
                ILeadRepository leadRepo,
                IMyListingsCacheRepository myListingsCacheRepo,
                ICarPhotosRepository carPhotosRepo
            )
        {
            _sellerCacheRepo = sellerCacheRepo;
            _carDetailsCache = carDetailsCache;
            _carVersion = carVersion;
            _compareCar = compareCar;
            _templateRenderer = templateRenderer;
            _emailNotifications = emailNotifications;
            _mylistingsRepo = mylistingsRepo;
            _leadRepo = leadRepo;
            _myListingsCacheRepo = myListingsCacheRepo;
            _carphotosRepo = carPhotosRepo;
        }

        public string GetMobileFromProfileId(int profileId)
        {
            return _mylistingsRepo.GetMobileOfActiveInquiry(profileId);
        }

        public bool IsValidToken(string authToken, string mobileNumber, string ipAddress, string cwcCookie)
        {
            bool isValid = false;
            try
            {
                string decodedAuthToken = HttpUtility.UrlDecode(authToken);
                long ticks = long.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(decodedAuthToken)).Split(':')[2]);
                string computedToken = TokenManager.GenerateToken(mobileNumber, ipAddress, cwcCookie, ticks);
                isValid = (decodedAuthToken.Equals(computedToken));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isValid;
        }

        public bool ValidateSearchType(int searchByType)
        {
            return Enum.IsDefined(typeof(SearchType), searchByType);
        }

        public string GetMobile(int type, string value)
        {
            switch (type)
            {
                case (int)SearchType.ProfileId:
                    if (value.ToLower()[0].Equals('s'))
                        value = value.Substring(1, value.Length - 1);
                    return GetMobileFromProfileId(Convert.ToInt32(value));
                case (int)SearchType.Mobile:
                    return value;
            }
            return string.Empty;
        }



        public MyListingsDTO GetDataForEditListing(int inquiryId)
        {
            MyListingsDTO listingsDTO = null;
            try
            {
                CarDetailsEntity carDetails = _carDetailsCache.GetIndividualListingDetails(Convert.ToUInt32(inquiryId));
                if (carDetails == null) return null;
                var carInfo = carDetails.BasicCarInfo;
                List<CarVersionEntity> othercarVersions = _carVersion.GetCarVersionsByType("used", Convert.ToUInt16(carInfo.ModelId), 0, Convert.ToUInt16(carInfo.MakeYear.Year));
                List<int> lstVersionId = new List<int>();
                lstVersionId.Add(Convert.ToInt32(carInfo.VersionId));
                var versions = _compareCar.Get(lstVersionId, getFeaturedVersion: false);
                listingsDTO = new MyListingsDTO();
                listingsDTO.Id = inquiryId;
                listingsDTO.CarName = string.Join(" ", carInfo.MakeName, carInfo.ModelName);
                listingsDTO.MakeYear = carInfo.MakeYear.Year;
                listingsDTO.MakeMonth = carInfo.MakeYear.Month;
                listingsDTO.MakeMonthYearFormatted = carInfo.MakeYear.ToString("MMM yyyy");
                listingsDTO.OtherCarVersions = new List<CarVersionEntity>(othercarVersions).OrderBy(x => x.ID != carInfo.VersionId).ToList();
                listingsDTO.Owners = new Dictionary<int, string>() { { 1, "First owner" }, { 2, "Second owner" }, { 3, "Third owner" }, { 4, "Four or More Owners" } }.OrderBy(x => x.Key != carInfo.OwnerNumber).ToDictionary(x => x.Key, x => x.Value);
                listingsDTO.SelectedOwner = carInfo.OwnerNumber;
                listingsDTO.CarColors = AutoMapper.Mapper.Map<List<List<Carwale.Entity.CompareCars.Color>>, List<List<Carwale.DTOs.CarData.Color>>>(versions.Colors).First();
                bool foundMatchingColor = false;
                if (!listingsDTO.CarColors.Any())
                {
                    listingsDTO.CarColors.Add(new Color { Name = "White", Value = "f7f7f7", });
                    listingsDTO.CarColors.Add(new Color { Name = "Silver", Value = "dbdbdb", });
                    listingsDTO.CarColors.Add(new Color { Name = "Gray", Value = "696a6d", });
                    listingsDTO.CarColors.Add(new Color { Name = "Black", Value = "171717", });
                    listingsDTO.CarColors.Add(new Color { Name = "Red", Value = "ef3f30", });
                    listingsDTO.CarColors.Add(new Color { Name = "Blue", Value = "0288d1", });
                    listingsDTO.CarColors.Add(new Color { Name = "Gold", Value = "ff9400", });
                    listingsDTO.CarColors.Add(new Color { Name = "Maroon", Value = "800000", });
                    listingsDTO.CarColors.Add(new Color { Name = "Brown", Value = "a52a2a", });
                };

                for (int i = 0; i < listingsDTO.CarColors.Count; i++)
                {
                    if (listingsDTO.CarColors[i].Name.Equals(carInfo.Color, StringComparison.OrdinalIgnoreCase))
                    {
                        var currentColor = listingsDTO.CarColors[i];
                        listingsDTO.CarColors.RemoveAt(i);
                        listingsDTO.CarColors.Insert(0, currentColor);
                        foundMatchingColor = true;
                        break;
                    }
                }
                listingsDTO.CarColors.Add(new Color { Name = "Other", Value = "0", });
                if (!foundMatchingColor)
                {
                    listingsDTO.CarColors.RemoveAt(listingsDTO.CarColors.Count - 1);
                    listingsDTO.CarColors.Insert(0, new Color { Name = "Other", Value = carInfo.Color });
                }

                listingsDTO.KilometersFormatted = carInfo.Kilometers;
                listingsDTO.Kilometers = carInfo.KmNumeric;
                listingsDTO.Price = carInfo.Price;
                listingsDTO.PriceFormatted = Format.FormatNumericCommaSep(carInfo.Price.ToString());
                listingsDTO.Insurance = new Dictionary<int, string>() { { 1, "Comprehensive" }, { 2, "Third Party" }, { 3, "Expired" } }.OrderBy(x => x.Key != InsuranceId(carInfo.Insurance)).ToDictionary(x => x.Key, x => x.Value);
                listingsDTO.SelectedInsurance = carInfo.Insurance;
                listingsDTO.InsuranceMonths = new Dictionary<int, string>() { { 1, "Jan" }, { 2, "Feb" }, { 3, "Mar" }, { 4, "Apr" }, { 5, "May" }, { 6, "Jun" }, { 7, "Jul" }, { 8, "Aug" }, { 9, "Sep" }, { 10, "Oct" }, { 11, "Nov" }, { 12, "Dec" } };
                listingsDTO.InsuranceYears = new List<int>();
                listingsDTO.InsuranceYears.AddRange(Enumerable.Range(DateTime.Now.Year, 5));
                if (!carInfo.Insurance.Equals("Expired", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(carInfo.InsuranceExpiry))
                {
                    int startYear = Convert.ToInt32(carInfo.InsuranceExpiry.Split(' ')[2]);
                    string month = carInfo.InsuranceExpiry.Split(' ')[1].Trim();
                    listingsDTO.InsuranceYears.Clear();
                    listingsDTO.InsuranceYears.AddRange(Enumerable.Range(startYear, 5));
                    listingsDTO.InsuranceMonths.Clear();
                    listingsDTO.InsuranceMonths = new Dictionary<int, string>() { { 1, "Jan" }, { 2, "Feb" }, { 3, "Mar" }, { 4, "Apr" }, { 5, "May" }, { 6, "Jun" }, { 7, "Jul" }, { 8, "Aug" }, { 9, "Sep" }, { 10, "Oct" }, { 11, "Nov" }, { 12, "Dec" } }.OrderBy(x => !x.Value.Equals(month, StringComparison.OrdinalIgnoreCase)).ToDictionary(x => x.Key, x => x.Value);
                }
                listingsDTO.SelectedRegType = carInfo.RegType.ToString();
                listingsDTO.RegType = new Dictionary<int, string>() { { 1, CarRegistrationType.Individual.ToString() }, { 2, CarRegistrationType.Corporate.ToString() }, { 3, CarRegistrationType.Taxi.ToString() } }
                  .OrderBy(x => !x.Value.Equals(carInfo.RegType.ToString())).ToDictionary(x => x.Key, x => x.Value);
                listingsDTO.RegistrationNumber = carInfo.RegistrationNumber;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return listingsDTO;
        }
        private int InsuranceId(string insurance)
        {
            switch (insurance)
            {
                case "Comprehensive": return 1;
                case "ThirdParty": return 2;
                case "Expired": return 3;
            }
            return 0;
        }

        private string GetListingDeleteSatus(int statusId)
        {
            string status = string.Empty;
            switch (statusId)
            {
                case (int)ListingStatus.Converted:
                    status = ListingStatus.Converted.ToString();
                    break;
                case (int)ListingStatus.CustomerDissatisfied:
                    status = ListingStatus.CustomerDissatisfied.ToString();
                    break;
                case (int)ListingStatus.LostToAnotherInquiry:
                    status = ListingStatus.LostToAnotherInquiry.ToString();
                    break;
                case (int)ListingStatus.LostToCompetitor:
                    status = ListingStatus.LostToCompetitor.ToString();
                    break;
                case (int)ListingStatus.MoneyRefund:
                    status = ListingStatus.MoneyRefund.ToString();
                    break;
                case (int)ListingStatus.NotSellingAnymore:
                    status = ListingStatus.NotSellingAnymore.ToString();
                    break;
                case (int)ListingStatus.SixtyDaysAutoRemoved:
                    status = ListingStatus.SixtyDaysAutoRemoved.ToString();
                    break;
            }
            return status;
        }

        public void SendMail(int inquiryId, SellCarInfo sellCarInfo)
        {
            try
            {
                string templateId = ConfigurationManager.AppSettings["c2bdeletetemplate"];
                string emailAddress = ConfigurationManager.AppSettings["c2bteamemailid"];
                Seller seller = _sellerCacheRepo.GetIndividualSeller(inquiryId);
                var model = new
                {
                    InquiryType = "Sell Car",
                    CustomerName = seller.Name,
                    CustomerMobile = seller.Mobile,
                    CustomerCity = seller.Address,
                    InquiryId = inquiryId,
                    Status = GetListingDeleteSatus(sellCarInfo.StatusId != null ? sellCarInfo.StatusId.Value : (int)ListingStatus.Converted),
                    Comments = sellCarInfo.DeleteComments
                };
                string message = _templateRenderer.Render(Convert.ToInt32(templateId), model);
                string subject = "Customer has archived his sell inquiry.";
                if (!string.IsNullOrWhiteSpace(message) && !string.IsNullOrWhiteSpace(emailAddress))
                {
                    _emailNotifications.SendMail(emailAddress, subject, message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public void RefreshCacheWithCriticalRead(int inquiryId)
        {
            _carDetailsCache.RefreshIndividualStockKey(inquiryId, true);
        }

        private string GetCustomerKey(int inquiryId)
        {
            return _mylistingsRepo.GetCustomerKey(inquiryId);
        }

        public string GetDestinationUrl(int inquiryId, string page)
        {
            if (page.ToLower().Contains("images"))
            {
                return string.Format(_desktopImagePage, GetCustomerKey(inquiryId), inquiryId);
            }
            else
            {
                return string.Format(_desktopListingPage, GetCustomerKey(inquiryId));
            }
        }

        public void AddCookie(string name, string value, string path)
        {
            Utility.Cookie cookie = new Utility.Cookie(name);
            cookie.Expires = DateTime.Now.AddMonths(3);
            cookie.Value = value;
            cookie.Path = path;
            CookieManager.Add(cookie);
        }

        public PageMetaTags GetPageMetaTags()
        {
            return new PageMetaTags
            {
                Title = "My car listings - CarWale user login",
                Description = "CarWale user login, User can see enquiries and edit the car listing",
                Keywords = "CarWale edit listing, Check enquiries, CarWale user login, Edit your Ad, Upload images, Login",
                Canonical = "https://www.carwale.com/used/mylistings/search"
            };
        }


        public IList<ClassifiedRequest> GetClassifiedRequests(int inquiryId, int requestDate)
        {
            var individualLeads = _leadRepo.GetClassifiedRequests(inquiryId, requestDate);
            var c2bLeads = GetC2BLeads(inquiryId);
            var carTradeLeads = GetCarTradeLeads(inquiryId);
            return MergeLeads(MergeLeads(individualLeads, c2bLeads), carTradeLeads);
        }

        private static IList<ClassifiedRequest> MergeLeads(IList<ClassifiedRequest> firstList, IList<ClassifiedRequest> secondList)
        {
            IList<ClassifiedRequest> leads = new List<ClassifiedRequest>();
            if (secondList == null)
            {
                return firstList;
            }

            if (firstList == null)
            {
                return secondList;
            }
            int firstListCount = firstList.Count();
            int secondListCount = secondList.Count();
            int pointerToFirstList = 0;
            int pointerToSecondList = 0;
            // merge process of merge sort
            while (pointerToFirstList < firstListCount && pointerToSecondList < secondListCount)
            {
                leads.Add((DateTime.Compare(firstList[pointerToFirstList].RequestDateTime, secondList[pointerToSecondList].RequestDateTime) <= 0 ?
                firstList[pointerToFirstList++] : secondList[pointerToSecondList++]));
            }
            while (pointerToFirstList < firstListCount)
            {
                leads.Add(firstList[pointerToFirstList++]);
            }
            while (pointerToSecondList < secondListCount)
            {
                leads.Add(secondList[pointerToSecondList++]);
            }
            return leads;
        }

        public int GetC2BLeadsCount(int inquiryId)
        {
            var c2bLeads = GetC2BLeads(inquiryId);
            return c2bLeads != null ? c2bLeads.Count() : 0;
        }
        public int GetCarTradeLeadsCount(int inquiryId)
        {
            var carTradeLeads = GetCarTradeLeads(inquiryId);
            return carTradeLeads != null ? carTradeLeads.Count() : 0;
        }

        private IList<ClassifiedRequest> GetC2BLeads(int inquiryId)
        {
            IList<ClassifiedRequest> c2bLeads = new List<ClassifiedRequest>();
            C2BLeadResponse c2bLeadResponse = _myListingsCacheRepo.GetC2BLeads(inquiryId);
            if (c2bLeadResponse != null && c2bLeadResponse.Result != null && c2bLeadResponse.Result.Any())
            {
                c2bLeads = AutoMapper.Mapper.Map<IList<C2BLead>, IList<ClassifiedRequest>>(c2bLeadResponse.Result);
            }
            Logger.LogInfo(c2bLeadResponse != null ? JsonConvert.SerializeObject(c2bLeadResponse) : "C2BLeadResponse object is null");
            return c2bLeads;
        }

        private IList<ClassifiedRequest> GetCarTradeLeads(int inquiryId)
        {
            IList<ClassifiedRequest> carTradeLeads = new List<ClassifiedRequest>();
            CarTradeLeadResponse carTradeLeadResponse = _myListingsCacheRepo.GetCarTradeLeads(inquiryId);
            if (carTradeLeadResponse != null && carTradeLeadResponse.Result != null && carTradeLeadResponse.Result.Any())
            {
                carTradeLeads = AutoMapper.Mapper.Map<IList<CarTradeLead>, IList<ClassifiedRequest>>(carTradeLeadResponse.Result);
            }
            Logger.LogInfo(carTradeLeadResponse != null ? JsonConvert.SerializeObject(carTradeLeadResponse) : "CarTradeLeadResponse object is null");
            return carTradeLeads;
        }


        public bool IsImagesPendingToApprove(int inquiryId)
        {
            var imagesList = _carphotosRepo.GetCarPhotos(inquiryId, false);
            if (imagesList == null || !imagesList.Any())
            {
                return false;
            }
            return !imagesList.Any(image => image.IsApproved);
        }
    }

}
