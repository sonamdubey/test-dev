using System;
using System.Configuration;
using System.Text;
using System.Web;

namespace Bikewale.Utility
{
    public static class Image
    {
        /// <summary>
        /// Modified by : Sanskar Gupta on 26 March 2018
        /// Description : Added Handling for `//` in Image URL.
        /// </summary>
        public static string GetPathToShowImages(string originalImagePath, string hostUrl, string size)
        {
            string imgUrl = String.Empty;
            if (!String.IsNullOrEmpty(originalImagePath) && !String.IsNullOrEmpty(hostUrl))
            {
                StringBuilder urlSb = new StringBuilder();
                if (hostUrl.EndsWith("/")) {
                    urlSb.Append(string.Format("{0}{1}", hostUrl, size));
                }
                else
                {
                    urlSb.Append(string.Format("{0}/{1}", hostUrl, size));
                }

                if (originalImagePath.StartsWith("/"))
                {
                    urlSb.Append(string.Format("{0}", originalImagePath));
                }
                else
                {
                    urlSb.Append(string.Format("/{0}", originalImagePath));
                }

                imgUrl = urlSb.ToString();
            }
            else
            {
                imgUrl = string.Format("https://imgd.aeplcdn.com/{0}/bikewaleimg/images/noimage.png", size);
            }
            return imgUrl;
        }

        /// <summary>
        /// Modified by : Sanskar Gupta on 26 March 2018
        /// Description : Added Handling for `//` in Image URL.
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="hostUrl"></param>
        /// <param name="size"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static string GetPathToShowImages(string originalImagePath, string hostUrl, string size, string quality)
        {
            string imgUrl = String.Empty;
            if (!String.IsNullOrEmpty(originalImagePath) && !String.IsNullOrEmpty(hostUrl))
            {
                StringBuilder urlSb = new StringBuilder();
                if (hostUrl.EndsWith("/"))
                {
                    urlSb.Append(string.Format("{0}{1}", hostUrl, size));
                }
                else
                {
                    urlSb.Append(string.Format("{0}/{1}", hostUrl, size));
                }

                if (originalImagePath.StartsWith("/"))
                {
                    urlSb.Append(string.Format("{0}", originalImagePath));
                }
                else
                {
                    urlSb.Append(string.Format("/{0}", originalImagePath));
                }
                if (!String.IsNullOrEmpty(quality))
                {
                    if (originalImagePath.IndexOf("?") > -1)
                        urlSb.Append(string.Format("&q={0}", quality));
                    else
                        urlSb.Append(string.Format("?q={0}", quality));
                }

                imgUrl = urlSb.ToString();
            }
            else
            {
                imgUrl = string.Format("https://imgd.aeplcdn.com/{0}/bikewaleimg/images/noimage.png?q={1}", size, quality);
            }
            return imgUrl;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 23/8/2012
        /// This function will return true is file name has one of follwing extension JPEG, GIF, PNG. Else will return false        
        /// </summary>
        /// <param name="imageFileName">name of the image file</param>
        /// <returns></returns>
        public static bool IsValidFileExtension(string imageFileName, out string extension)
        {
            bool isImageFileName = false;

            try
            {
                // Extract extension from file name.
                int lastDotIndex = imageFileName.LastIndexOf('.');
                string fileExtension = imageFileName.Substring(lastDotIndex, imageFileName.Length - lastDotIndex).ToLower();

                // Check the extension
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".gif" || fileExtension == ".png")
                {
                    extension = fileExtension;
                    isImageFileName = true;
                }
                else
                {
                    extension = String.Empty;
                }
            }
            finally
            {

            }

            return isImageFileName;
        }

        public static string GetPathToSaveImages(string relativePath)
        {
            string physicalPath = string.Empty;

            if (HttpContext.Current.Request["HTTP_HOST"].IndexOf("localhost") >= 0)
            {
                physicalPath = HttpContext.Current.Request["APPL_PHYSICAL_PATH"].ToLower() + relativePath;
            }
            else
            {
                physicalPath = ConfigurationManager.AppSettings["imgPathFolder"] + relativePath;
            }
            return physicalPath;
        }

    }

    public static class ImageSize
    {
        //For 1024 resolution
        //(when grid has a width of 996 px, 10 px margin on left and right, 12 columns)
        public const string _144x81 = "144x81";
        public const string _227x128 = "227x128";
        public const string _310x174 = "310x174";
        public const string _360x202 = "360x202";
        public const string _393x221 = "393x221";
        public const string _476x268 = "476x268";
        public const string _559x314 = "559x314";
        public const string _642x361 = "642x361";
        public const string _725x408 = "725x408";
        public const string _808x455 = "808x455";
        public const string _891x501 = "891x501";

        //For 1366 resolution
        //(when grid has a width of 1176 px, 10 px margin on left and right, 12 columns)
        public const string _174x98 = "174x98";
        public const string _272x153 = "272x153";
        public const string _370x208 = "370x208";
        public const string _468x263 = "468x263";
        public const string _566x318 = "566x318";
        public const string _664x374 = "664x374";
        public const string _762x429 = "762x429";
        public const string _860x484 = "860x484";
        public const string _958x539 = "958x539";
        public const string _1056x594 = "1056x594";
        public const string _1280x720 = "1280x720";

        public const string _110x61 = "110x61";
        public const string _160x89 = "160x89";
        public const string _640x348 = "640x348";
        public const string _210x118 = "210x118";
    }


    public static class QualityFactor
    {
        public const string _60 = "60";
        public const string _65 = "65";
        public const string _70 = "70";
        public const string _75 = "75";
        public const string _80 = "80";
        public const string _85 = "85";
        public const string _90 = "90";
    }
}
