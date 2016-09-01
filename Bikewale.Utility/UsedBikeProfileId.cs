using System.Text.RegularExpressions;

namespace Bikewale.Utility
{
    public class UsedBikeProfileId
    {
        /// <summary>
        /// To validate profile id
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns>boolean</returns>
        public static bool IsValidProfileId(string profileId)
        {
            bool isValid = true;

            if (profileId == "")
                isValid = false;

            if (!Regex.IsMatch(profileId, @"^((d|D)|(s|S))[0-9]*$"))
                isValid = false;

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public static void GetInquiryId(string profileId, out string inquiryId, out string consumerType)
        {
            if (IsValidProfileId(profileId))
            {
                consumerType = profileId.Substring(0, 1);
                inquiryId = profileId.Substring(1);
            }
            else
            {
                consumerType = "";
                inquiryId = "";
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Returns the inquiryid
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public static string GetProfileNo(string profileId)
        {
            string retVal = "";

            switch (profileId.Substring(0, 1).ToUpper())
            {
                case "D":
                    retVal = profileId.Substring(1, profileId.Length - 1);
                    break;

                case "S":
                    retVal = profileId.Substring(1, profileId.Length - 1);
                    break;

                default:
                    retVal = profileId;
                    break;
            }

            return retVal;
        }
    }
}
