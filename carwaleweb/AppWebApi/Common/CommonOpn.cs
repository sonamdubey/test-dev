using Carwale.DAL.Security;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;

namespace AppWebApi.Common
{
    public class CommonOpn
	{
		private HttpContext objTrace = HttpContext.Current;
		
        public static bool CheckId( string input )
        {
            bool retVal = false;
            try
            {
                //check with the regular expression
                if(Regex.IsMatch(input, @"^[0-9]+$") == true)
                {
                    //check its length
                    if(input.Length <=12)
                    {
                        retVal = true;
                    }
                    else
                    {
                        retVal = false;
                    }
                }
                else
                {
                    retVal = false;
                }
            }
            catch(Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
			
            return retVal;
        } // CheckId

        public static bool CheckValidId(string input)
        {
            bool retVal = false;
            try
            {
                if (input.Trim() == "-1" || CheckId(input))
                    retVal = true;
                else
                    retVal = false;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return retVal;
        }


        public static bool ValidateData(params string[] inputs)
        {
            bool retVal = true;
            foreach (var input in inputs)
            {
                string[] _arr = input.ToString().Split(',');

                for (int i = 0; i < _arr.Length; i++)
                {
                    if (!CheckValidId(_arr[i]))
                    {
                        retVal = false;
                        break;
                    }
                }

                if (retVal == false)
                    break;
            }
            return retVal;
        }
        
        public static string RemoveAnchorTag(string str)
        {
            str = Regex.Replace(str, @"<a [^>]+>(.*?)<\/a>", "$1");
            return str;
        }

        public static string ApiHostUrl
        {
            get { return ConfigurationManager.AppSettings["WebApiHostUrl"].ToString(); }
        }
        
        public static string GetPrice(string price)
        {
            if (price != "")
            {
                double _price = Convert.ToDouble(price) / 100000.0;
                if (_price < 100)
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

        public static string GetPriceInLakhs(string price)
        {
            if (price != "")
            {
                double _price = Convert.ToDouble(price) / 100000.0;

                return "₹ " + Convert.ToString(Math.Round(Convert.ToDecimal(_price), 2)) + "L";
            }
            else
                return "";
        }
        
        public static bool SkipValidation
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["SkipValidation"].ToString()); }
        }
        
        public static string GetAbsReviewRate(double reviewRate)
        {

            int absVal = (int)Math.Floor(reviewRate);   //returns mod value 3.9 --> 3
            //absVal  = 3
            int i;
            float rRate = 0.0f;
            for (i = 0; i < absVal; i++)
                rRate += 1;            //appends 3 coloured stars here

            if (reviewRate > absVal)
                rRate += 0.5f;                //appends 1 half coloured star here
            else
                i--;

              //appends 1 star with plain colour//

            return rRate.ToString();

        }

        public static bool IsValidSource(string SourceId, string CWK)
        {
            SecurityRepository<bool> securityRepo = new SecurityRepository<bool>();
            return securityRepo.IsValidSource(SourceId, CWK);
        }

        public static string GetDate(string _displayDate)
        {
            string retVal = "";
            TimeSpan tsDiff = DateTime.Now.Subtract(Convert.ToDateTime(_displayDate));

            if (tsDiff.Days > 0)
            {
                retVal = tsDiff.Days.ToString();
                if (retVal == "1")
                    retVal += " day ago";
                else
                    retVal += " days ago";
            }
            else if (tsDiff.Hours > 0)
            {
                retVal = tsDiff.Hours.ToString();

                if (retVal == "1")
                    retVal += " hour ago";
                else
                    retVal += " hours ago";
            }
            else if (tsDiff.Minutes > 0)
            {
                retVal = tsDiff.Minutes.ToString();

                if (retVal == "1")
                    retVal += " minute ago";
                else
                    retVal += " minutes ago";
            }
            else if (tsDiff.Seconds > 0)
            {
                retVal = tsDiff.Seconds.ToString();

                if (retVal == "1")
                    retVal += " second ago";
                else
                    retVal += " seconds ago";
            }

            if (tsDiff.Days > 360)
            {
                retVal = Convert.ToString(tsDiff.Days / 360);

                if (retVal == "1")
                    retVal += " year ago";
                else
                    retVal += " years ago";
            }
            else if (tsDiff.Days > 30)
            {
                retVal = Convert.ToString(tsDiff.Days / 30);

                if (retVal == "1")
                    retVal += " month ago";
                else
                    retVal += " months ago";
            }

            return retVal;
        }
        
        //Function to Format Url Added by Supriya on 10/6/2014 
        public static string FormatSpecial(string url)
        {
            string reg = @"[^/\-0-9a-zA-Z\s]*"; // everything except a-z, 0-9, / and -

            url = Regex.Replace(url, reg, "", RegexOptions.IgnoreCase);

            return url.Replace(" ", "").Replace("-", "").Replace("/", "").ToLower();
        }
        
     public static string GetArticleUrlForApp(string categoryId,string articleUrl,string basicId,string modelMaskingName,string makeName)
        {

             string url = string.Empty;
            
                switch(categoryId)
                {
                    case "1": url = "https://www.carwale.com/news/" + basicId + "-" + articleUrl + "/";
                        break;
                    case "2": url = "https://www.carwale.com/expert-reviews/" + articleUrl + "-" + basicId + "/";
                        break;
                    case "6": url = "https://www.carwale.com/features/" + articleUrl + "-" + basicId + "/";
                        break;
                    case "8": url = "https://www.carwale.com/" + Carwale.Utility.Format.FormatSpecial(makeName) + "-cars/" + modelMaskingName + "/expert-reviews-" + basicId + "/";
                       break;
                    case "19":url = "https://www.carwale.com/news/" + basicId + "-" + articleUrl + "/";
                        break;
                }
                return url;
        }
	}
}	