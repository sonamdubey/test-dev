using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace Carwale.Utility
{
    public static class Format
    {
        private const string c_strPriceInCrore = "{0}Cr";
        private const string c_strPriceInLakh = "{0}L";
        private const string c_strPriceFromCroreToCrore = "{0} - {1}Cr";
        private const string c_strPriceFromLakhsToCrore = "{0}L - {1}Cr";
        private const string c_strPriceFromLakhToLakh = "{0} - {1}L";
        private const string c_strPriceAfterDot = ".00";
        private static readonly string _rupeeSymbol = ConfigurationManager.AppSettings["rupeeSymbol"];
        public const string c_strCrores = "Crores";
        public const string c_strLakhs = "Lakhs";
        public const string c_strAbbreviatedCrores = "Cr";
        public const string c_strAbbreviatedLakhs = "L";
        public const string c_strSinglePriceFormat = "{0} {1}";
        public const string c_strDoublePriceFormat = "{0} - {1} {2}";
        private const string c_strNotApplicable = "N/A";
        public const string c_strDoublePriceFormatSeperateUnits = "{0} {1} - {2} {3}";
        private static readonly CultureInfo cultureInfo = new CultureInfo("hi-IN");

        public static string RemoveSpecialCharacters(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            string pattern = "[^/\\-0-9a-zA-Z\\s]*";
            url = Regex.Replace(url, pattern, "", RegexOptions.IgnoreCase);
            return url.Replace(" ", "").Replace("-", "").Replace("/", "").ToLower();
        }

        // will format the a name to make it URL compatible 
        //for accessories only
        public static string FormatURL(string url)
        {
            string reg = @"[^0-9a-zA-Z\s]*"; // everything except a-z and 0-9

            url = Regex.Replace(url, reg, "", RegexOptions.IgnoreCase);

            return url.Replace(" ", "").ToLower();
        }

        public static string GetCompareUrl(List<Tuple<int, string>> data)
        {
            if (data.Any())
            {
                string compareUrl = string.Empty;
                int pairCount = 1;
                foreach (var pair in data.OrderByDescending(item => item.Item1))
                {
                    compareUrl += string.Format("{0}{1}", pair.Item2, (pairCount < data.Count ? "-vs-" : ""));
                    pairCount++;
                }
                return compareUrl;
            };
            return string.Empty;
        }

        public static string Numeric(string numberToFormat)
        {
            decimal decimalNumber;
            string formatted = string.Empty;
            if (decimal.TryParse(numberToFormat, out decimalNumber))
            {
                formatted = numberToFormat.Contains('.') ? string.Format(cultureInfo, "{0:#,0.00}", decimalNumber) : string.Format(cultureInfo, "{0:#,0}", decimalNumber);
            }
            return formatted;
        }

        /// <summary>
        /// Removes HTML tags from content passed. Replaces special characters in HTML with appropriate symbols.
        /// Note : This function should be usefull in cases like formating of smallDescription where indentation of original data is not needed to maintain.
        /// Written By : Supriya on 25/6/2014
        /// </summary>
        /// <returns></returns>
        public static string RemoveHtmlTags(string content)
        {
            Dictionary<string, string> patternList = new Dictionary<string, string>();
            patternList.Add("<br>", "");                                    // removes <br> tag from content
            patternList.Add("<br/>", "");                                   // removes <br/> tag from content
            patternList.Add("<br />", "");                                  // removes <br /> tag from content
            patternList.Add("<div.*?>(.*?)</div>", "$1");                   // removes <div> </div> tag from content & maintain only string within it
            patternList.Add("<div.*?>(.*?)|</div>", "$1");                  // removes <div> </div> tag from content & maintain only string within it
            patternList.Add("<ul.*?>(.*?)</ul>", "$1");                     // removes <ul> </ul> tag from content & maintain only string within it
            patternList.Add("<ul.*?>(.*?)|</ul>", "$1");                    // removes <ul> </ul> tag from content & maintain only string within it
            patternList.Add("</li>", "");                                   // removes </li> tag from content
            patternList.Add("<p.*?>(.*?)", "$1");                           // removes <p> tag from content & maintain only string within it
            patternList.Add("<h2.*?>(.*?)</h2>", "$1");                     // removes <h2> </h2> tag from content & maintain only string within it
            patternList.Add("<h3.*?>(.*?)</h3>", "$1");                     // removes <h3> </h3> tag from content & maintain only string within it
            patternList.Add("<h1.*?>(.*?)</h1>", "$1");                     // removes <h1> </h1> tag from content & maintain only string within it
            patternList.Add("<P.*?>(.*?)", "$1");                           // removes <P> tag from content & maintain only string within it
            patternList.Add("</P>", "");                                    // removes </P> tag from content
            patternList.Add("</p>", "");                                    // removes </p> tag from content
            patternList.Add("<!--.*?if.*?>(.*?)|<.*?endif.*?>", "");        // removes 
            patternList.Add("<a.*?>(.*?)</a>", "$1");                       // removes <a> </a> tag from content & maintain only string within it
            patternList.Add("<a.*?>(.*?)|</a>", "$1");                      // removes <a> </a>tag from content & maintain only string within it
            patternList.Add(@"<img\s+[^>]*?src\s*=\s*[""']([^""']+)[""']\s*.*?/*>", "");      // removes <img /> tag from content 
            patternList.Add("<em.*?>(.*?)</em>", "$1");                     // removes <em> </em> tag from content & maintain only string within it
            patternList.Add("<span.*?>(.*?)</span>", "$1");                 // removes <span> </span> tag from content & maintain only string within it
            patternList.Add("<span.*?>(.*?)|</span>", "$1");                // removes <span> </span> tag from content & maintain only string within it
            patternList.Add("<strong.*?>(.*?)</strong>", "$1");             // removes <strong> </strong> tag from content & maintain only string within it
            patternList.Add("<strong.*?>(.*?)|</strong>", "$1");            // removes <strong> </strong> tag from content & maintain only string within it
            patternList.Add("<strike.*?>(.*?)</strike>", "$1");             // removes <strike> </strike> tag from content & maintain only string within it
            patternList.Add("<strike.*?>(.*?)|</strike>", "$1");            // removes <strike> </strike> tag from content & maintain only string within it
            patternList.Add("<font.*?>(.*?)</font>", "$1");                 // removes <font> </font> tag from content & maintain only string within it
            patternList.Add("<font.*?>(.*?)|</font>", "$1");                // removes <font> </font> tag from content & maintain only string within it
            patternList.Add(@"<object.*?>(.*?)</object>", "");              // removes <object> </object> tag from content
            patternList.Add(@"<object.*?>(.*?)|</object>", "");             // removes <object> </object> tag from content
            patternList.Add(@"<embed.*?>(.*?)|</embed>", "");               // removes <embed> </embed> tag from content
            patternList.Add(@"<script.*?>(.*?)|</script>", "");             // removes <script> </script> tag from content
            patternList.Add(@"<center.*?>(.*?)|</center>", "");             // removes <center> </center> tag from content
            patternList.Add(@"<!--(\n|.)*-->", "");
            patternList.Add("&nbsp;|\t|\r|\n", "");                         //removes &nbsp, \t, \r, \n tags from content
            patternList.Add("&rsquo;", "'");                                //removes &rsquo; from content
            patternList.Add("&lsquo;", "'");                                //removes &lsquo; from content
            patternList.Add("&ndash;", "-");                                //replaces &ndash; with - 
            patternList.Add("&lt;", "<");                                   //replaces &lt; with <
            patternList.Add("&gt;", ">");                                   //replaces &gt; with >
            patternList.Add("&amp;", "&");                                  //replaces &amp; with &
            patternList.Add("&pound;", "£");                                //replaces &pound; with £
            patternList.Add("&euro;", "€");                                 //replaces &euro; with €
            patternList.Add("&aacute;", "á");                               //replaces &aacute; with á
            patternList.Add("&uuml;", "ü");                                 //replaces &uuml; with ü
            patternList.Add("&ouml;", "ö");                                 //replaces &ouml; with ö
            patternList.Add("&eacute;", "é");                               //replaces &eacute; with é
            patternList.Add("&Prime;", "″");                                //replaces &Prime; with ″
            patternList.Add("&lrm;", " ");                                  //removes &lrm; 
            patternList.Add("&yacute;", "ý");                               //replaces &yacute; with ý
            patternList.Add("&ldquo;", "“");                                //replaces &ldquo; with “
            patternList.Add("&rdquo;", "”");                                //replaces &rdquo; with ”
            patternList.Add("&bull;", "-");                                 //replaces &bull; with -
            patternList.Add(@"<iframe\s+[^>]*?src\s*=\s*[""']([^""']+)[""']\s*.*?/*></iframe>", "$1");     // removes <iframe> </iframe> tag from content & maintain only src value of iframe tag

            foreach (KeyValuePair<string, string> pattern in patternList)
                content = Regex.Replace(content, pattern.Key, pattern.Value);

            return content;
        }
        public static string GetPlainTextFromHTML(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            string text = Regex.Replace(doc.DocumentNode.InnerText, @"\s+", " ").Trim().Replace("&nbsp;", " ");
            return HttpUtility.HtmlDecode(text);
        }
        /// <summary>
        /// returns numbers from input string
        /// eg i/p= "10 Am 45"      --- o/p= "1045"
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string getNumberFromString(string content)
        {
            string returnString = string.Empty;
            var chars = content.ToCharArray();

            foreach (char c in chars)
            {
                if (RegExValidations.IsNumeric(c.ToString())) returnString += c.ToString();
            }
            return returnString;
        }

        /// <summary>
        /// Added by : Purohith Guguloth on 17 Apr 2015
        /// Summary : To capitalize 1st letter of words
        /// </summary>
        /// <param ="va"></param>
        /// <returns></returns>
        public static string UppercaseWords(string value)
        {
            char[] array = value.ToLower().ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }   //End of UppercaseWords

        /// <summary>
        /// Function is used to Format price in lakhs and crores...
        /// </summary>
        /// <param name="price">price(e.g.200000)</param>
        /// <returns>It returns formatted number as 200,000</returns>
        public static string FormatFullPrice(string price, bool addRupeeSymbol = false)
        {
            string priceRange = string.Empty, tempPrice = string.Empty, decimalPart = string.Empty;

            if (!String.IsNullOrEmpty(price))
            {
                if (price.Contains(",") == true)
                {
                    price = price.Replace(",", "");
                }
                if (price.Contains("."))
                {
                    string[] priceWithDecimal = price.Split('.');
                    if (priceWithDecimal.Length == 2)
                    {
                        price = priceWithDecimal[0];
                        decimalPart = priceWithDecimal[1];
                    }
                    else
                        return "N/A";
                }

                if (price.Length >= 8) //when price in crore.
                {
                    tempPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(price) / 10000000).ToString()), 2, MidpointRounding.AwayFromZero));
                    if (tempPrice.Equals("1.00"))
                    {
                        priceRange = tempPrice.ToString() + "Cr";
                    }
                    else
                    {
                        priceRange = tempPrice.ToString() + "Cr";
                    }
                }

                else if ((price.Length >= 6) && (price.Length < 8)) //when price in lakhs.
                {
                    tempPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(price) / 100000).ToString()), 2, MidpointRounding.AwayFromZero));
                    priceRange = tempPrice.ToString() + "L";
                }
                else //when price in thousands.
                {
                    priceRange = Numeric(price);
                    if (decimalPart != string.Empty)
                        priceRange = string.Format("{0}.{1}", priceRange, decimalPart);
                }
            }
            else
            {
                priceRange = "N/A";
            }
            return (addRupeeSymbol ? _rupeeSymbol : null) + priceRange.Replace(".00", "");
        }

        /// <summary>
        /// Created By:Jitendra On 7 Jan 2013
        /// function is used to Format Price in comma seperate
        /// </summary>
        /// <param name="price">price (e.g.2,000)</param>
        /// <returns></returns>
        public static string FormatNumericCommaSep(string inputPrice)
        {
            var formattedPrice = string.Empty;
            int breakPoint = 3;
            for (int i = inputPrice.Length - 1; i >= 0; i--)
            {

                formattedPrice = inputPrice[i] + formattedPrice;
                if ((inputPrice.Length - i) == breakPoint && inputPrice.Length > breakPoint)
                {
                    formattedPrice = "," + formattedPrice;
                    breakPoint = breakPoint + 2;
                }
            }
            return formattedPrice;
        }

        /// <summary>
        /// Added by: Purohith Guguloth on 17th April, 2015
        /// summary: Takes the last updated date as input and returns the time span.
        /// </summary>
        /// <param name="LastUpdatedDate">The Last Updated date</param>
        /// <returns>Time Span Display in a format</returns>
        public static string GetDisplayTimeSpan(string LastUpdatedDate)
        {
            string retVal = "";
            TimeSpan tsDiff = DateTime.Now.Subtract(Convert.ToDateTime(LastUpdatedDate));

            if (tsDiff.Days >= 1)
                retVal = tsDiff.Days.ToString() + " day(s) ago";
            else if (tsDiff.Hours >= 1)
                retVal = tsDiff.Hours.ToString() + " hour(s) ago";
            else if (tsDiff.Minutes >= 1)
                retVal = tsDiff.Minutes.ToString() + " minute(s) ago";
            else if (tsDiff.Seconds >= 1)
                retVal = tsDiff.Seconds.ToString() + " second(s) ago";

            if (tsDiff.Days >= 365)
                retVal = Convert.ToString(tsDiff.Days / 365) + " year(s) ago";
            else if (tsDiff.Days > 30)
            {
                double months = Math.Floor(tsDiff.Days * 12 / 365.0);
                retVal = Convert.ToInt32(months).ToString() + " month(s) ago";
            }

            return retVal;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 27 Apr 2015
        /// Summary : to get Formatted 10 digit masking Number
        /// Modified By : Sadhana Upadhyay on 26 
        /// </summary>
        /// <param name="maskingNo"></param>
        /// <returns></returns>
        public static string GetFormattedMaskingNo(string maskingNo)
        {
            string maskingNumber = string.Empty;
            if (!String.IsNullOrEmpty(maskingNo) && maskingNo.Length >= 10)
                maskingNumber = maskingNo.Substring(maskingNo.Length - 10, 10);

            return maskingNumber;
        }

        public static string FormatPhoneNo(string number)
        {
            if (number.IndexOf("+91") == 0) number = number.Substring(3);
            if (number.Length == 10) number = number.Substring(0, 3) + " " + number.Substring(3, 3) + " " + number.Substring(6);
            return number;
        }

        /// <summary>
        /// written by : Ashwini on 5 Aug 2015
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string PriceLacCr(string price)
        {
            string priceRange = "N/A", tempPrice = string.Empty;
            if (!String.IsNullOrEmpty(price))
            {
                if (price.Contains(",") == true)
                {
                    price = price.Replace(",", "");
                }

                if (price.Length >= 8) //when price in crore.
                {
                    tempPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(price) / 10000000).ToString()), 2, MidpointRounding.AwayFromZero));
                    priceRange = tempPrice.ToString() + "Cr";
                }
                else if ((price.Length >= 6) && (price.Length < 8)) //when price in lakhs.
                {
                    tempPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(price) / 100000).ToString()), 2, MidpointRounding.AwayFromZero));
                    priceRange = tempPrice.ToString() + "L";
                }
                else //when price in thousands.
                {
                    priceRange = Convert.ToInt32(price) > 0 ? price : "N/A";
                }
            }
            else
            {
                priceRange = "N/A";
            }
            return priceRange.Replace(".00", "");
        }

        /// <summary>
        /// moved from Carwale.UI.Common.FormatPrice to Carwale.Utility.Format by Rakesh Yadav on 17 Nov 2015
        /// </summary>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns>will return formated price</returns>

        public static string FormatFullPrice(string minPrice, string maxPrice)
        {
            string priceRange = string.Empty, tempMinPrice = string.Empty, tempMaxPrice = string.Empty;

            if (!String.IsNullOrEmpty(minPrice) && !String.IsNullOrEmpty(minPrice))
            {
                if (minPrice.Contains(",") == true)
                    minPrice = minPrice.Replace(",", "");

                if (maxPrice.Contains(",") == true)
                    maxPrice = maxPrice.Replace(",", "");


                if (double.Parse(minPrice) == double.Parse(maxPrice))
                {
                    if (minPrice.Length >= 8) //when price in crore.
                    {
                        tempMinPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(minPrice) / 10000000).ToString()), 2, MidpointRounding.AwayFromZero));
                        priceRange = string.Format(c_strPriceInCrore, tempMinPrice);
                    }

                    else if ((minPrice.Length >= 6) && (minPrice.Length < 8)) //when price in lakhs.
                    {
                        tempMinPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(minPrice) / 100000).ToString()), 2, MidpointRounding.AwayFromZero));
                        priceRange = string.Format(c_strPriceInLakh, tempMinPrice);
                    }
                    else //when price in thousands.
                    {
                        priceRange = Convert.ToInt32(minPrice) > 0 ? minPrice : "N/A";
                    }
                }
                else
                {
                    if (minPrice.Length >= 8)  //when both min and max prices are in crores
                    {
                        tempMinPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(minPrice) / 10000000).ToString()), 2, MidpointRounding.AwayFromZero));
                        tempMaxPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(maxPrice) / 10000000).ToString()), 2, MidpointRounding.AwayFromZero));
                        priceRange = string.Format(c_strPriceFromCroreToCrore, tempMinPrice, tempMaxPrice);
                    }
                    else if ((minPrice.Length < 8) && (minPrice.Length >= 6) && (maxPrice.Length >= 8)) //when min price in lakhs and max price is in crores
                    {
                        tempMinPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(minPrice) / 100000).ToString()), 2, MidpointRounding.AwayFromZero));
                        tempMaxPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(maxPrice) / 10000000).ToString()), 2, MidpointRounding.AwayFromZero));
                        priceRange = string.Format(c_strPriceFromLakhsToCrore, tempMinPrice, tempMaxPrice);
                    }
                    else if ((minPrice.Length >= 6) && (maxPrice.Length < 8) && (minPrice.Length < 8)) //when min ans max prices are in lakhs
                    {
                        tempMinPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(minPrice) / 100000).ToString()), 2, MidpointRounding.AwayFromZero));
                        tempMaxPrice = String.Format("{0:0.00}", Math.Round(double.Parse((double.Parse(maxPrice) / 100000).ToString()), 2, MidpointRounding.AwayFromZero));
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
                priceRange = "N/A";
            }

            return priceRange.Replace(c_strPriceAfterDot, string.Empty);
        }

        public static string LimitText(string text, int limitLength)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = Regex.Replace(text, @"<[^>]+>", string.Empty);

                if (text.Length < limitLength)
                    return text;
                else
                {
                    text = text.Substring(0, limitLength);
                    int lastWhiteSpace = text.LastIndexOf(" ");
                    if (lastWhiteSpace != -1) text = text.Substring(0, lastWhiteSpace);
                    return text + "...";
                }
            }
            return string.Empty;
        }
        public static string LimitTextWithoutDotAtTheEnd(string text, int limitLength)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = Regex.Replace(text, @"<[^>]+>", string.Empty);

                if (text.Length < limitLength)
                    return text;
                else
                {
                    text = text.Substring(0, limitLength);
                    int lastWhiteSpace = text.LastIndexOf(" ");
                    if (lastWhiteSpace != -1) text = text.Substring(0, lastWhiteSpace);
                    return text;
                }
            }
            return string.Empty;
        }

        public static string RemoveBasicHtmlTags(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? string.Empty : Regex.Replace(text, "<(.|\n)+?>", string.Empty);
        }

        public static string GetAddress(string address, string area, string city, string state, string pinCode)
        {
            string add = "";

            string strCity = city;

            if (address != "")
                add = "Address: " + address;

            if (area != "")
            {
                if (add == "")
                    add = area;
                else
                    add += ",<br>" + area;
            }

            if (city != "")
            {
                if (add == "")
                    add = city;
                else
                    add += ",<br>" + city;
            }

            if (state != "")
            {
                if (add == "")
                    add = state;
                else
                    add += ", " + state;
            }

            if (pinCode != "")
            {
                if (add == "")
                    add = pinCode;
                else
                    add += ", " + pinCode;
            }
            if (add != "")
            {
                add = "<p class='font14 text-light-grey'>" + add + "</p>";
            }
            return add;
        }



        public static string GetPrice(string price)
        {
            if (price != "")
            {
                double _price = Convert.ToDouble(price) / 100000.0;
                if (_price < 1)
                {
                    return "NA";
                }
                else if (_price < 100)
                    return "₹ " + Convert.ToString(Math.Round(Convert.ToDecimal(_price), 2)) + "L";
                else
                {
                    _price = _price / 100;
                    return "₹ " + Convert.ToString(Math.Round(Convert.ToDecimal(_price), 2)) + "Cr";
                }
            }
            else
                return "";
        }

        public static string GetAbsReviewRate(double reviewRate)
        {

            int absVal = (int)Math.Floor(reviewRate);   //returns mod value 3.9 --> 3
            //absVal  = 3
            int i;
            float rRate = 0.0f;
            for (i = 0; i < absVal; i++)
                rRate += 1;
            //appends 3 coloured stars here
            if (reviewRate > absVal)
                rRate += 0.5f;                //appends 1 half coloured star here
            else
                i--;

            //appends 1 star with plain colour//

            return rRate.ToString();

        }

        public static string FilterModelName(string modelName)
        {
            if (String.IsNullOrWhiteSpace(modelName))
            {
                return String.Empty;
            }
            var index = modelName.IndexOf('[');
            if (index >= 0)
            {
                return modelName.Substring(0, index).Trim();
            }
            else
            {
                return modelName;
            }
        }

        public static string FormatSpecial(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            string reg = @"[^/\-0-9a-zA-Z\s]*"; // everything except a-z, 0-9, / and -

            url = Regex.Replace(url, reg, "", RegexOptions.IgnoreCase);

            return url.Replace(" ", "").Replace("-", "").Replace("/", "").ToLower();
        }

        public static string FormatUrlColor(string url)
        {
            return Regex.Replace(url.ToLower(), @"[^0-9a-zA-Z]+", "-");
        }

        public static string FormatText(string text)
        {
            return text.Replace(" ", "").ToLower();
        }

        public static string GetFuelEconomy(string mileage)
        {
            string returnVal = "";

            if (mileage.ToString() != "0" && mileage.ToString() != "")
            {
                returnVal = mileage.ToString() + " kpl";
            }

            return returnVal;
        }

        public static string PurchasedAs(string isOwned, string isNewlyPurchased)
        {
            string returnVal = "";

            if (isOwned == "No")
            {
                returnVal = "Not Purchased";
            }
            else if (isNewlyPurchased == "Yes")
            {
                returnVal = "New";
            }
            else if (isNewlyPurchased == "No")
            {
                returnVal = "Used";
            }
            else
            {
                returnVal = "";
            }

            return returnVal;
        }


        public static string XMLSerialize<T>(T data)
        {
            string dataXML;
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    var serializer = new XmlSerializer(data.GetType());
                    serializer.Serialize(writer, data);
                    writer.Flush();
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataXML = reader.ReadToEnd();
                    }
                }
            }
            return dataXML;
        }


        public static string ConvertDataTableToString(DataTable dt)
        {
            if (dt == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            for (int rowId = 0; rowId < dt.Rows.Count; rowId++)
            {

                for (int colId = 0; colId < dt.Columns.Count; colId++)
                {
                    sb.Append(dt.Rows[rowId][colId].ToString());
                    if (colId < dt.Columns.Count - 1)
                        sb.Append("#c0l#");
                }
                if (rowId < dt.Rows.Count - 1)
                    sb.Append("|r0w|");
            }
            return sb.ToString();
        }

        public static string GetValueInINR(decimal? emiValue)
        {
            if (emiValue > 0 && emiValue != null)
            {
                return emiValue.Value.ToString(Decimal.Round(emiValue.Value) == emiValue.Value ? "c0" : "c", new CultureInfo("EN-in"));
            }
            return null;
        }

        // will format the a name to make it URL compatible 
        //only for yures 
        public static string FormatTyresURL(string url)
        {
            url = url.Replace(" ", "-").Replace("--", "-");
            return url.ToLower();
        }
        public static string GetFormattedPriceV2(string p1, string p2 = null, string format = null, bool abbreviateUnits = false)
        {
            string p1units = c_strLakhs, p2units = c_strCrores;

            if (RegExValidations.IsNumeric(p1))
            {
                int p1length = PriceLength(p1);
                if (p1length >= 8)
                {
                    p1 = Math.Round((double.Parse(p1) / 10000000), 2, MidpointRounding.AwayFromZero).ToString("0.##");
                    p1units = abbreviateUnits ? c_strAbbreviatedCrores : c_strCrores;
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
                    return p2 == c_strNotApplicable || p1 == c_strNotApplicable ? string.Format(format ?? c_strSinglePriceFormat, p1, p1units) : string.Format(format ?? c_strDoublePriceFormatSeperateUnits, p1, p1units, p2, p2units);
                }
                else
                {
                    return p1 == p2 ? string.Format(format ?? c_strSinglePriceFormat, p1, p1units) : string.Format(format ?? c_strDoublePriceFormat, p1, p2, p2units);
                }
            }
            else
            {
                return string.Format(format ?? c_strSinglePriceFormat, p1, p1units);
            }
        }
        public static int PriceLength(string price)
        {
            string[] s = new string[2];
            s = price.Split('.');
            return (s[0].Length);
        }

        public static string FormatSpecificationData(string value)
        {
            return value.Replace("CUSTOM", String.Empty).Replace("BIT", String.Empty).Replace(@"~", " ");
        }
        public static string GetUpcomingFormatPrice(double minprice, double maxprice)
        {
            double fullminprice = (minprice * 100000);
            double fullmaxprice = (maxprice * 100000);
            return Format.GetFormattedPriceV2(fullminprice.ToString(), fullmaxprice.ToString());
        }
        public static string GetNumberSuffix(int number)
        {
            int ordinalNumber;
            ordinalNumber = number % 100;
            if (ordinalNumber >= 11 && ordinalNumber <= 13)
            {
                return "th";
            }
            switch (ordinalNumber % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
        public static string SecondsToTime(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"mm\:ss");
        }
        public static string GetNumberInKilos(int number)
        {
            if (number < 1000)
                return number.ToString();
            int decimalPoint = (number % 1000) / 100;
            return string.Format("{0}{1} K", number / 1000, decimalPoint > 0 ? "." + decimalPoint : string.Empty);
        }
        public static string TrimCcFromDisplacement(string displacement)
        {
            if (string.IsNullOrEmpty(displacement))
            {
                return string.Empty;
            }
            string word = displacement.Trim();
            char[] charsToTrim = { 'c' };
            return word.TrimEnd(charsToTrim);
        }
        // returns range Text for two positive values
        public static string GetRangeText(double lowest, double highest, string units)
        {
            if ((lowest > 0 || highest > 0) && !(highest - lowest < 0))
            {
                if (lowest <= 0 || (highest - lowest < 0.01))
                {
                    return string.Format("{0} {1}", highest, units ?? string.Empty);
                }
                return string.Format("{0} to {1} {2}", lowest, highest, units ?? string.Empty);
            }
            return string.Empty;
        }
        // return gramatically comma separated string from list of strings 
        // example: 
        // input: { A,B,C,D }
        // output: "A, B, C and D"
        public static string GetSentenceFromList(List<string> words)
        {
            if(words.IsNotNullOrEmpty())
            {
                if(words.Count == 1)
                {
                    return words[0];
                }
                if(words.Count == 2)
                {
                    return string.Format("{0} and {1}", words[0] , words[1]);
                }
                StringBuilder stringList = new StringBuilder();
                int i;
                for(i=0;i<words.Count-2;i++)
                {
                    stringList.AppendFormat("{0}, ", words[i]);
                }
                stringList.AppendFormat("{0} and {1}", words[i], words[words.Count-1]);
                return stringList.ToString();
            }
            return string.Empty;
        }
    }//class
}//namespace
