using AutoMapper;
using Carwale.DTOs.CMS.UserReviews;
using Carwale.DTOs.CMS;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Carwale.Interfaces.CMS;
using Carwale.Entity;
using Carwale.Interfaces;
using Carwale.Entity.UserReviews;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;

namespace Carwale.BL.CMS.UserReviews
{
    public class UserReviews : IUserReviews
    {
		private readonly ICarVersionCacheRepository _carVersionCacheRepo;
		private readonly IUserReviewsRepository _userReviewRepository;
        private readonly IUserReviewsCache _userReviewCacheRepo;
        private readonly IVideosBL _videosBL;
        private readonly ICustomerRepository<Customer, CustomerOnRegister> _customerRepo;

        public UserReviews(IUserReviewsRepository userReviewRepository, IUserReviewsCache userReviewCacheRepo,
            IVideosBL videosBL, ICustomerRepository<Customer, CustomerOnRegister> customerRepo,
			ICarVersionCacheRepository carVersionCacheRepo)
        {
            _userReviewRepository = userReviewRepository;
            _userReviewCacheRepo = userReviewCacheRepo;
            _videosBL = videosBL;
            _customerRepo = customerRepo;
			_carVersionCacheRepo = carVersionCacheRepo;
		}

        public bool CheckVersionReview(string versionId, string email, string customerId, string modelId)
        {
            return _userReviewRepository.CheckVersionReview(versionId, email, customerId, modelId);
        }

        public string SaveDetails(UserReviewDetail userReviewDetail, string userRating)
        {
            try
            {
                var ratingsCookie = JsonConvert.DeserializeObject<UserRatingEntity>(userRating);
                userReviewDetail.OverallR = ratingsCookie.OverallRatings;
                short purchasedAs = GetValueFromDictionary(ratingsCookie.Questions, string.Format("q-{0}", (short)EditorialEnum.RatingQuestion.PurchasedAs));
                userReviewDetail.IsNewlyPurchased = purchasedAs == (short)EditorialEnum.PurchasedAs.New ? "1" : "0";
                userReviewDetail.IsOwned = purchasedAs != (short)EditorialEnum.PurchasedAs.NotPurchased ? "1" : "0";
                userReviewDetail.Familiarity = GetValueFromDictionary(ratingsCookie.Questions, string.Format("q-{0}", (short)EditorialEnum.RatingQuestion.Familiarity)).ToString();
                var reviewQuestions = JsonConvert.DeserializeObject<Dictionary<string, short>>(userReviewDetail.ReviewQuestions);
                userReviewDetail.StyleR = GetValueFromDictionary(reviewQuestions, string.Format("q-{0}", (short)EditorialEnum.ReviewQuestions.ExteriorAndStyle));
                userReviewDetail.ComfortR = GetValueFromDictionary(reviewQuestions, string.Format("q-{0}", (short)EditorialEnum.ReviewQuestions.ComfortAndSpace));
                userReviewDetail.FuelEconomyR = GetValueFromDictionary(reviewQuestions, string.Format("q-{0}", (short)EditorialEnum.ReviewQuestions.FuelEconomy));
                userReviewDetail.PerformanceR = GetValueFromDictionary(reviewQuestions, string.Format("q-{0}", (short)EditorialEnum.ReviewQuestions.Performance));
                userReviewDetail.ValueR = GetValueFromDictionary(reviewQuestions, string.Format("q-{0}", (short)EditorialEnum.ReviewQuestions.Value));
                return _userReviewRepository.SaveDetails(userReviewDetail);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BL/UserReviewRepository/SaveDetails | error - {0}", ex.ToString()));
                objErr.SendMail();
            }
            return string.Empty;
        }

