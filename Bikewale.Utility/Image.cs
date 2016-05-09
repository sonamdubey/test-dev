using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    /// <summary>
    /// Modified By : Lucky Rathore on 09 May 2016
    /// Description : Add BikewaleLogo
    /// </summary>
    public class Image
    {
        public static string BikewaleLogo = "http://imgd1.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-logo.png";
        public static string GetPathToShowImages(string originalImagePath, string hostUrl, string size)
        {
            string imgUrl = String.Empty;
            if (!String.IsNullOrEmpty(originalImagePath) && !String.IsNullOrEmpty(hostUrl))
            {
                imgUrl = String.Format("{0}/{1}/{2}", hostUrl, size, originalImagePath);
            }
            else
            {
                imgUrl = "http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/nobike.jpg";
            }
            return imgUrl;
        }
    }

    public class ImageSize
    {
        //For 1024 resolution
        //(when grid has a width of 996 px, 10 px margin on left and right, 12 columns)
        public const string _144x81 = "144x81";
        public const string _227x128 = "227x128";
        public const string _310x174 = "310x174";
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

        public const string _110x61 = "110x61";
        public const string _160x89 = "160x89";
        public const string _640x348 = "640x348";
        public const string _210x118 = "210x118";
    }
}
