using Bikewale.DAL.MobileVerification;
using Bikewale.Entities.MobileVerification;
using Bikewale.Interfaces.MobileVerification;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bikewale.BAL.MobileVerification
{
    /// <summary>
    /// Created By : Ashish G. Kamble On 24 Apr 2014
    /// Summary : Class have functions for the entire mobile verification process.
    /// </summary>
    public class MobileVerification : IMobileVerification, IMobileVerificationRepository
    {
        private readonly IMobileVerificationRepository mobileVerRespo = null;
        static readonly IUnityContainer _container;

        static MobileVerification()
        {
            _container = new UnityContainer();
            _container.RegisterType<IMobileVerificationRepository, MobileVerificationRepository>();
        }

        public MobileVerification()
        {
                mobileVerRespo = _container.Resolve<IMobileVerificationRepository>();
        }

        /// <summary>
        /// Summary : Function to process the mobile verification. If email and mobile is verified return true.
        /// Else enters mobile email to the pending list and return Other mobile verification parameters.
        /// </summary>
        /// <param name="email">Email is to be verified.</param>
        /// <param name="mobile">Mobile number to be verified.</param>
        /// <returns>Return object of MobileverificationEntity containing all necessory info of verification.</returns>
        public MobileVerificationEntity ProcessMobileVerification(string email, string mobile)
        {
            MobileVerificationEntity objMobileEntity = new MobileVerificationEntity();

            if (IsMobileVerified(mobile, email))
            {
                objMobileEntity.IsMobileVerified = true;
                objMobileEntity.CustomerEmail = email;
                objMobileEntity.CustomerMobile = mobile;
            }
            else
            {
                string cwiCode = string.Empty, cuiCode = string.Empty;
                ulong cvId = 0;

                // Write logic to generate cwicode and cuiCode.
                Random rnd = new Random();
                Random rnd1 = new Random(rnd.Next());

                //get a random 5 digit number for Bikewale initiated code and the customer initiated code
                cwiCode = GetRandomCode(rnd, 5);
                cuiCode = GetRandomCode(rnd1, 5);

                cvId = AddMobileNoToPendingList(mobile, email, cwiCode, cuiCode);

                objMobileEntity.CustomerEmail = email;
                objMobileEntity.CustomerMobile = mobile;
                objMobileEntity.CWICode = cwiCode;
                objMobileEntity.CUICode = cuiCode;
                objMobileEntity.CvId = cvId;
                objMobileEntity.IsMobileVerified = false;
            }

            return objMobileEntity;
        }

        /// <summary>
        /// Function to check whether mobile email pair is verified or not.
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="emailId"></param>
        /// <returns>If mobile is verified return true else false.</returns>
        public bool IsMobileVerified(string mobileNo, string emailId)
        {
            bool isVerified = false;

            isVerified = mobileVerRespo.IsMobileVerified(mobileNo, emailId);

            return isVerified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public sbyte OTPAttemptsMade(string mobileNo, string emailId)
        {
            sbyte noOfOTPSend = 0;

            noOfOTPSend = mobileVerRespo.OTPAttemptsMade(mobileNo, emailId);

            return noOfOTPSend;
        }


        /// <summary>
        /// Function to add mobile and email to the verification pending list.
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="emailId"></param>
        /// <param name="cwiCode"></param>
        /// <param name="cuiCode"></param>
        /// <returns>Returns customer verification id.</returns>
        public ulong AddMobileNoToPendingList(string mobileNo, string emailId, string cwiCode, string cuiCode)
        {
            ulong cvId = 0;

            cvId = mobileVerRespo.AddMobileNoToPendingList(mobileNo, emailId, cwiCode, cuiCode);

            return cvId;
        }

        /// <summary>
        /// Function to check the user supplied code matches actual verification code.
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="cwiCode"></param>
        /// <param name="cuiCode"></param>
        /// <returns>If verified returns true else false.</returns>
        public bool VerifyMobileVerificationCode(string mobileNo, string cwiCode, string cuiCode)
        {
            bool isVerified = false;

            isVerified = mobileVerRespo.VerifyMobileVerificationCode(mobileNo, cwiCode, cuiCode);

            return isVerified;
        }
       
        /// <summary>
        /// this function generates a random 5 digit code where all the characters are numeric
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GetRandomCode(Random rnd, int length)
        {
            string charPool = "1234567890098765432112345678900987654321";
            StringBuilder rs = new StringBuilder();

            while (length-- > 0)
                rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);

            return rs.ToString();
        }



        public IEnumerable<string> GetBlockedPhoneNumbers()
        { 
                return mobileVerRespo.GetBlockedPhoneNumbers();
        }
    }   // Class
}   // namespace
