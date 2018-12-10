using Carwale.BL.Customers;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Notifications.Classified;
using Carwale.Notifications.Interface;
using Carwale.Notifications.SMSTemplates.Classified;
using Carwale.Utility;
using Carwale.Utility.Classified;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.BL.Classified.UsedSellCar
{
    public class SellCarBL : ISellCarBL
    {
        private const int minMakeYear = 1998;
        private const int minIsuranceYearDiff = 5;
        private readonly ISellCarRepository _sellCarRepository;
        private readonly ISmsLogic _smsLogic;
        private readonly ICarVersionCacheRepository _versionCacheRepo;
        private readonly ISellCarCacheRepository _sellcarCacheRepo;
        private readonly IConsumerToBusinessBL _consumerToBusinessBL;
        public static readonly string _freePkgType = ConfigurationManager.AppSettings["freePkgType"];
        public static readonly string _paidPkgType = ConfigurationManager.AppSettings["paidPkgType"];
        private static readonly string _c2bIpAddress = ConfigurationManager.AppSettings["C2BIpAddress"];

        public SellCarBL(ISellCarRepository sellCarRepository,
            ICarVersionCacheRepository versionCacheRepo,
            ISellCarCacheRepository sellcarCacheRepo,
            IConsumerToBusinessBL consumerToBusinessBL,
            ISellerCacheRepository sellerCacheRepo,
            ISmsLogic smsLogic
            )
        {
            _sellCarRepository = sellCarRepository;
            _versionCacheRepo = versionCacheRepo;
            _sellcarCacheRepo = sellcarCacheRepo;
            _consumerToBusinessBL = consumerToBusinessBL;
            _smsLogic = smsLogic;
        }

        public void UpdateSellCarCurrentStep(int inquiryId, int currentStep, bool updateForceFully)
        {
            if (!updateForceFully)
            {
                int completedStep = _sellCarRepository.GetSellCarStepsCompleted(inquiryId);
                updateForceFully = currentStep > completedStep;
            }
            if (updateForceFully)
            {
                _sellCarRepository.UpdateSellCarCurrentStep(inquiryId, currentStep);
            }
        }
        public bool CheckFreeListingAvailability(string mobile)
        {
            return (_sellCarRepository.GetFreeListingCount(mobile) < Constants.IndividualListingLimit);
        }

        public bool CreateCustomer(SellCarCustomer sellCustomer)
        {
            ICustomerBL<Customer, CustomerOnRegister> customerBL = new CustomerActions<Customer, CustomerOnRegister>();
            var customer = customerBL.CreateCustomer(new Customer()
            {
                Name = sellCustomer.Name,
                Email = sellCustomer.Email,
                Mobile = sellCustomer.Mobile,
                CityId = sellCustomer.CityId
            });
            sellCustomer.Id = Convert.ToInt32(customer.CustomerId);
            return customer.StatusOnRegister == "N"; //return true if new customer
        }

        public int ProcessContactDetails(SellCarCustomer sellCustomer)
        {
            int tempInquiryId = -1;
            tempInquiryId = _sellCarRepository.SaveTempSellCarInquiryDetails(null, sellCustomer);
            if (tempInquiryId > 0)
            {
                _consumerToBusinessBL.PushToIndividualStockQueue(tempInquiryId, C2BActionType.AddCustomerDetails);
            }
            return tempInquiryId;
        }

        public int CreateSellCarInquiry(TempCustomerSellInquiry tempInquiry)
        {
            int inquiryId = -1;
            if (tempInquiry != null)
            {
                inquiryId = _sellCarRepository.SaveSellCarDetails(tempInquiry.sellCarInfo, tempInquiry.sellCarCustomer);
                ProcessCreatedInquiry(tempInquiry, inquiryId);
            }
            return inquiryId;
        }

        public int CreateSellCarInquiryV1(TempCustomerSellInquiry tempInquiry)
        {
            int inquiryId = -1;
            if (tempInquiry != null)
            {
                inquiryId = _sellCarRepository.SaveSellCarDetailsV1(tempInquiry.sellCarInfo, tempInquiry.sellCarCustomer);
                ProcessCreatedInquiry(tempInquiry, inquiryId);
            }
            return inquiryId;
        }

        private void ProcessCreatedInquiry(TempCustomerSellInquiry tempInquiry, int inquiryId)
        {
            if (inquiryId > 0)
            {
                var versionDetails = _versionCacheRepo.GetVersionDetailsById(tempInquiry.sellCarInfo.VersionId);
                if (versionDetails != null)
                {
                    string carName = $"{ versionDetails.MakeName } { Format.FilterModelName(versionDetails.ModelName) }";
                    SendNotifications(inquiryId, carName, tempInquiry.sellCarCustomer, tempInquiry.Platform);
                }

                if (tempInquiry.sellCarInfo.ExpectedPrice > 0 && tempInquiry.sellCarInfo.ExpectedPrice == tempInquiry.sellCarInfo.RecommendedPrice) // hot lead
                {
                    _consumerToBusinessBL.PushToIndividualStockQueue(tempInquiry.sellCarInfo.TempInquiryId, C2BActionType.AddCarDetails, inquiryId, tempInquiry.sellCarInfo.RecommendedPrice);
                }
                else
                {
                    _consumerToBusinessBL.PushToIndividualStockQueue(tempInquiry.sellCarInfo.TempInquiryId, C2BActionType.AddCarDetails, inquiryId);
                }
            }
        }

        public IEnumerable<int> GetMakeYears()
        {
            return Enumerable.Range(minMakeYear, DateTime.Now.Year - minMakeYear + 1).Reverse();
        }

        public IEnumerable<int> GetInsuranceYears()
        {
            return Enumerable.Range(DateTime.Now.Year, minIsuranceYearDiff);
        }

        public PageMetaTags GetPageMetaTags()
        {
            return new PageMetaTags
            {
                Title = "Sell Car | Sell Used Car in India - CarWale",
                Description = "Sell Your Used / pre-owned car at CarWale.com. Selling at carwale.com is easy, quick, effective and guaranteed. India's no 1 car site."
            };
        }

        public void SendNotifications(int inquiryId, string carName, SellCarCustomer customer, Platform platform)
        {
            string profileId = "S" + inquiryId;
            ClassifiedMails.MailToSellCarCustomer(customer.Name, customer.Mobile, customer.Email, profileId);

            SMS sellCarSMS = SellerSMSTemplate.GetSellCarSMSTemplate(customer.Mobile, carName, "/used/sell/", platform);
            
            if (sellCarSMS != null)
            {
                _smsLogic.Send(sellCarSMS);
            }
        }

        public bool IsC2bCity(int cityId)
        {
            IEnumerable<int> cities = _sellcarCacheRepo.C2BCities();
            if (cities == null) return false;
            return Array.BinarySearch<int>(cities.ToArray(), cityId) > -1;
        }

        public bool ValidateSource(int sourceId)
        {
            return Enum.IsDefined(typeof(Platform), sourceId);
        }

        public static string GetPlanName(ClassifiedPackageType classifiedPackageType)
        {
            switch (classifiedPackageType)
            {
                case ClassifiedPackageType.FreePlan:
                    return "Free Plan";
                case ClassifiedPackageType.AssistedSales:
                    return "Guaranteed Sale Plan";
                default:
                    return string.Empty;
            }
        }

        private bool IsValidC2BIpAddress(string ipAddress)
        {
            return !string.IsNullOrWhiteSpace(_c2bIpAddress) && _c2bIpAddress.Contains(ipAddress);
        }

        public bool InsertVerifiedMobile(string mobile, int sourceId)
        {
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                return _sellCarRepository.InsertVerifiedMobileEmailPair(mobile, mobile + "@unknown.com", sourceId);
            }
            return false;
        }
    }
}
