using System;
using System.Web;
using AjaxPro;
using Bikewale.Common;
using Bikewale.Used;
using System.Data;
using System.Data.SqlClient;
using Bikewale.RabbitMQ;

namespace Bikewale.Ajax
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 21/8/2012
    ///     Class for the ajax related functions for sell bikes
    /// </summary>
    public class AjaxSellBike
    {
        /// <summary>
        ///     // Function to remove the bike photo
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool RemoveBikePhotos(string inquiryId, string photoId)
        {
            bool isRemoved = false;

            if (CommonOpn.IsNumeric(inquiryId) && CommonOpn.IsNumeric(photoId))
            {
                SellBikeCommon sellObj = new SellBikeCommon();
                isRemoved = sellObj.RemoveBikePhotos(inquiryId, photoId);
            }

            return isRemoved;
        }

        /// <summary>
        ///     Function to make image as main image which will be display on search pages
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool MakeMainImage(string inquiryId, string photoId)
        {
            bool isDone = false;

            if (CommonOpn.IsNumeric(inquiryId) && CommonOpn.IsNumeric(photoId))
            {
                bool _IsDealer = CurrentUser.Role == "DEALER" ? true : false;

                SellBikeCommon sellObj = new SellBikeCommon();
                isDone = sellObj.MakeMainImage(inquiryId, photoId, _IsDealer);
            }

            return isDone;
        }

        /// <summary>
        ///     Function to add photo description of the bike
        /// </summary>
        /// <param name="photoId"></param>
        /// <param name="imgDesc"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public bool AddImageDescription(string photoId, string imgDesc)
        {
            bool isAdded = false;

            if (CommonOpn.IsNumeric(photoId))
            {
                SellBikeCommon sellObj = new SellBikeCommon();
                isAdded = sellObj.AddImageDescription(photoId, imgDesc);
            }

            return isAdded;
        }

        ///// <summary>
        ///// Created By : Sadhana Upadhyay on 24th Dec 2013
        ///// Summary : To get Id, HostUrl, DirectoryPth, ImageUrlThumbSmall of image
        ///// </summary>
        ///// <param name="inquiryId"></param>
        ///// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string FetchProcessedImagesList(string imageList)
        {
            string json = string.Empty;
            if (imageList.Length > 0)
                imageList = imageList.Substring(0, imageList.Length - 1);

            BikeCommonRQ bikeRQ = new BikeCommonRQ();
            DataSet ds = bikeRQ.FetchProcessedImagesList(imageList, ImageCategories.BIKEWALESELLER);

            try
            {
                if (ds.Tables.Count > 0)
                    json = JSON.GetJSONString(ds.Tables[0]);
                else
                    json = "";
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxSellBike.FetchProcessedImagesList");
                objErr.SendMail();
            }
            return json;
        }
    }   // End of class
}   // end of namespace