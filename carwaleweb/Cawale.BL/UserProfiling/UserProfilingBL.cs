using AutoMapper;
using AEPLCore.Cache;
using Bhrigu;
using Carwale.Cache.UserProfiling;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.Bhrigu;
using Carwale.DAL.UserProfiling;
using Carwale.DTOs.Autocomplete;
using Carwale.DTOs.CarData;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.UserProfiling;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.UserProfiling
{
    public class UserProfilingBL : IUserProfilingBL
    {
        private readonly ICarModelRepository _carModelRepos;
        private readonly ICarVersionRepository _carVersionRepo;
        private readonly static UserProfilingCache _userProfilingCache = new UserProfilingCache(new CacheManager(), new UserProfilingRepo());

        public UserProfilingBL(ICarModelRepository carModelRepos, ICarVersionRepository carVersionRepo)
        {
            _carVersionRepo = carVersionRepo;
            _carModelRepos = carModelRepos;
        }

        public List<ModelSpecificationsDTO> GetVersionDetails()
        {
            List<ModelDetails> modelListData = _carModelRepos.GetModelSpecs(true);
            List<VersionSubSegment> versionListData = _carVersionRepo.GetVersionSpecs(true);

            try
            {
                if (modelListData != null && versionListData != null)
                {
                    List<ModelSpecificationsEntity> modelVersionDetails = Mapper.Map<List<ModelDetails>, List<ModelSpecificationsEntity>>(modelListData);

                    foreach (var model in modelVersionDetails)
                    {
                        model.Versions = (from version in versionListData
                                          where model.ModelId == version.ModelId
                                          select version).ToList();

                        if (model.Versions.Count > 0)
                        {
                            model.AvgPrice = !model.IsUpcoming ? versionListData.Where(v => v.ModelId == model.ModelId && v.AvgPrice > 0)
                                .Select(v => v.AvgPrice).DefaultIfEmpty().Min() : model.AvgPrice;
                            model.SubSegment = model.Versions.OrderBy(y => y.SubSegmentId).FirstOrDefault().SubSegment;
                        }
                        if (model.BodyStyle == null)
                            model.BodyStyle = "NA";

                        if (model.SubSegment == null)
                            model.SubSegment = "NA";
                    }
                    List<ModelSpecificationsDTO> modelVersionDto = Mapper.Map<List<ModelSpecificationsEntity>, List<ModelSpecificationsDTO>>(modelVersionDetails);
                    return modelVersionDto;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "UserProfilingBL.GetVersionDetails()");
            }
            return null;
        }

        public static List<LabelValueDTO> GetGrpcUserProfile(string cwcCookie, Platform platform)
        {
            bool isGrpcCallNeeded = _userProfilingCache.GetUserProfilingStatus((int)platform);
            if (isGrpcCallNeeded)
            {
                return GetUserProfile(cwcCookie);
            }
            return null;
        }

        private static List<LabelValueDTO> GetUserProfile(string cwcCookie)
        {
            var userProfileData = new List<LabelValueDTO>();
            var userProfileResponse = new UserProfileResponse();
            var scoreResponse = new ScoreResponse();
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            apiGatewayCaller.GetUserProfile(Bhrigu.Application.Carwale, cwcCookie);
            apiGatewayCaller.GetModelScore(cwcCookie);
            apiGatewayCaller.Call();
            //first call
            try
            {
                userProfileResponse = apiGatewayCaller.GetResponse<UserProfileResponse>(0);
            }
            catch (Exception ex)
            {
                userProfileResponse = null;
                Logger.LogException(ex);
            }
            //second call
            try
            {
                scoreResponse = apiGatewayCaller.GetResponse<ScoreResponse>(1);
            }
            catch (Exception ex)
            {
                scoreResponse = null;
                Logger.LogException(ex);
            }
            //add userprofile data
            if (userProfileResponse != null && userProfileResponse.Response != null && userProfileResponse.Response.Count > 0)
            {
                userProfileData = SetUserProfile(userProfileResponse);
            }
            //else set default value
            else
            {
                userProfileData = SetDefaultUserProfile();
            }
            //add LeadPrediction Data 
            if (scoreResponse != null && !string.IsNullOrWhiteSpace(scoreResponse.Response))
            {
                LabelValueDTO lableValue = new LabelValueDTO
                {
                    Value = scoreResponse.Response,
                    Label = (UserProfileResponseEnum.LeadPrediction).ToNullSafeString()
                };
                userProfileData.Add(lableValue);
            }
            //else set default data
            else
            {
                userProfileData.Add(new LabelValueDTO() { Label = "LeadPrediction", Value = "NA" });
            }
            return userProfileData;
        }

        /// Written by Abhsihek Lovanshi on 19 July, 2018.
        /// <summary>
        /// This method set default response in case of null/empty response.
        /// It also sets default values if one of value in responses returns null/whitespaces/empty.
        /// It sets "0" in case of int else sets "NA".
        /// To set any default for perticuler value one must know type of response.(for type of responses check UserProfileResponseEnum)
        /// For numeric type response one must add condition in 'if' to set it default as "0"(in case of no response)
        /// All other responses set as "NA" in case of null/whitespaces/empty response.
        /// </summary>
        /// <param name="userProfileResponse"> UserProfileResponse,this parameter contains response that need to be set in list of LabelValue entity</param>
        /// <returns>List of LabelValue with UserProfileResponseEnum as 'Label' and corresponding response as 'Value'</returns>
        private static List<LabelValueDTO> SetUserProfile(UserProfileResponse userProfileResponse)
        {
            var userProfileData = new List<LabelValueDTO>();
            try
            {
                var userProfileValue = String.Empty;
                foreach (var item in userProfileResponse.Response)
                {
                    //checking type of response to set default values of response in case of null/empty response
                    if (item.Sequence == (int)UserProfileResponseEnum.CarCount || item.Sequence == (int)UserProfileResponseEnum.LeadCount)
                    {
                        userProfileValue = string.IsNullOrWhiteSpace(item.Result_) ? "0" : item.Result_; //Default value for int response
                    }
                    else
                    {
                        userProfileValue = string.IsNullOrWhiteSpace(item.Result_) ? "NA" : item.Result_; //Default value for string response
                    }
                    LabelValueDTO lableValue = new LabelValueDTO
                    {
                        Value = userProfileValue,
                        Label = ((UserProfileResponseEnum)item.Sequence).ToNullSafeString()
                    };
                    userProfileData.Add(lableValue);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return userProfileData;
        }

        private static List<LabelValueDTO> SetDefaultUserProfile()
        {
            var userProfileData = new List<LabelValueDTO>();
            userProfileData.Add(new LabelValueDTO() { Label = "CarType", Value = "NA" });
            userProfileData.Add(new LabelValueDTO() { Label = "LeadCount", Value = "0" });
            userProfileData.Add(new LabelValueDTO() { Label = "ModelsList", Value = "NA" });
            userProfileData.Add(new LabelValueDTO() { Label = "BodyStyleList", Value = "NA" });
            userProfileData.Add(new LabelValueDTO() { Label = "BudgetSegmentList", Value = "NA" });
            userProfileData.Add(new LabelValueDTO() { Label = "CarCount", Value = "0" });
            return userProfileData;
        }
    }
}
