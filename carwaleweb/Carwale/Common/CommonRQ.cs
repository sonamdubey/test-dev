using Carwale.Notifications;
using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web;
using Carwale.DAL.Images;
using Carwale.Entity.Enum;

/// <summary>
/// Summary description for CommonRQ
/// </summary>
namespace Carwale.UI.Common
{ 
    public class CommonRQ
    {
        #region enum decs
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        #endregion

        public static string UploadImageToCommonDatabase(string photoId, string imageName, ImageCategories imgC, string directoryPath)
        {
            string url = string.Empty;
            try
            {
                Images imgDal = new Images();
                url = imgDal.UploadImageToCommonDatabase(photoId,imageName,imgC,directoryPath);
            } 
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UploadImageProcessStart" + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                url = "exception" + ex.Message;
            } 
            return url;
        }
       public DataSet FetchProcessedImagesList(string imageList, string imgC)
        {
           var imgDal = new Images();    
           return imgDal.FetchProcessedImagesList(imageList, imgC);              
        }
    }
}