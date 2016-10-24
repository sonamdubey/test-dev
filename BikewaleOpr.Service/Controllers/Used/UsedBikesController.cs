﻿using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Bikewale.Entities.Used;
using BikewaleOpr.Interface.Used;

namespace BikewaleOpr.Service.Controllers.Used
{
    /// <summary>
    /// Created by : Aditi Srivastava on 24 Oct 2016
    /// Summary    : Controller for used bikes
    /// </summary>
    public class UsedBikesController : ApiController
    {
        private readonly ISellBikes _objSellBikes = null;

        public UsedBikesController(ISellBikes objSellBikes)
        {
            _objSellBikes = objSellBikes;
        }
        /// <summary>
        ///  Created by : Aditi Srivastava on 24 Oct 2016
        ///  Summary    : To get all edited used sell inquiries pending approval
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/used/sell/pendinginquiries/")]
        public IHttpActionResult GetClassifiedPendingInquiries()
        {
            IEnumerable<SellBikeAd> objPending = null;
            try
            {

                objPending = _objSellBikes.GetClassifiedPendingInquiries();

                return Ok(objPending);
                
                
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UsedBikesController.GetClassifiedPendingInquiries");
                objErr.SendMail();
                return InternalServerError();
            }
        }
        /// <summary>
        ///  Created by : Aditi Srivastava on 24 Oct 2016
        ///  Summary    : To update edited entries
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isApproved"></param>
        /// <param name="approvedBy"></param>
        /// <returns></returns>
        [HttpPost, Route("api/used/sell/pendinginquiries/{inquiryId}/{isApproved}/{approvedBy}/{profileId}/{bikeName}")]
        public IHttpActionResult SaveEditedInquiry(uint inquiryId, short isApproved, int approvedBy,string profileId,string bikeName)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objSellBikes.SaveEditedInquiry(inquiryId, isApproved, approvedBy, profileId, bikeName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex,  String.Format("UsedBikesController.SaveEditedInquiry: InquiryId:{0}, IsApproved:{1}, Approvedby{2}",inquiryId,isApproved,approvedBy));
                objErr.SendMail();
                return InternalServerError();
            }
            return Ok(isSuccess);
        }
    }
}