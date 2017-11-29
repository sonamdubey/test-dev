﻿
using Bikewale.Entities.UrlShortner;
using Bikewale.Notifications;
using Bikewale.Utility;
using Consumer;
using System;
using System.Collections.Generic;
namespace Bikewale.NoPhotoListingsCommunication
{
    /// <summary>
    /// Created By:-Subodh Jain 24 Nov 2016
    /// summary:- For photo upload sms and mail
    /// </summary>
    public class NoPhotoSMS
    {
        private readonly NoPhotoSMSDAL _objNoPhotoSMSDAL = null;
        public NoPhotoSMS(NoPhotoSMSDAL objNoPhotoSMSDAL)
        {
            _objNoPhotoSMSDAL = objNoPhotoSMSDAL;
        }
        /// <summary>
        /// Created By:-Subodh Jain 24 Nov 2016
        /// summary:- For photo upload sms and mail
        /// </summary>
        public void SendSMSNoPhoto()
        {
            NoPhotoUserListEntity objNoPhotoList = null;
            try
            {
                Logs.WriteInfoLog("Calling the SendSMSNoPhoto Dal Layer for SMS and Email Job");
                objNoPhotoList = _objNoPhotoSMSDAL.SendSMSNoPhoto();
                Logs.WriteInfoLog("List return Succefully the SendSMSNoPhoto Dal Layer for SMS and Email Job");
                if (objNoPhotoList != null)
                {
                    Logs.WriteInfoLog("List return is not empty from SendSMSNoPhoto Dal Layer for SMS and Email Job");
                    if (objNoPhotoList.objTwoDaySMSList != null)
                    {
                        SendSMSTwoDays(objNoPhotoList.objTwoDaySMSList);
                    }
                    if (objNoPhotoList.objThreeDayMailList != null)
                    {
                        SendEmailThreeDays(objNoPhotoList.objThreeDayMailList);
                    }
                    if (objNoPhotoList.objSevenDayMailList != null)
                    {
                        SendEmailSevenDays(objNoPhotoList.objSevenDayMailList);
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in NoPhotoListingsCommunication.NoPhotoSMS : ");
                ErrorClass.LogError(ex, "NoPhotoSMS.SendSMSNoPhoto");
                
            }
            Logs.WriteInfoLog("Ended  NoPhotoUpload SMS and Email Job Succefully in NoPhotoListingsCommunication.NoPhotoSMS : ");
        }
        /// <summary>
        /// Created By:-Subodh Jain 24 Nov 2016
        /// summary:- For photo upload SendSMSTwoDays
        /// </summary>
        private void SendSMSTwoDays(IEnumerable<NoPhotoSMSEntity> objTwoDaySMSList)
        {
            try
            {
                UrlShortnerResponse response = null;
                Logs.WriteInfoLog("Started SMS for two days list");
                foreach (var CustomerDetails in objTwoDaySMSList)
                {

                    string editUrl = string.Format("{0}/used/sell/?id={1}&hash=uploadphoto", Utility.BWConfiguration.Instance.BwHostUrl, CustomerDetails.InquiryId);
                    if (!String.IsNullOrEmpty(editUrl))
                    {
                        response = new UrlShortner().GetShortUrl(editUrl);
                    }
                    string shortUrl = response != null ? response.ShortUrl : editUrl;
                    SendEmailSMSToDealerCustomer.SMSNoPhotoUploadTwoDays(CustomerDetails.CustomerName, CustomerDetails.CustomerNumber, CustomerDetails.Make, CustomerDetails.Model, CustomerDetails.InquiryId, shortUrl);
                }
                Logs.WriteInfoLog("Ended SMS for two days list");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in NoPhotoSMS.SendSMSTwoDays : ");
                ErrorClass.LogError(ex, "NoPhotoSMS.SendSMSTwoDays");
                
            }
        }
        /// <summary>
        /// Created By:-Subodh Jain 24 Nov 2016
        /// summary:- For photo upload SendEmailThreeDays
        /// </summary>
        private void SendEmailThreeDays(IEnumerable<NoPhotoSMSEntity> objThreeDayMailList)
        {
            try
            {
                Logs.WriteInfoLog("Started Email for three days list");
                foreach (var CustomerDetails in objThreeDayMailList)
                {
                    SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailForThreeDays(CustomerDetails.CustomerEmail, CustomerDetails.CustomerName, CustomerDetails.Make, CustomerDetails.Model, CustomerDetails.InquiryId);
                }
                Logs.WriteInfoLog("Ended Email for three days list");

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in NoPhotoSMS.SendEmailThreeDays : ");
                ErrorClass.LogError(ex, "NoPhotoSMS.SendEmailThreeDays");
                
            }
        }
        /// <summary>
        /// Created By:-Subodh Jain 24 Nov 2016
        /// summary:- For photo upload SendEmailSevenDays
        /// </summary>
        private void SendEmailSevenDays(IEnumerable<NoPhotoSMSEntity> objSevenDayMailList)
        {
            try
            {
                Logs.WriteInfoLog("Started Email for seven days list");
                foreach (var CustomerDetails in objSevenDayMailList)
                {

                    SendEmailSMSToDealerCustomer.UsedBikePhotoRequestEmailForSevenDays(CustomerDetails.CustomerEmail, CustomerDetails.CustomerName, CustomerDetails.Make, CustomerDetails.Model, CustomerDetails.InquiryId);

                }
                Logs.WriteInfoLog("Ended Email for seven days list");

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in NoPhotoSMS.SendEmailSevenDays : ");
                ErrorClass.LogError(ex, "NoPhotoSMS.SendEmailSevenDays");
                
            }
        }
    }
}
