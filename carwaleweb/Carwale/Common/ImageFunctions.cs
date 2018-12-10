// Mails Class
//
using System;
using System.Web;
using System.Configuration;
using System.Web.Mail;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Carwale.UI.Common
{
    public class ImageFunctions
    {
        public static void GenerateThumbnail(string savedLocation, string targetLocation, int desiredWidth, int desiredHeight)
        {
            // Get dimentions of the uploaded image.
            System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(savedLocation);
            SizeF sz = imgOriginal.PhysicalDimension;

            int width = Convert.ToInt32(sz.Width); // width of the uploaded temp image
            int height = Convert.ToInt32(sz.Height); // height of the uploaded temp image

            imgOriginal.Dispose(); // clean up.

            // is the proportion right? If proportion is right,
            // don't need to worry about resizing the image. 
            // desired image size can be used immediately.
            if (width * desiredHeight == height * desiredWidth)
            {
                height = desiredHeight;
                width = desiredWidth;
            }
            // is width more than height? width would be the base parameter.
            else if (width >= height)
            {
                height = height * desiredWidth / width; // new height will have to be calculated.
                width = desiredWidth;
            }
            else if (height > width) // height is more than width
            {
                width = width * desiredHeight / height;
                height = desiredHeight;
            }

            System.Drawing.Image img = System.Drawing.Image.FromFile(savedLocation);
            System.Drawing.Image thumbnail = new Bitmap(width, height);
            Graphics graphic = Graphics.FromImage(thumbnail);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            graphic.DrawImage(img, rectangle);
            thumbnail.Save(targetLocation, ImageFormat.Jpeg);
            img.Dispose();
        }

        public static void GenerateSquareThumbnail(string savedLocation, string targetLocation, int size)
        {
            // Get dimentions of the uploaded image.
            System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(savedLocation);
            SizeF sz = imgOriginal.PhysicalDimension;

            int width = Convert.ToInt32(sz.Width); // width of the uploaded temp image
            int height = Convert.ToInt32(sz.Height); // height of the uploaded temp image

            int x = 0, y = 0; // image start co-ordinates. needed for square image.

            // is width more than height? width would be the base parameter.
            if (width > height)
            {
                width = (int)width * size / height;
                height = size;
                // moves cursor so that crop is more centered 
                x = Convert.ToInt32(Math.Ceiling((double)((width - height) / 2)));
            }
            else if (height < width) // height is less than width
            {
                height = (int)height * size / width; // new height will have to be calculated.
                width = size;
                // moves cursor so that crop is more centered 
                y = Convert.ToInt32(Math.Ceiling((double)((height - width) / 2)));
            }
            else
            {
                width = size;
                height = size;
            }

            //First Resize the Existing Image 
            System.Drawing.Image thumbNailImg;
            thumbNailImg = imgOriginal.GetThumbnailImage(width, height, null, System.IntPtr.Zero);
            //Clean up / Dispose... 
            imgOriginal.Dispose();

            //Create a Crop Frame to apply to the Resized Image 
            Bitmap myBitmapCropped = new Bitmap(size, size);
            Graphics myGraphic = Graphics.FromImage(myBitmapCropped);

            //Apply the Crop to the Resized Image 
            myGraphic.DrawImage(thumbNailImg, new Rectangle(0, 0, myBitmapCropped.Width,
                myBitmapCropped.Height), x, y, myBitmapCropped.Width, myBitmapCropped.Height,
                GraphicsUnit.Pixel);
            //Clean up / Dispose... 
            myGraphic.Dispose();

            //Save the Croped and Resized image as a new square thumnail 
            myBitmapCropped.Save(targetLocation, ImageFormat.Jpeg);

            //Clean up / Dispose... 
            thumbNailImg.Dispose();
            myBitmapCropped.Dispose();
        }        
    }//class
}//namespace