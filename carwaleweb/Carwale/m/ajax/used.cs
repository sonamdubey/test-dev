using Carwale.BL;
using Carwale.BL.Classified.Leads;
using Carwale.DAL.Classified;
using Carwale.DAL.Classified.UsedDealers;
using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Classified.UsedLeads;
using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Entity.Stock.Certification;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Interfaces.Notifications;
using Carwale.Interfaces.Stock;
using Carwale.Notifications.SMSTemplates.Classified;
using Carwale.Service;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using MobileWeb.Common;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;


namespace MobileWeb.Ajax
{
    public class Used
    {
        [AjaxPro.AjaxMethod()]
        public bool ProcessUsedCarPhotoRequest(string profileId, string isDealer, string inquiryId)
        {
            bool isDone = false;
            try
            {
                // Send mail to inform seller
                string subject = "Upload Car Photos";
                string listingUrl = HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/mycarwale/myinquiries/addcarphotos.aspx?car=" + profileId;
                string mailBody = string.Empty;
                Carwale.UI.Common.MailServices objMails = new Carwale.UI.Common.MailServices();

                bool isDealerBool = isDealer == "1";
                ILeadBL leadBL;
                IStockBL stockBL;
                using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
                {
                    leadBL = container.Resolve<ILeadBL>();
                    stockBL = container.Resolve<IStockBL>();
                }
                Seller seller = leadBL.GetSeller(Convert.ToInt32(inquiryId), isDealerBool);
                var stockInfo = stockBL.GetStock(profileId).BasicCarInfo;
                string carName = $"{stockInfo.MakeName} {stockInfo.ModelName} {stockInfo.VersionName}";

                if (isDealerBool)
                {
                    if (seller.Email != string.Empty && seller.Name != string.Empty)
                    {
                        isDone = true;
                        mailBody = Carwale.UI.Common.ClassifiedMailContent.PhotoRequestToDealerSeller(seller.Name, string.Empty, string.Empty, carName, profileId);
                        objMails.SendMail(seller.Email, subject, mailBody, true);   //Send Email
                    }
                }
                else
                {
                    if (seller.Email != string.Empty && seller.Name != string.Empty)
                    {
                        isDone = true;
                        mailBody = Carwale.UI.Common.ClassifiedMailContent.PhotoRequestToIndividualSeller(seller.Name, string.Empty, string.Empty, carName, listingUrl);
                        objMails.SendMail(seller.Email, subject, mailBody, true);  //Send Email
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " :ProcessUsedCarPhotoRequest for inquiryId : " + inquiryId);
                objErr.SendMail();
            }
            return isDone;
        }
    }
}