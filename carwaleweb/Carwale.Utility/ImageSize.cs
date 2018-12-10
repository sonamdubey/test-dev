using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    public static class ImageSize
    {
        private static Hashtable GetHashImageData()
        {
            Hashtable imageSize = new Hashtable();

            imageSize.Add("Small", "160x90");
            imageSize.Add("XLarge", "600X450");
            imageSize.Add("Large", "300x250");
            imageSize.Add("CustomSize", "199x112");
            imageSize.Add("PhotosLarge", "600x337");

            return imageSize;
        }

        public static string GetImageSize(string imgSizeText)
        {
            Hashtable hashtable = GetHashImageData();
            string value = "";
            value = (string)hashtable[imgSizeText];
            value = (value != "" ? value : "");
            return value;
        }
    }
}
