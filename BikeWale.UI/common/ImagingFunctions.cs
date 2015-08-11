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
using System.Web.UI.HtmlControls;

namespace Bikewale.Common
{
    public class ImagingFunctions
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

        // will apply watermark to a given image.
        // please note, do not attempt to apply watermark to a very small image.
        // all paths have to be physical ones.
        public static void AddWatermark(string sourceFile, string outputFile, string watermarkSource, int outputWidth, int outputHeight)
        {
            AddWatermark(sourceFile, outputFile, watermarkSource, outputWidth, outputHeight, 1);
        }

        // will apply watermark to a given image.
        // please note, do not attempt to apply watermark to a very small image.
        // all paths have to be physical ones.
        //imagePosition : 1 for top right and 2 for bottom right
        public static void AddWatermark(string sourceFile, string outputFile, string watermarkSource, int outputWidth,
                                            int outputHeight, int imagePosition)
        {
            // resizing

            System.Drawing.Image img = System.Drawing.Image.FromFile(sourceFile);

            System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(sourceFile);
            SizeF sz = imgOriginal.PhysicalDimension;

            int width = Convert.ToInt32(sz.Width); // width of the uploaded temp image
            int height = Convert.ToInt32(sz.Height); // height of the uploaded temp image

            HttpContext.Current.Trace.Warn(" Width " + width.ToString());
            HttpContext.Current.Trace.Warn(" height " + height.ToString());

            //double ratioOrig = Convert.ToDouble(width)/Convert.ToDouble(height);

            //double outRatio = Convert.ToDouble(outputWidth)/Convert.ToDouble(outputHeight);

            //HttpContext.Current.Trace.Warn("Original Ratio : " + ratioOrig.ToString());
            //HttpContext.Current.Trace.Warn("Required Ratio : " + outRatio.ToString());

            HttpContext.Current.Trace.Warn("--Disposing the image");
            imgOriginal.Dispose(); // clean up.

            // is the proportion right? If proportion is right,
            // don't need to worry about resizing the image. 
            // desired image size can be used immediately.
            if (width * outputHeight == height * outputWidth)
            {
                height = outputHeight;
                width = outputWidth;
            }
            else if (width > outputWidth)// is original Width more than output width? make the width equal to the output width
            {
                height = height * outputWidth / width; // new height will have to be calculated.
                width = outputWidth;
            }

            HttpContext.Current.Trace.Warn("Creating object of thumbnail");
            System.Drawing.Image thumbnail = new Bitmap(width, height);

            HttpContext.Current.Trace.Warn("Creating object of graphics");
            Graphics graphic = Graphics.FromImage(thumbnail);

            HttpContext.Current.Trace.Warn("Setting CompositingQuality of graphics");
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            HttpContext.Current.Trace.Warn("Setting SmoothingMode of graphics");
            graphic.SmoothingMode = SmoothingMode.HighQuality;

            HttpContext.Current.Trace.Warn("Setting SmoothingMode of graphics");
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

            HttpContext.Current.Trace.Warn("Setting InterpolationMode of graphics");
            Rectangle rectangle = new Rectangle(0, 0, width, height);

            HttpContext.Current.Trace.Warn("Drawing image");
            graphic.DrawImage(img, rectangle);

            // apply watermark
            // figure out a point where the watermark is to be placed.
            switch (imagePosition)
            {
                case 1:
                    Point point = new Point(width - 132, height - (height - 30));
                    Bitmap watermark = (Bitmap)Bitmap.FromFile(watermarkSource);
                    graphic.DrawImage(watermark, point);

                    thumbnail.Save(outputFile, ImageFormat.Jpeg);

                    thumbnail.Dispose();
                    graphic.Dispose();
                    watermark.Dispose();
                    img.Dispose();
                    break;
                case 2:
                    Point point1 = new Point(width - 132, height - (height - 30));
                    Bitmap watermark1 = (Bitmap)Bitmap.FromFile(watermarkSource);
                    graphic.DrawImage(watermark1, point1);

                    thumbnail.Save(outputFile, ImageFormat.Jpeg);

                    thumbnail.Dispose();
                    graphic.Dispose();
                    watermark1.Dispose();
                    img.Dispose();
                    break;

                case 4:
                    Point point4 = new Point(width - 118, 10);
                    Bitmap watermark4 = (Bitmap)Bitmap.FromFile(HttpContext.Current.Server.MapPath("/common/cw_watermark.png"));
                    graphic.DrawImage(watermark4, point4);

                    Point point5 = new Point(10, 10);
                    Bitmap watermark5 = (Bitmap)Bitmap.FromFile(HttpContext.Current.Server.MapPath("/common/autobild_watermark.png"));
                    graphic.DrawImage(watermark5, point5);

                    thumbnail.Save(outputFile, ImageFormat.Jpeg);

                    thumbnail.Dispose();
                    graphic.Dispose();
                    watermark4.Dispose();
                    watermark5.Dispose();
                    img.Dispose();
                    break;

                default:
                    Point point2 = new Point(width - 132, height - (height - 30));
                    Bitmap watermark2 = (Bitmap)Bitmap.FromFile(watermarkSource);
                    graphic.DrawImage(watermark2, point2);

                    thumbnail.Save(outputFile, ImageFormat.Jpeg);

                    thumbnail.Dispose();
                    graphic.Dispose();
                    watermark2.Dispose();
                    img.Dispose();
                    break;
            }
        }

