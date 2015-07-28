﻿using System;
using System.Web;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

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
                physicalPath = HttpContext.Current.Request["APPL_PHYSICAL_PATH"].ToLower().Replace("bikewaleopr", "carwaleimg") + relativePath;    
            }
            return physicalPath;
        }

        public static string GetPathToShowImages(string relativePath)
        {
                       
            return "http://" + ConfigurationManager.AppSettings["imgHostURL"] + relativePath;
        }

        public static string GetPathToShowImages(string relativePath, string hostUrl)
        {

            return "http://" + hostUrl + relativePath;
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