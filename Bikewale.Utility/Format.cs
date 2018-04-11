using System;
using System.Text.RegularExpressions;

namespace Bikewale.Utility
{
    public static class Format
    {
		/// <summary>
		/// Created by : Ashutosh Sharma on 23 Nov 2017
		/// Description : Format time to hh:mm:ss
		/// </summary>
		public static string FormatTime(uint seconds)
		{
			string time = string.Empty;
			try
			{
				if (seconds > 3600)
				{
					time = TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
				}
				else
				{
					time = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
				}
			}
			catch (Exception)
			{
                throw;
			}
			return time;
		}

        public static string FormatPrice(string minPrice, string maxPrice)
        {
            if ((string.IsNullOrEmpty(minPrice) && string.IsNullOrEmpty(maxPrice)) || (minPrice == "0" && maxPrice == "0"))
            {
                return "N/A";
            }
            else if (minPrice == maxPrice)
            {
                return FormatNumeric(minPrice);
            }
            else
                return FormatNumeric(minPrice) + "-" + FormatNumeric(maxPrice);
        }

        public static string FormatPrice(string price)
        {
            if (price == "" || price == "0")
                return "N/A";
            else
                return FormatNumeric(price);
        }

        /// <summary>
        /// Formats the numeric value - Price, views, likes etc.
        /// </summary>
        /// <param name="numberToFormat">The number to format.</param>
        /// <returns>
        /// Updated by : Sangram Nandkhile on 23-May-2017 
        /// Summary: Added check to show only 3 commas 
        /// For ex. The number 98007654321 will be formatted as  9800,76,54,321
        /// </returns>
        public static string FormatNumeric(string numberToFormat)
        {
            string formatted = string.Empty;
            try
            {
                int breakPoint = 3, noOfCommas = 3;
                string [] tokens = numberToFormat.Split('.');
                if(tokens.Length > 1)
                {
                    formatted += "." + tokens[1];
                }
                numberToFormat = tokens[0];
                for (int i = numberToFormat.Length - 1; i >= 0; i--)
                {
                    formatted = numberToFormat[i].ToString() + formatted;
                    if ((numberToFormat.Length - i) == breakPoint && numberToFormat.Length > breakPoint && noOfCommas > 0)
                    {
                        formatted = "," + formatted;
                        breakPoint += 2;
                        noOfCommas--;
                    }
                }
            }
            catch (Exception)
            {
                return numberToFormat;
            }
            return formatted;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 10 April 2018
        /// Description : Format string containing numbers
        /// </summary>
        /// <param name="numberToFormat"></param>
        /// <returns></returns>
        public static string FormatNumericWithRpm(string numberToFormat)
        {
            string formatted = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(numberToFormat))
                {
                    string[] tokens = numberToFormat.Split(' ');
                    float num;
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (float.TryParse(tokens[i], out num))
                        {
                            tokens[i] = FormatNumeric(tokens[i]);
                        }
                        formatted += tokens[i] + " ";
                    }
                    if (formatted.Length > 1)
                    {
                        formatted.Remove(formatted.Length - 2, 1);
                    }
                }
            }
            catch (Exception)
            {
                return numberToFormat;
            }
            return formatted;
        }
        
        /// <summary>
        /// Created by  : Sushil Kumar on 30th Aug 2016 
        /// Description : Represent number in ordinals i.e 1st,2nd,3rd,11th 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string AddNumberOrdinal(uint num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return string.Format("{0}th", num);
            }

            switch (num % 10)
            {
                case 1:
                    return string.Format("{0}st", num);
                case 2:
                    return string.Format("{0}nd", num);
                case 3:
                    return string.Format("{0}rd", num);
                default:
                    return string.Format("{0}th", num);
            }

        }

        /// <summary>
        /// Created by  : Sushil Kumar on 30th Aug 2016 
        /// Description : Represent number in ordinals i.e 1st,2nd,3rd,11th with upper limit value
        /// </summary>
        /// <param name="num"></param>
        /// <param name="upperLimit"></param>
        /// <returns></returns>
        public static string AddNumberOrdinal(ushort num, ushort upperLimit)
        {
            if (num > upperLimit) return string.Format("{0}+", num);
            else return AddNumberOrdinal(num);
        }

        /// <summary>
        /// Created By : Vivek Gupta
        /// Date : 23-05-2016
        /// Desc : formatting price in format : 23456 => 23.45 K
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FormatPriceShort(string number)
        {
            string retValue = string.Empty;

            try
            {
                if (!Regex.IsMatch(number, @"^[0-9]+$") || number.Length > 9)
                {
                    return "N/A";
                }

                int length = number.Length;
                double numeric = Convert.ToDouble(number);
                switch (length)
                {
                    case 4:
                    case 5:
                        retValue = String.Format("{0}K", Convert.ToString(Math.Round(numeric / 1000, 1)));
                        break;
                    case 6:
                    case 7:
                        retValue = String.Format("{0}L", Convert.ToString(Math.Round(numeric / 100000, 1)));
                        break;
                    case 8:
                    case 9:
                        retValue = String.Format("{0}C", Convert.ToString(Math.Round(numeric / 10000000, 1)));
                        break;
                    default:
                        retValue = FormatPrice(number);
                        break;
                }

                retValue = retValue.Replace(".00", string.Empty);
            }
            catch (Exception)
            {
                return "N/A";
            }

            return retValue;
        }
        /// <summary>
        /// Created by Subodh Jain on 19 Aug 2016 
        /// To Show Lakhs Thousands or crore on Model page
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FormatPriceLong(string number)
        {
            string retValue = string.Empty;

            try
            {
                if (!Regex.IsMatch(number, @"^[0-9]+$") || number.Length > 9)
                {
                    return "N/A";
                }

                int length = number.Length;
                double numeric = Convert.ToDouble(number);
                switch (length)
                {
                    case 4:
                    case 5:
                        retValue = String.Format("{0} Thousands", Convert.ToString(Math.Round(numeric / 1000, 2)));
                        break;
                    case 6:
                    case 7:
                        retValue = String.Format("{0} Lakhs", Convert.ToString(Math.Round(numeric / 100000, 2)));
                        break;
                    case 8:
                    case 9:
                        retValue = String.Format("{0} Crore", Convert.ToString(Math.Round(numeric / 10000000, 2)));
                        break;
                    default:
                        retValue = FormatPrice(number);
                        break;
                }

                retValue.Replace(".00", string.Empty);
            }
            catch (Exception)
            {
                return "N/A";
            }

            return retValue;
        }


        /// <summary>
        /// Formats the numbers.
        /// Created by: Sajal Gupta on 18 Aug 2017
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static string FormatNumbers(uint number)
        {
            try
            {
                if (number < 1000)
                {
                    return number.ToString();
                }
                else if (number < 1000000) //less than million
                {
                    return String.Format("{0:0.#}k", ((double)number / 1000));
                }
                else // greater than million
                {
                    return String.Format("{0:0.#}m", (number / 1000000));
                }

            }
            catch (Exception)
            {
                return "0";
            }
        }

        /// <summary>
        /// Created by Subodh Jain on 6 Sep 2016
        /// To format manufacturer's params
        /// Modifide By :- Subodh jain on 02 March 2017
        /// Summary:- added manufacturer campaign leadpopup changes
        /// <param name="textToReplace"></param>
        /// <param name="ManufacturerName"></param>
        /// <param name="MaskingNumber"></param>
        /// <param name="dealerid"></param>
        /// <param name="dealerArea"></param>
        /// <param name="LeadSourceId"></param>
        /// <param name="PqSourceId"></param>
        /// <param name="action"></param>
        /// <param name="category"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static string FormatManufacturerAd(string textToReplace,
            uint campaignId,
            string ManufacturerName,
            string MaskingNumber,
            uint dealerid,
            string dealerArea,
            string LeadSourceId,
            string PqSourceId,
            string action,
            string category,
            string label, string hide, string LeadCapturePopupHeading, string LeadCapturePopupDescription, string LeadCapturePopupMessage, bool PinCodeRequired, bool EmailRequired)
        {
            string retVal = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(textToReplace))
                {
                    retVal = String.Format(textToReplace, campaignId, ManufacturerName, MaskingNumber, dealerid, dealerArea, LeadSourceId, PqSourceId, action, category, label, hide, LeadCapturePopupHeading, LeadCapturePopupDescription, LeadCapturePopupMessage, PinCodeRequired.ToString().ToLower(), Convert.ToString(EmailRequired).ToLower());
                }
            }
            catch (Exception)
            {
                return retVal;
            }

            return retVal;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 13 Jan 2017
        /// Description: Format numbers as ordinals (1st,2nd,4th etc)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string FormatRank(int num)
        {
            string RankText = string.Empty;
            switch (num)
            {
                case 1:
                    RankText = String.Format("{0}<sup>st</sup>", num);
                    break;
                case 2:
                    RankText = String.Format("{0}<sup>nd</sup>", num);
                    break;
                case 3:
                    RankText = String.Format("{0}<sup>rd</sup>", num);
                    break;
                default:
                    RankText = String.Format("{0}<sup>th</sup>", num);
                    break;

            }
            return RankText;
        }

    }
}