        public static string GetImagePath(string relativePath)
        {
            string absolutePath = "";

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
                absolutePath = "http://img.carwale.com" + relativePath;
            else
                absolutePath = "http://webserver:8083" + relativePath;

            HttpContext.Current.Trace.Warn("absolutePath: " + absolutePath);

            return absolutePath;
        }

        public static string GetImagePath(string relativePath, string hostUrl)
        {
            string absolutePath = "";

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
                absolutePath = "http://" + hostUrl + relativePath;
            else
                absolutePath = "http://webserver:8083" + relativePath;

            HttpContext.Current.Trace.Warn("absolutePath: " + absolutePath);

            return absolutePath;
        }

        // added by vikas c. to fetch image from opr
        public static string GetImagePathOpr(string relativePath, string hostUrl)
        {
            string absolutePath = "";

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
                absolutePath = "http://" + hostUrl + relativePath;
            else
                absolutePath = "http://webserver:8082/images" + relativePath;

            HttpContext.Current.Trace.Warn("absolutePath: " + absolutePath);

            return absolutePath;
        }


        /* added on 30th mar'2011 by dipti(used in DCRM/UploadBasic*/

        public static void ResizeImage(string savedLocation, string targetLocation, int desiredWidth, int desiredHeight)
        {
            // Get dimentions of the uploaded image.
            Image imgOriginal = Image.FromFile(savedLocation);

            Size sz = GetDimensions(desiredWidth, desiredHeight, ref imgOriginal);

            System.Drawing.Image img = System.Drawing.Image.FromFile(savedLocation);
            System.Drawing.Image thumbnail = new Bitmap(sz.Width, sz.Height);
            Graphics graphic = Graphics.FromImage(thumbnail);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle rectangle = new Rectangle(0, 0, sz.Width, sz.Height);
            graphic.DrawImage(img, rectangle);
            thumbnail.Save(targetLocation, ImageFormat.Jpeg);
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

        /* added on 24th June'2011 by umesh(used in AutoBildIndia Profile pic*/

        public static void ResizePicture(String sourceImageFile, String TargetLocation, int width, int height)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(sourceImageFile);
            System.Drawing.Image oThumbNail = new Bitmap(img, width, height);
            Graphics oGraphic = Graphics.FromImage(oThumbNail);
            oGraphic.CompositingQuality = CompositingQuality.HighQuality;
            oGraphic.SmoothingMode = SmoothingMode.HighQuality;
            oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle oRectangle = new Rectangle(0, 0, width, height);
            oGraphic.DrawImage(img, oRectangle);
            oThumbNail.Save(TargetLocation);
            img.Dispose();
            oThumbNail.Dispose();
        }

        // Added By : Umesh Ojha on 26/07/2012
        // Function to return the image path for carwale. If website is live then img.carwale or path is webserver:8083
        public static string GetRootImagePath()
        {
            return "http://" + ConfigurationManager.AppSettings["imgHostURL"];
        }

        public static string GetImagePathCWImg(string relativePath, string hostUrl)
        {
            string absolutePath = "";

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("bikewale.com") >= 0)
                absolutePath = "http://" + hostUrl + relativePath;
            else
                absolutePath = "/images" + relativePath;

            return absolutePath;
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
                physicalPath = HttpContext.Current.Request["APPL_PHYSICAL_PATH"].ToLower().Replace("bikewale", "carwaleimg") + relativePath;
            }
            return physicalPath;
        }

        public static string GetPathToShowImages(string relativePath)
        {
            return "http://" + ConfigurationManager.AppSettings["imgHostURL"] + relativePath;
        }

        public static string GetPathToShowImages(string relativePath, string hostUrl)
        {
            return hostUrl + relativePath;
        }

        public static string GetPathToShowImages(string originalImagePath, string hostUrl, string size)
        {
            return String.Format("{0}/{1}/{2}", hostUrl, size, originalImagePath);
        }

        public static void SaveImageContent(HtmlInputFile fil, string relativePath)
        {
            string imgPath = "";

            imgPath = GetPathToSaveImages(relativePath);

            HttpContext.Current.Trace.Warn("imgPath=" + imgPath);
            fil.PostedFile.SaveAs(imgPath);
        } // ResolvePath

        /// <summary>
        /// Written By : Ashish G. Kamble on 23/8/2012
        /// This function will return true is file name has one of follwing extension JPEG, GIF, PNG. Else will return false        
        /// </summary>
        /// <param name="imageFileName">name of the image file</param>
        /// <returns></returns>
        public static bool IsValidFileExtension(string imageFileName)
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
                    isImageFileName = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return isImageFileName;
        }

    }//class
}//namespace