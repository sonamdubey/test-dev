
using RabbitMqPublishing.Common;
using System;
using System.Text.RegularExpressions;
namespace Bikewale.Utility
{
    public static class Format
    {
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

        public static string FormatNumeric(string numberToFormat)
        {
            string formatted = "";
            int breakPoint = 3;

            for (int i = numberToFormat.Length - 1; i >= 0; i--)
            {
                formatted = numberToFormat[i].ToString() + formatted;
                if ((numberToFormat.Length - i) == breakPoint && numberToFormat.Length > breakPoint)
                {
                    //HttpContext.Current.Trace.Warn(formatted);
                    formatted = "," + formatted;
                    breakPoint += 2;
                }
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

                retValue.Replace(".00", string.Empty);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, String.Format("FormatPriceShort, input : {0}", number));
                objErr.SendMail();
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
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, String.Format("FormatPriceShort, input : {0}", number));
                objErr.SendMail();
                return "N/A";
            }

            return retValue;
        }
    }
}
