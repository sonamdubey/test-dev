

using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;
using Carwale.Notifications;
using Carwale.Utility;

namespace Carwale.UI.Common
{
    public class FormatPrice
    {

        #region constants
        private const string c_strPriceFormatString = "{0:0.00}";
        private const string c_strPriceFromCroreToCrore = "{0} - {1}Cr";
        private const string c_strPriceFromLakhsToCrore = "{0}L - {1}Cr";
        private const string c_strPriceFromLakhToLakh = "{0} - {1}L";
        private const string c_strPriceInCrore = "{0}Cr";
        private const string c_strPriceInLakh = "{0}L";
        private const string c_strNotApplicable = "N/A";
        private const string c_strPriceAfterDot = ".00";

        public const string c_strSinglePriceFormat = "{0} {1}";
        public const string c_strDoublePriceFormat = "{0} - {1} {2}";
        public const string c_strDoublePriceFormatSeperateUnits = "{0} {1} - {2} {3}";
        public const string c_strCrores = "Crores";
        public const string c_strLakhs = "Lakhs";
        public const string c_strAbbreviatedCrores = "Cr";
        public const string c_strAbbreviatedLakhs = "L";

        #endregion

        /// <summary>
        /// Created By Prashant Vishe On 15 July 2013
        /// Function is used to Format Price in lakhs and crores...
        /// </summary>
        /// <param name="minPrice">minPrice (e.g. 100000)</param>
        /// <param name="maxPrice">maxPrice  (e.g. 20000000)</param>
        /// <returns></returns>
        public static string FormatFullPrice(string minPrice, string maxPrice)
        {
            string priceRange = string.Empty, tempMinPrice = string.Empty, tempMaxPrice = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(minPrice) && !String.IsNullOrEmpty(maxPrice))
                {
                    minPrice = RemoveCommaFromPriceString(minPrice);
                    maxPrice = RemoveCommaFromPriceString(maxPrice);

                    if (double.Parse(minPrice) == double.Parse(maxPrice))
                    {
                        if (minPrice.Length >= 8) //when price in crore.
                        {
                            tempMinPrice = GetPriceFormattedAndDividedBy10000000(minPrice);
                            priceRange = string.Format(c_strPriceInCrore, tempMinPrice);
                        }

                        else if ((minPrice.Length >= 6) && (minPrice.Length < 8)) //when price in lakhs.
                        {
                            tempMinPrice = GetPriceFormattedAndDividedBy100000(minPrice);
                            priceRange = string.Format(c_strPriceInLakh, tempMinPrice);
                        }
                        else //when price in thousands.
                        {
                            priceRange = minPrice;
                        }
                    }
                    else
                    {
                        if (minPrice.Length >= 8)  //when both min and max prices are in crores
                        {
                            tempMinPrice = GetPriceFormattedAndDividedBy10000000(minPrice);
                            tempMaxPrice = GetPriceFormattedAndDividedBy10000000(maxPrice);
                            priceRange = string.Format(c_strPriceFromCroreToCrore, tempMinPrice, tempMaxPrice);
                        }
                        else if ((minPrice.Length < 8) && (minPrice.Length >= 6) && (maxPrice.Length >= 8)) //when min price in lakhs and max price is in crores
                        {
                            tempMinPrice = GetPriceFormattedAndDividedBy100000(minPrice);
                            tempMaxPrice = GetPriceFormattedAndDividedBy10000000(maxPrice);
                            priceRange = string.Format(c_strPriceFromLakhsToCrore, tempMinPrice, tempMaxPrice);
                        }
                        else if ((minPrice.Length >= 6) && (maxPrice.Length < 8) && (minPrice.Length < 8)) //when min ans max prices are in lakhs
                        {
                            tempMinPrice = GetPriceFormattedAndDividedBy100000(minPrice);
                            tempMaxPrice = GetPriceFormattedAndDividedBy100000(maxPrice);
                            priceRange = string.Format(c_strPriceFromLakhToLakh, tempMinPrice, tempMaxPrice);
                        }
                        else //when min and max prices are in thousands
                        {
                            priceRange = string.Format("{0} - {1}", minPrice, maxPrice);
                        }
                    }
                }
                else
                {
                    priceRange = c_strNotApplicable;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "FormatFullPrice");
                objErr.SendMail();
            }
            return priceRange.Replace(c_strPriceAfterDot, string.Empty);
        }

        /// <summary>
        /// Created By:Prashant Vishe on 16 july 2013
        /// Function is used to Format price in lakhs and crores...
        /// </summary>
        /// <param name="price">price(e.g.200000)</param>
        /// <returns></returns>
        public static string FormatFullPrice(string price)
        {
            string priceRange = string.Empty, tempPrice = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(price) && !price.Equals("0"))
                {
                    price = RemoveCommaFromPriceString(price);


                    if (price.Length >= 8) //when price in crore.
                    {
                        tempPrice = GetPriceFormattedAndDividedBy10000000(price);
                        priceRange = string.Format(c_strPriceInCrore, tempPrice);
                    }

                    else if ((price.Length >= 6) && (price.Length < 8)) //when price in lakhs.
                    {
                        tempPrice = GetPriceFormattedAndDividedBy100000(price);
                        priceRange = string.Format(c_strPriceInLakh, tempPrice);
                    }
                    else //when price in thousands.
                    {
                        priceRange = price;
                    }
                }
                else
                {
                    priceRange = c_strNotApplicable;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "FormatFullPrice");
                objErr.SendMail();
            }
            return priceRange.Replace(c_strPriceAfterDot, string.Empty);
        }

        /// <summary>
        /// Created By:Prashant Vishe On 15 July 2013
        /// function is used to calculate digits before decimal point...
        /// </summary>
        /// <param name="price">price (e.g.300.10)</param>
        /// <returns></returns>
        public static int PriceLength(string price)
        {
            string[] s = new string[2];
            try
            {
                s = price.Split('.');

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "PriceLength");
                objErr.SendMail();
            }
            return (s[0].Length);
        }

		private static string RemoveCommaFromPriceString(string inputPrice)
		{
			if (string.IsNullOrEmpty(inputPrice))
				return string.Empty;
			return inputPrice.Replace(",", string.Empty);
		}

        private static string GetPriceFormattedAndDividedBy10000000(string price)
        {
            if (string.IsNullOrEmpty(price))
                return string.Empty;
            else
                return String.Format(c_strPriceFormatString, Math.Round((double.Parse(price) / 10000000), 2, MidpointRounding.AwayFromZero));
        }

        private static string GetPriceFormattedAndDividedBy100000(string price)
        {
            if (string.IsNullOrEmpty(price))
                return string.Empty;
            else
                return String.Format(c_strPriceFormatString, Math.Round((double.Parse(price) / 100000), 2, MidpointRounding.AwayFromZero));
        }

        public static string GetFormattedPriceV2(string p1, string p2 = null,string format=null,bool abbreviateUnits=false)
        {
            string p1units = c_strLakhs, p2units = c_strCrores;

            if (RegExValidations.IsNumeric(p1))
            {
                int p1length = PriceLength(p1);
                if (p1length >= 8)
                {
                    p1 = Math.Round((double.Parse(p1) / 10000000), 2, MidpointRounding.AwayFromZero).ToString("0.##");
                    p1units = abbreviateUnits? c_strAbbreviatedCrores : c_strCrores;
                }
                else if (p1length >= 4)
                {
                    p1 = Math.Round((double.Parse(p1) / 100000), 2, MidpointRounding.AwayFromZero).ToString("0.##");
                    p1units = abbreviateUnits ? c_strAbbreviatedLakhs : c_strLakhs;
                }
                else if (double.Parse(p1) == 0)
                {
                    p1 = c_strNotApplicable;
                    p1units = string.Empty;
                }
            }
            else return c_strNotApplicable;
            if (RegExValidations.IsNumeric(p2))
            {
                int p2length = PriceLength(p2);
                if (p2length >= 8)
                {
                    p2 = Math.Round((double.Parse(p2) / 10000000), 2, MidpointRounding.AwayFromZero).ToString("0.##");
                    p2units = abbreviateUnits ? c_strAbbreviatedCrores : c_strCrores;
                }
                else if (p2length >= 4)
                {
                    p2 = Math.Round((double.Parse(p2) / 100000), 2, MidpointRounding.AwayFromZero).ToString("0.##");
                    p2units = abbreviateUnits ? c_strAbbreviatedLakhs : c_strLakhs;
                }
                else if (double.Parse(p2) == 0)
                {
                    p2 = c_strNotApplicable;
                    p2units = string.Empty;
                }
                if (p1units != p2units)
                {
                    return p2 == c_strNotApplicable || p1 == c_strNotApplicable ? string.Format(format??c_strSinglePriceFormat, p1, p1units) : string.Format(format??c_strDoublePriceFormatSeperateUnits, p1, p1units, p2, p2units);
                }
                else
                {
                    return p1 == p2 ? string.Format(format??c_strSinglePriceFormat, p1, p1units) : string.Format(format??c_strDoublePriceFormat, p1, p2, p2units);
                }
            }
            else
            {
                return string.Format(format??c_strSinglePriceFormat, p1, p1units);
            }
        }
    }//class
}//namespace