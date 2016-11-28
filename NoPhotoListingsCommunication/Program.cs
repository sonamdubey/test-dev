
using Bikewale.Notifications;
using Consumer;
using System;
namespace Bikewale.NoPhotoListingsCommunication
{
    /// <summary>
    /// Created By:-Subodh Jain 24 Nov 2016
    /// summary:- For photo upload sms and mail
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Logs.WriteInfoLog("Started the NoPhotoUpload SMS and Email Job");
                NoPhotoSMS objNoPhoto = new NoPhotoSMS(new NoPhotoSMSDAL());
                Logs.WriteInfoLog("Calling the SendSMSNoPhoto for SMS and Email Job from Main");
                objNoPhoto.SendSMSNoPhoto();
                Logs.WriteInfoLog("Ended the SendSMSNoPhoto for SMS and Email Job from Main");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in Main NoPhotoListingsCommunication.Program: ");
                ErrorClass objErr = new ErrorClass(ex, "NoPhotoListingsCommunication.Program");
                objErr.SendMail();
            }
            Logs.WriteInfoLog("Ended  NoPhotoUpload SMS and Email Job Succefully");

        }
    }
}
