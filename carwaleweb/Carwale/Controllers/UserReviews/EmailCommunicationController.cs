using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Carwale.DTOs;
using Carwale.DTOs.CMS.UserReviews;
using Carwale.Entity;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Interfaces;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Notifications.Logs;
using Carwale.Service;
using Carwale.UI.ClientBL;
using Carwale.Utility;

namespace Carwale.UI.Controllers.UserReviews
{
    public class EmailCommunicationController : Controller
    {
		private readonly IUserReviewsRepository _userReviewsRepo;
		public EmailCommunicationController(IUserReviewsRepository userReviewsRepo)
		{
			try
			{
				_userReviewsRepo = userReviewsRepo;
			}
            catch (Exception ex)
            {
                Logger.LogException(ex, "Dependency Injection Block at EmailCommunicationController");
            }
		}
		public ActionResult VerifyUserReviewCustomerEmail()
		{
			Response.AddHeader("Vary", "User-Agent");
			bool isMobile = DeviceDetectionManager.IsMobile(this.HttpContext);
			ViewData["IsMobile"] = isMobile;
			ViewData["Message"] = "It seems some error has occurred please try again after some time.";
			ViewData["Icon"] = "Error";
            ViewData["Action"] = "verification";
			try
			{
				string hash = Request.QueryString["id"] ?? string.Empty;
				int customerId;
				string userIdDecripted = Utils.Utils.DecryptTripleDES(hash);
				Int32.TryParse(userIdDecripted, out customerId);

				if (customerId > 0)
				{
					var customer = _userReviewsRepo.GetUserReviewCustomerById(customerId);
					if (customer != null && customer.Id == customerId)
					{
                        UserReviewCustomerDto customerDetails = null;
						customerDetails = Mapper.Map<UserReviewCustomerInfo, UserReviewCustomerDto>(customer);
						int res = _userReviewsRepo.ProcessEmailVerfication(true, customerId);
						ViewData["Message"] = res > 0 ? "Your Email Id has been verified successfully." : "You have already been verified, or the link has expired.";
						ViewData["Icon"] = res > 0 ?"Success": "Failure";
                        return View("~/Views/UserReview/VerifyEmail.cshtml", customerDetails);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogException(ex, "EmailCommunicationController.VerifyEmail()\n Exception : " + ex.Message);
			}
			return View("~/Views/UserReview/VerifyEmail.cshtml", null);
		}

		public ActionResult InvalidReview()
		{
			Response.AddHeader("Vary", "User-Agent");
			ViewData["IsMobile"] = DeviceDetectionManager.IsMobile(this.HttpContext);
			ViewData["Message"] = "It seems some error has occurred please try again after some time.";
			ViewData["Icon"] = "Error";
            ViewData["Action"] = "Not_me";
			try
			{
				string id = Request.QueryString["id"] ?? string.Empty;
				int replicaId;
                string replicaIdDecripted = Utils.Utils.DecryptTripleDES(id);
				Int32.TryParse(replicaIdDecripted, out replicaId);

				if (replicaId > 0)
				{
                    UserReviewCustomerDto customerDetails = null;
                    var result = _userReviewsRepo.InvalidateUserReviewReplica(replicaId);
                    var customer = _userReviewsRepo.GetUserReviewCustomerById(result.Item2);
                    if (customer != null && customer.Id == result.Item2)
                    {
                        customerDetails = Mapper.Map<UserReviewCustomerInfo, UserReviewCustomerDto>(customer);
                    }
					if (result.Item1 > 0)
					{
                        int rowCount = InvalidateUserReview(result.Item1,replicaId);
                        if(rowCount > 0)
                        {
                            ViewData["Message"] = "Your review has been removed successfully.";
                            ViewData["Icon"] = "Success";
                            //TODO: Send Success Mail to Customer 
                        }
                        else if (rowCount == 0)
                        {
                            ViewData["Message"] = "Review has been removed already, or the link has expired.";
                            ViewData["Icon"] = "Failure";
                        }
					}
                    return View("~/Views/UserReview/VerifyEmail.cshtml", customerDetails);
				}
			}
			catch (Exception ex)
			{
				Logger.LogException(ex, "VerifyEmailController.InvalidReview()\n Exception : " + ex.Message);
			}
            return View("~/Views/UserReview/VerifyEmail.cshtml", null);
		}
        private int InvalidateUserReview(int reviewId, int replicaId)
        {
            var replicaDetails = _userReviewsRepo.GetLatestUserReviewReplicaByReviewId(reviewId);
            if (replicaDetails != null && replicaDetails.Id > 0)
            {
                return _userReviewsRepo.IsVerifiedReviewReplica(replicaId) ?
                    _userReviewsRepo.UpdateReviewWithLatestReplica(replicaDetails) : 1;
            }
            else
            {
                return _userReviewsRepo.IsActiveReview(reviewId) ?
                    _userReviewsRepo.DeleteUserReview(reviewId) : 0;
            }
        }
	}
}