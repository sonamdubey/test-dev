// Mails Class
//
using System;
using System.Web;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Carwale.Notifications;

namespace Carwale.UI.Common
{
	public class ImagingFunctions
	{	
		public static void ResizeImage( string savedLocation, string targetLocation, int desiredWidth, int desiredHeight )
		{
			// Get dimentions of the uploaded image.
			Image imgOriginal = Image.FromFile( savedLocation );
			
			Size sz = GetDimensions(desiredWidth, desiredHeight, ref imgOriginal);
			
			System.Drawing.Image img = System.Drawing.Image.FromFile(savedLocation);
			System.Drawing.Image thumbnail = new Bitmap(sz.Width, sz.Height);
			Graphics graphic = Graphics.FromImage(thumbnail);
			graphic.CompositingQuality = CompositingQuality.HighQuality;
			graphic.SmoothingMode = SmoothingMode.HighQuality;
			graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
			Rectangle rectangle = new Rectangle( 0, 0, sz.Width, sz.Height );
			graphic.DrawImage(img, rectangle);
			thumbnail.Save( targetLocation, ImageFormat.Jpeg );
			img.Dispose(); 
		}
		
		public static Size GetDimensions(int maxWidth, int maxHeight, ref Image Img)
		{		
			int height; int width; float multiplier;
		
			height = Img.Height; width = Img.Width;
			
			Img.Dispose();
			
			if (height <= maxHeight && width <= maxWidth)			
				return new Size(width, height);
			
			multiplier = (float)((float)maxWidth / (float)width);
			
			if ((height * multiplier) <= maxHeight)			
			{			
				height = (int)(height * multiplier);			
				return new Size(maxWidth, height);			
			}
			
			multiplier = (float)maxHeight / (float)height;
			
			width = (int)(width * multiplier);						
			
			return new Size(width, maxHeight);		
		}			

        public static string GetImagePath(string relativePath, string hostUrl)
        {
            string absolutePath = "";

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
                absolutePath = "https://" + hostUrl + relativePath;
            else
                absolutePath = "https://webserver:8083" + relativePath;

            return absolutePath;
        }

        // Added By : Ashish G. Kamble on 20/4/2012
        // Function to return the image path for carwale. If website is live then img.carwale or path is webserver:8083
        public static string GetRootImagePath()
        {
            return ConfigurationManager.AppSettings["imgRootPath"];
        }
    }//class
}//namespace