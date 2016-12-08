﻿using BikeWaleOpr.RabbitMQ;
using System;
using System.Data;

namespace BikeWaleOpr.Common.Ajax
{
    public class ImageReplication
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 27th Jan 2014
        /// Summary : to get pending image list 
        /// </summary>
        /// <param name="imageList">image id list</param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string FetchProcessedImagesList(string imageList)
        {
            string json = string.Empty;
            if (imageList.Length > 0)
                imageList = imageList.Substring(0, imageList.Length - 1);

            BikeCommonRQ bikeRQ = new BikeCommonRQ();
            DataSet ds = bikeRQ.FetchProcessedImagesList(imageList, ImageCategories.EDITCMS);

            try
            {
                if (ds.Tables.Count > 0)
                    json = JSON.GetJSONString(ds.Tables[0]);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ImageReplication.FetchProcessedImagesList");
                objErr.SendMail();
            }
            return json;
        }   //End of FetchProcessedImagesList

        /// <summary>
        /// Created By : Sadhana Upadhyay on 28th Jan 2014
        /// Summary : to Check Image Status
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string CheckImageStatus(string imageId)
        {
            string json = string.Empty;
            BikeCommonRQ bikeRQ = new BikeCommonRQ();
            try
            {
                if (!String.IsNullOrEmpty(imageId))
                {
                    DataSet ds = bikeRQ.CheckImageStatus(imageId, ImageCategories.EDITCMS);

                    if (ds.Tables.Count > 0)
                        json = JSON.GetJSONString(ds.Tables[0]);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ImageReplication.CheckImageStatus");
                objErr.SendMail();
            }
            return json;
        }   //End of CheckImageStatus

        /// <summary>
        /// Created By : Sadhana Upadhyay on 28th Jan 2014
        /// Summary : to Check Image Status of Expected Bike Launches
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string CheckImageStatus_ExpLaunch(string imageId)
        {
            string json = string.Empty;
            BikeCommonRQ bikeRQ = new BikeCommonRQ();

            try
            {
                if (!String.IsNullOrEmpty(imageId))
                {
                    DataSet ds = bikeRQ.CheckImageStatus(imageId, ImageCategories.EXPECTEDLAUNCH);

                    if (ds.Tables.Count > 0)
                        json = JSON.GetJSONString(ds.Tables[0]);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ImageReplication.CheckImageStatus");
                objErr.SendMail();
            }
            return json;
        }   //End of CheckImageStatus_ExpLaunch

        /// <summary>
        /// Created By : Sadhana Upadhyay on 28th Jan 2014
        /// Summary : to Check Image Status of BikeVersion
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string checkImageStatus_BikeVersion(string imageId)
        {
            string json = string.Empty;
            BikeCommonRQ bikeRQ = new BikeCommonRQ();

            try
            {
                if (!String.IsNullOrEmpty(imageId))
                {
                    DataSet ds = bikeRQ.CheckImageStatus(imageId, ImageCategories.BIKEVERSION);

                    if (ds.Tables.Count > 0)
                        json = JSON.GetJSONString(ds.Tables[0]);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ImageReplication.checkImageStatus_BikeVersion");
                objErr.SendMail();
            }
            return json;
        }   // End of checkImageStatus_BikeVersion

        /// <summary>
        /// Created By : Sadhana Upadhyay on 28th Jan 2014
        /// Summary : to Check Image Status of BikeVersion
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string checkImageStatus_SeriesPhotos(string imageId)
        {
            string json = string.Empty;
            BikeCommonRQ bikeRQ = new BikeCommonRQ();

            try
            {
                DataSet ds = bikeRQ.CheckImageStatus(imageId, ImageCategories.BIKESERIES);

                if (ds.Tables.Count > 0)
                    json = JSON.GetJSONString(ds.Tables[0]);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ImageReplication.checkImageStatus_BikeVersion");
                objErr.SendMail();
            }
            return json;
        }   // End of checkImageStatus_SeriesPhotos

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Aug 2015
        /// Summary : To check Status of image Where replicated or not
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string CheckImageStatusByCategory(string imageId, string Category)
        {
            string json = string.Empty;
            BikeCommonRQ bikeRQ = new BikeCommonRQ();

            try
            {
                if (!String.IsNullOrEmpty(imageId))
                {
                    ImageCategories imageType = (ImageCategories)Enum.Parse(typeof(ImageCategories), Category, true);

                    DataSet ds = bikeRQ.CheckImageStatus(imageId, imageType);

                    if (ds != null && ds.Tables.Count > 0)
                        json = JSON.GetJSONString(ds.Tables[0]);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ImageReplication.checkImageStatus_BikeVersion");
                objErr.SendMail();
            }
            return json;
        }
    }   //End of class
}   //End of namespace