using System;

namespace Bikewale.Utility
{
    public class Ordinal
    {
        /// <summary>
        /// Created by: Sangram Nandkhile on 30th Aug 2016
        /// Summary: This function returns 2nd where 2 is passed
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetRank(UInt16 num)
        {
            string retValue = string.Empty;
            if (num > 0)
            {
                switch (num)
                {
                    case 1:
                        retValue = "1st";
                        break;
                    case 2:
                        retValue = "2nd";
                        break;
                    case 3:
                        retValue = "3rd";
                        break;
                    default:
                        retValue = "3+";
                        break;
                }
            }
            else
            {
                return num.ToString();
            }
            return retValue;
        }
    }
}
