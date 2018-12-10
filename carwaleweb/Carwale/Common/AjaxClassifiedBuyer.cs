using Carwale.BL.Classified.Leads;
using Carwale.DAL.Classified;
using Carwale.DAL.Classified.CarDetails;
using Carwale.DAL.Classified.Leads;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Leads;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Service;
using Carwale.UI.Common;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Web;

namespace CarwaleAjax
{
    public class AjaxClassifiedBuyer
    {
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool UploadPhotosRequest(string sellInquiryId, string consumerType, string buyerMessage, string buyerName, string buyerEmail, string buyerMobile, string carName)
        {
            bool isDone = false;
            int inquiryId = 0;

            try
            {
                Int32.TryParse(sellInquiryId,out inquiryId);
                if (CommonOpn.CheckId(sellInquiryId))
                {
                    string buyerId = "";
                    AutomateRegistration ar = new AutomateRegistration();
                    AutomateRegistrationResult arr = ar.ProcessRequest(buyerName, buyerEmail, "", buyerMobile, "", "", "");
                    if (!arr.FakeCustomer && !arr.FakeMobileNo)
                    {
                        buyerId = arr.CustomerId;
                    }

                    if (buyerId != "" && buyerId != "-1")
                    {
                        CarDetailRepository carDetailRepo = new CarDetailRepository();
                        isDone = carDetailRepo.UploadPhotosRequest(inquiryId, Convert.ToInt32(buyerId), Convert.ToInt32(consumerType), buyerMessage);

                        // If buyer's request to seller successfully saved to database
                        // Send mail to inform seller
                        if (isDone)
                        {
                            bool isDealer = consumerType == "1" ? true : false;

                            ILeadBL leadBL;
                            // Get Seller details required to send mail
                            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
                            {
                                leadBL = container.Resolve<ILeadBL>();
                            }
                            Seller seller = leadBL.GetSeller(inquiryId, isDealer);

                            if (seller.Email != "" && seller.Name != "")
                            {
                                string subject = "Upload Car Photos";
                                string profileId = (consumerType == "1" ? "D" : "S") + sellInquiryId;
                                string listingUrl = HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/mycarwale/myinquiries/addcarphotos.aspx?car=" + profileId;
                                string mailBody = "";

                                if (isDealer)
                                {
                                    mailBody = ClassifiedMailContent.PhotoRequestToDealerSeller(seller.Name, buyerName, buyerMobile, carName, profileId);
                                }
                                else
                                {
                                    mailBody = ClassifiedMailContent.PhotoRequestToIndividualSeller(seller.Name, buyerName, buyerMobile, carName, listingUrl);
                                }

                                MailServices objMails = new MailServices();

                                // Send mail to myself also. just for testing purpose
                                objMails.SendMail(seller.Email, subject, mailBody, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : AJAX_UploadPhotosRequest");
                objErr.SendMail();
            }
            return isDone;
        }

        /**
         *  Summary : Ajax function to set buyer alerts for the cars
         *  Auther  :Jugal Singh created on Aug 4, 2014
         *  
         */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool SetBuyerAlerts(string email, string alertFrq, string url, string city, string budget, string year,
                                  string kms, string car, string bodyStyle, string fuel,
                                  string transmission, string seller, string filterBy, string owner)
        {
            bool status = false;

            string minBudget = budget.Substring(0, budget.IndexOf("-"));
            float parsedMinBudget = 0F;
            if (!string.IsNullOrEmpty(minBudget))
            {
                float.TryParse(minBudget, out parsedMinBudget);
            }
            string maxBudget = string.Empty;
            float parsedMaxBudget = 0F;
            if (budget.Length > budget.IndexOf("-") + 1)
            {
                maxBudget = budget.Substring(budget.IndexOf("-") + 1);
            }
            float.TryParse(maxBudget, out parsedMaxBudget);
            string minAge = year.Substring(0, year.IndexOf("-"));
            string maxAge = string.Empty;
            if (year.Length > year.IndexOf("-") + 1)
            {
                maxAge = year.Substring(year.IndexOf("-") + 1);
            }

            string minKms = kms.Substring(0, kms.IndexOf("-"));
            string maxKms = string.Empty;
            if (kms.Length > kms.IndexOf("-") + 1)
            {
                maxKms = kms.Substring(kms.IndexOf("-") + 1);
            }

            string owners = owner == string.Empty ? string.Empty : owner.Replace("+", ",");
            string bodyType = bodyStyle == string.Empty ? string.Empty : bodyStyle.Replace("+", ",");
            string transmissionType = transmission == string.Empty ? string.Empty : transmission.Replace("+", ",");
            string sellerType = seller == string.Empty ? string.Empty : seller.Replace("+", ",");
            string fuelType = fuel == string.Empty ? string.Empty : fuel.Replace("+", ",");

            string[] carMakes;
            string strCarMakes = string.Empty;
            string[] carModels;
            string strCarModels = string.Empty;
            if (car.Length > 0)
            {
                ArrayList values = new ArrayList();
                carMakes = car.Split(new char[] { '+' });
                foreach (string carMake in carMakes)
                {
                    if (carMake.IndexOf(".") < 0)
                    {
                        values.Add(carMake);
                    }
                }

                foreach (string val in values)
                {
                    strCarMakes += "," + val.ToString();
                }

                if (strCarMakes.Length > 0)
                {
                    strCarMakes = strCarMakes.Substring(1, strCarMakes.Length - 1);
                }

                carModels = car.Split(new char[] { '+' });
                foreach (string carModel in carModels)
                {
                    if (carModel.IndexOf(".") > 0)
                    {
                        strCarModels += "," + carModel;
                    }
                }
                if (strCarModels.Length > 0)
                {
                    strCarModels = strCarModels.Substring(1, strCarModels.Length - 1);
                }
            }


            bool certifiedcars = false;
            bool carsWithPhotos = false;
            if (filterBy.Trim().IndexOf(" ") > 0)
            {
                certifiedcars = true;
                carsWithPhotos = true;
            }
            else
            {
                if (!String.IsNullOrEmpty(filterBy))
                {
                    if (filterBy == "1")
                    {
                        certifiedcars = true;
                    }
                    else
                    {
                        carsWithPhotos = true;
                    }
                }
            }

            ClassifiedEmailAlertRepository clasifiedEmailRepo = new ClassifiedEmailAlertRepository();
            status = clasifiedEmailRepo.SaveNdUsedCarAlertCustomerList(new NdUsedCarAlert()
            {
                CustomerId = Convert.ToInt32(CurrentUser.Id),
                Email = email == string.Empty ? null : email,
                CityId = city == string.Empty ? Int32.MinValue : Convert.ToInt32(city),
                MakeId = strCarMakes == string.Empty ? null : strCarMakes,
                ModelId = strCarModels == string.Empty ? null : strCarModels,
                FuelTypeId = fuelType == string.Empty ? null : fuelType,
                BodyStyleId = bodyType == string.Empty ? null : bodyType,
                TransmissionId = transmissionType == string.Empty ? null : transmissionType,
                SellerId = sellerType == string.Empty ? null : sellerType,
                MinBudget = parsedMinBudget > 0 ? parsedMinBudget : (float?)null,
                MaxBudget = parsedMaxBudget > 0 ? parsedMaxBudget : (float?)null,
                MinCarAge = minAge == string.Empty ? Int32.MinValue : Convert.ToInt32(minAge),
                MaxCarAge = maxAge == string.Empty ? Int32.MinValue : Convert.ToInt32(maxAge),
                MinKms = minKms == string.Empty ? Int32.MinValue : Convert.ToInt32(minKms),
                MaxKms = maxKms == string.Empty ? Int32.MinValue : Convert.ToInt32(maxKms),
                NeedOnlyCertifiedCars = certifiedcars,
                NeedCarWithPhotos = carsWithPhotos,
                OwnerTypeId = owners == string.Empty ? null : owners,
                AlertFrequency = alertFrq == string.Empty ? Int32.MinValue : Convert.ToInt32(alertFrq),
                AlertUrl = url
            });
            return status;
        }

        /// <summary>
        /// Raises a complaint against a particular classified listing.
        /// </summary>
        /// <param name="inquiryId">Inquiry Id of the listing</param>
        /// <param name="inquiryType">Dealer or Individual listing</param>
        /// <param name="reason">Why the complaint was raised</param>
        /// <param name="description">Description of the complaint</param>
        /// <param name="email">Email of the customer who raised the complaint</param>
        /// <returns>Complaint Id</returns>
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public int ReportListing(int inquiryId, int inquiryType, int reasonId, string description, string email)
        {
            CarDetailRepository carDetailRepo = new CarDetailRepository();
            return carDetailRepo.ReportListing(inquiryId, inquiryType, reasonId, description, email);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string ProcessUsedCarPurchaseInquiry(string profileId, string buyerName, string buyerEmail, string buyerMobile, string carModel, string makeYear, string pageUrl, string transToken, string ltsrc, string buyerSourceId, string cwc, string abTestcookie, string requestType = "1", string utma = "", string utmz = "", int originId = 0, string isFromCaptcha = "0", string isGSDClick = "0", string isRecommended = "false", bool isCertificationDownload = false)
        {
            Logger.LogInfo(string.Format("Old used cars lead API called with headers:{0}", HttpContext.Current.Request.Headers.ToString()));
            return string.Empty;
        }
    }// class
}// namespace