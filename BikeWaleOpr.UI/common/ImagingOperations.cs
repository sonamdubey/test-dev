﻿using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for ImagingFunctions
/// </summary>
/// 
namespace BikeWaleOpr
{
    public class ImagingOperations
    {
        public static string GetPathToSaveImages(string relativePath)
        {
            string physicalPath = string.Empty;

            if (HttpContext.Current.Request["HTTP_HOST"].IndexOf("localhost") >= 0)
            {
                physicalPath = HttpContext.Current.Request["APPL_PHYSICAL_PATH"].ToLower() + relativePath;
            }
            else
            {
                //physicalPath = HttpContext.Current.Request["APPL_PHYSICAL_PATH"].ToLower().Replace("bikewaleopr", "carwaleimg") + relativePath;   
                physicalPath = ConfigurationManager.AppSettings["imgPathFolder"] + relativePath;
            }
            return physicalPath;
        }

        public static string GetPathToShowImages(string relativePath)
        {

            return "https://" + ConfigurationManager.AppSettings["imgHostURL"] + relativePath;
        }

        public static string GetPathToShowImages(string relativePath, string hostUrl)
        {

            return "https://" + hostUrl + relativePath;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Aug 2015
        /// Summary : To get path to show image
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="imgSize"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetPathToShowImages(string hostUrl, string imgSize, string relativePath)
        {
            return Bikewale.Utility.Image.GetPathToShowImages(relativePath, hostUrl, imgSize);
        }

        public static void SaveImageContent(HtmlInputFile fil, string relativePath)
        {
            string imgPath = "";

            imgPath = GetPathToSaveImages(relativePath);

            HttpContext.Current.Trace.Warn("imgPath=" + imgPath);
            fil.PostedFile.SaveAs(imgPath);
        } // ResolvePath

        /// Function to delete the provided file
        void DeleteTempImgs(string imgPath)
        {
            FileInfo tempFile = new FileInfo(imgPath);
            tempFile.Delete();// delete the provided file
        }
    }
}