        public UserReviewDetailDesktopDto GetUserReviewsDetails(int id)
        {
            UserReviewDetailDesktopDto userReviewDetailsDTO = null;
            try
            {
                UserReviewDetail userReviewDetails = _userReviewCacheRepo.GetUserReviewDetailById(id, CMSAppId.Carwale);
                userReviewDetailsDTO = Mapper.Map<UserReviewDetail, UserReviewDetailDesktopDto>(userReviewDetails);
                if (userReviewDetails != null)
                {
                    userReviewDetailsDTO.SmallPicUrl = CWConfiguration._imgHostUrl + ImageSizes._310X174 + (string.IsNullOrWhiteSpace(userReviewDetails.OriginalImgPath) ? "/cw/cars/no-cars.jpg" : userReviewDetails.OriginalImgPath);
                    userReviewDetailsDTO.Comments = CMSCommon.RemoveAnchorTag(userReviewDetails.Comments);
                    userReviewDetailsDTO.ImageGalleryUrl = (userReviewDetails.ImageCount > 0 && userReviewDetails.OriginalImgPath != null) ? CMSCommon.GetImageUrl(userReviewDetails.Make, userReviewDetails.MaskingName, isMsite: true) : "javascript:void(0)";
                    Tuple<string, string> prevNextIds = GetNextPrevIds(userReviewDetails.ModelId, id);
                    userReviewDetailsDTO.PrevId = prevNextIds != null ? prevNextIds.Item1 : "";
                    userReviewDetailsDTO.NextId = prevNextIds != null ? prevNextIds.Item2 : "";
                    userReviewDetailsDTO.VideoCount = _videosBL.GetVideosByModelId(Convert.ToInt32(userReviewDetails.ModelId), CMSAppId.Carwale, 1, -1).Count;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return userReviewDetailsDTO;
        }

        private short GetValueFromDictionary(IDictionary<string, short> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            else
                return 0;
        }

        public List<UserReviewDTO> GetUserReviewsByType(UserReviewsSorting type)
        {
            return Mapper.Map<List<UserReviewEntity>, List<UserReviewDTO>>(_userReviewCacheRepo.GetUserReviewsByType(type));
        }

        public List<CarReviewBaseEntity> GetMostReviewedCars()
        {
            return _userReviewCacheRepo.GetMostReviewedCars();
        }

        private Tuple<string, string> GetNextPrevIds(string modelId, int id)
        {
            string prevId = string.Empty;
            string nextId = string.Empty;
            Tuple<string, string> prevNextIds = null;
            try
            {
                var modelsreviewIds = _userReviewCacheRepo.GetUserReviewedIdsByModel(CustomParser.parseIntObject(modelId));
                if (!string.IsNullOrEmpty(modelsreviewIds))
                {
                    string _commaIds = modelsreviewIds + ",";
                    string[] stringSeparators = new string[] { "," + id + "," };
                    string[] _splittedIds = _commaIds.Split(stringSeparators, StringSplitOptions.None);
                    if (_splittedIds.Length == 2)
                    {
                        string firstPart = _splittedIds[0];
                        string secondPart = _splittedIds[1];
                        if (firstPart != "")
                        {
                            string[] firstPartSplitted = firstPart.Split(',');
                            prevId = firstPartSplitted[firstPartSplitted.Length - 1];
                        }
                        if (secondPart != "")
                        {
                            string[] secondPartSplitted = secondPart.Split(',');
                            nextId = secondPartSplitted[0];
                        }
                        prevNextIds = new Tuple<string, string>(prevId, nextId);
                    }
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return prevNextIds;
        }

        public Tuple<int, bool> SubmitRating(RatingDetails ratingDetails)
        {
            try
            {
				var customerResponse = _customerRepo.Create(new Customer
				{
					Name = ratingDetails.UserDetails.UserName,
					Email = ratingDetails.UserDetails.UserEmail
                });
				CarVersionDetails versionDetails = _carVersionCacheRepo.GetVersionDetailsById(ratingDetails.CarDetails.VersionId);
				ratingDetails.CarDetails.ModelId = versionDetails.ModelId;
				ratingDetails.CarDetails.MakeId = versionDetails.MakeId;
				ratingDetails.CustomerId = !string.IsNullOrEmpty(customerResponse.CustomerId) ? Convert.ToInt32(customerResponse.CustomerId) : 0;
                if (ratingDetails.CustomerId > 0)
                {
                    _customerRepo.UpdateSourceId(EnumTableType.CustomerReviews, ratingDetails.CustomerId.ToString());
                    foreach (var question in ratingDetails.Rating.RatingQuestions)
                    {
                        if (question.QuestionId == ((int)EditorialEnum.RatingQuestion.Familiarity))
                        {
                            ratingDetails.Rating.Familiarity = question.Answer;
                        }
                        else if (question.QuestionId == ((int)EditorialEnum.RatingQuestion.PurchasedAs))
                        {
                            ratingDetails.Rating.IsNewlyPurchased = question.Answer == (short)EditorialEnum.PurchasedAs.New;
                            ratingDetails.Rating.IsOwned = question.Answer != (short)EditorialEnum.PurchasedAs.NotPurchased;
                        }
                    }
                    int reviewId = _userReviewRepository.GetReviewId(ratingDetails.CustomerId, ratingDetails.CarDetails.VersionId);
                    return reviewId > 0 ? Tuple.Create<int, bool>(reviewId, true) : Tuple.Create<int, bool>(_userReviewRepository.SaveRating(ratingDetails), false);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "SubmitRating");
            }
            return Tuple.Create<int, bool>(0, false);
        }
    }
}
