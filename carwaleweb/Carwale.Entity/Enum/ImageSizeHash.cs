using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Enum
{
    public static class ImageSizeHash
    {
        private static Hashtable GetHashImageData()
        {
            Hashtable imageSize = new Hashtable();
            imageSize.Add("Small", "200x400");
            imageSize.Add("XLarge", "123");
            imageSize.Add("Large", "2324");
            imageSize.Add("Thumb", "110x65");
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
