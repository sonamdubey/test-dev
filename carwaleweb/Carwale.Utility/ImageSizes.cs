using System;
using System.Text.RegularExpressions;

namespace Carwale.Utility
{
    public static class ImageSizes
    {
        //All the image sizes that are allowed should be mentioned as static string here

        #region 16:9
        public const string _0X0 = "0x0";
        public const string _110X61 = "110x61";
        public const string _160X89 = "160x89";
        public const string _640X348 = "640x348";
        public const string _210X118 = "210x118";
        public const string _310X174 = "310x174";
        public const string _559X314 = "559x314";
        public const string _393X221 = "393x221";
        public const string _144X81 = "144X81";
        public const string _227X128 = "227x128";
        public const string _476X268 = "476x268";
        public const string _642X361 = "642x361";
        public const string _725X408 = "725x408";
        public const string _808X455 = "808x455";
        public const string _891X501 = "891x501";
        public const string _600X337 = "600x337";
        public const string _199X112 = "199x112";
        public const string _174X98 = "174x98";
        public const string _272X153 = "272x153";
        public const string _370X208 = "370x208";
        public const string _468X263 = "468x263";
        public const string _566X318 = "566x318";
        public const string _664X374 = "664x374";
        public const string _762X429 = "762x429";
        public const string _860X484 = "860x484";
        public const string _958X539 = "958x539";
        public const string _1056X594 = "1056x594";
        public const string _530x170 = "530x170";
        public const string _940X300 = "940x300";
        public const string _80X52 = "80x52";
        public const string _357X114 = "357x114";
        public const string _1280x720 = "1280x720";
        public const string _424x424 = "424x424";
        public const string _211x211 = "211x211";
        #endregion

        #region 4:3
        public const string _300x225 = "300x225";
        #endregion


        public static readonly ushort defaultImageQuality = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["DefaultImageQuality"] ?? "85");
        public static readonly string _defaultImageUrl = System.Configuration.ConfigurationManager.AppSettings["DefaultImageUrl"];

        //A static class for Creating Image URL with size.
        public static string CreateImageUrl(string hostUrl, string size, string originaImgPath, ushort quality = 0)
        {
            if (quality == 0)
            {
                quality = defaultImageQuality;
            }

            Regex domainMatch = new Regex("imgd([0-9])*");
            if (!string.IsNullOrEmpty(hostUrl) && !string.IsNullOrEmpty(originaImgPath))
            {
                return string.Format(@"{0}{1}{2}{3}", (domainMatch.IsMatch(hostUrl) ? CWConfiguration._imgHostUrl : hostUrl), size, originaImgPath, originaImgPath.Contains("?") ? "&q=" + quality : "?q=" + quality
                );
            }
            else
            {
                return string.Format(@"{0}{1}{2}", CWConfiguration._imgHostUrl, size, _defaultImageUrl);
            }
        }
    }
}
