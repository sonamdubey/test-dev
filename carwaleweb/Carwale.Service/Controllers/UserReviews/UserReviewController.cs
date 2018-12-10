using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Service.Filters;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Entity.CMS;
using AutoMapper;
using Carwale.Interfaces.UesrReview;
using Carwale.Utility;
using Carwale.Entity.Enum;
using System.Configuration;
using Carwale.DTOs.UserReview;
using Carwale.BL.UserReview;
using Carwale.Entity.UserReview;
using Carwale.DTOs.UserReviews;
using Carwale.Entity.UserReviews;

namespace Carwale.Service.Controllers.UserReviews
{
    public class UserReviewController : ApiController
    {
        private readonly ICarVersionCacheRepository _carVersionCacheRepo;
        private readonly IUserReviewsCache _userReviewCacheRepo;
        private readonly IUserReviews _userReviews;
        private readonly IUserReviewLogic _userReviewLogic;
        public UserReviewController(ICarVersionCacheRepository carVersionCacheRepo, IUserReviewsCache userReviewCacheRepo,
            IUserReviews userReviews, IUserReviewLogic userReviewLogic)
        {
            _carVersionCacheRepo = carVersionCacheRepo;
            _userReviewCacheRepo = userReviewCacheRepo;
            _userReviews = userReviews;
            _userReviewLogic = userReviewLogic;
        }

        [HttpGet, AuthenticateBasicAttribute, Route("api/userreviews/ratecar"), HandleException]
        public IHttpActionResult GetRatingPageDetails()
        {
            int versionId;
            CarVersionDetails versionDetails = null;
            NameValueCollection nvc = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            int.TryParse(nvc["versionid"], out versionId);
            if (versionId <= 0)
            {
                return BadRequest("Version Id is invalid.");
            }
            versionDetails = _carVersionCacheRepo.GetVersionDetailsById(versionId);
            if (versionDetails != null)
            {
                var ratingObject = new RateCarDto
                {
                    CarDetails = new UserReviewCarDetails
                    {
                        Make = new NameIdBase
                        {
                            Name = versionDetails.MakeName ?? string.Empty,
                            Id = versionDetails.MakeId
                        },
                        Model = new NameIdBase
                        {
                            Name = versionDetails.ModelName ?? string.Empty,
                            Id = versionDetails.ModelId
                        },
                        Version = new NameIdBase
                        {
                            Name = versionDetails.VersionName ?? string.Empty,
                            Id = versionDetails.VersionId
                        },
                        HostUrl = versionDetails.HostURL ?? string.Empty,
                        OriginalImgPath = versionDetails.OriginalImgPath ?? string.Empty
                    },
                    RatingDetails = RateCarObject.GetRateCarDetails()
                };

                return Ok(ratingObject);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet, AuthenticateBasicAttribute, Route("api/userreviews/reviewcar"), HandleException]
        public IHttpActionResult GetReviewPageDetails()
        {
            int reviewId;
            NameValueCollection nvc = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            int.TryParse(nvc["reviewid"], out reviewId);
            if (reviewId <= 0)
            {
                return BadRequest("review Id is invalid.");
            }
            UserReviewDetail userReviewDetails = _userReviewCacheRepo.GetUserReviewDetailById(reviewId, CMSAppId.Carwale);
            if (userReviewDetails != null)
            {
                return Ok(WriteReviewObject.GetWriteReviewPageDetails(userReviewDetails));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost, AuthenticateBasicAttribute, Route("api/userreviews/rating/"), HandleException]
        public IHttpActionResult SubmitRating([FromBody]RatingDetailsDTO userResponse)
        {
            if (!ValidateRatingRequest(userResponse))
            {
                return BadRequest("Please check the details you have entered");
            }
			int plateFormId = Request.Headers.GetValueFromHttpHeader<int>("sourceid");
			if (plateFormId > 0)
			{
				userResponse.PlatformId = plateFormId;
			}

			Tuple<int, bool> review = _userReviews.SubmitRating(Mapper.Map<RatingDetailsDTO, RatingDetails>(userResponse));
            string message = string.Empty;
            if (review.Item1 <= 0)
            {
                return BadRequest("Please check the details you have entered");
            }
            if (!review.Item2)
            {
                message = "Thank you";
            }
            else
            {
                var versionDetails = _carVersionCacheRepo.GetVersionDetailsById(userResponse.CarDetails.VersionId);
                message = string.Format(RateCarObject.ResponseMessage,
                    versionDetails?.MakeName ?? string.Empty, versionDetails?.ModelName ?? string.Empty,
                    versionDetails?.VersionName ?? string.Empty);
            }
            var result = new
            {
                message,
                rewiewId = review?.Item1,
                hash = Utils.Utils.EncryptTripleDES(string.Format("{0}|{1}", review?.Item1.ToString() ?? string.Empty, "CWURS")),
                isDuplicate = review?.Item2
            };
            return Ok(result);
        }


        [AuthenticateBasicAttribute, HttpPut, Route("api/v1/userreviews/"), HandleException]
        public IHttpActionResult Put([FromBody] UserReviewBody reviewBody)
        {
            if (reviewBody != null && reviewBody.Hash.IsNotNullOrEmpty())
            {
                string decryptedHash = Utils.Utils.DecryptTripleDES(reviewBody.Hash);
                int reviewId;
                int.TryParse(decryptedHash.Split('|')[0], out reviewId);
                if (reviewId > 0)
                {
                    reviewBody.Id = reviewId;
                    string customerName = _userReviewLogic.SaveCustomerReview(reviewBody);
                    if (customerName == null)
                    {
                        return BadRequest();
                    }
                    var result = new
                    {
                        heading = string.Format("Thank You {0}!", !string.IsNullOrEmpty(customerName) ? customerName : string.Empty),
                        message = string.Format(WriteReviewObject.ResponseMessage, ConfigurationManager.AppSettings["ReplyTo"])
                    };
                    return Ok(result);
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("api/userreviews/landingpage/"), HandleException]
        public IHttpActionResult GetReviewLangingPageDetails()
        {
            Platform platformId = HttpContextUtils.GetHeader<Platform>("platformid");
            return Ok(_userReviewLogic.GetLandingPageDetails(platformId));
        }

        private static bool ValidateRatingRequest(RatingDetailsDTO ratingDetails)
        {
            if (!ValidateUserData(ratingDetails.UserDetails))
            {
                return false;
            }
            else if (!ValidateCarData(ratingDetails.CarDetails))
            {
                return false;
            }
            else if (ratingDetails.Rating == null || ratingDetails.Rating.UserRating <= 0 || ratingDetails.Rating.UserRating > 5)
            {
                return false;
            }
            else if (!ratingDetails.Rating.RatingQuestions.IsNotNullOrEmpty() || ratingDetails.Rating.RatingQuestions.Count < 2)
            {
                return false;
            }
            else if (ratingDetails.Rating.RatingQuestions.Exists(x => x.QuestionId <= 0) ||
                ratingDetails.Rating.RatingQuestions.Exists(x => x.Answer <= 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool ValidateUserData(UserDetailsDto userData)
        {
            return !(userData == null || !RegExValidations.IsValidEmail(userData.UserEmail?.ToLower()) || String.IsNullOrEmpty(userData.UserName));
        }

        private static bool ValidateCarData(CarDetailsDto carDetails)
        {
            return !(carDetails == null || carDetails.VersionId <= 0);
        }

    }
}
