using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Models.QuestionAndAnswers;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.Models.QuestionsAnswers
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th August 2018
    /// Description: UI Model class for question details page
    /// </summary>
    public class QuestionDetailsModel
    {
        public StatusCodes Status { get; private set; }
        public string RedirectUrl { get; set; }
        public bool IsMobile { get; set; }
        private readonly IQuestions _bwQuestions = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly IBikeInfo _objGenericBike = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _models;
        private readonly IBikeSeries _bikeSeries = null;

        private readonly ushort _mobAnsTrimLen = 150, _deskAnsTrimLen = 360, _mobQuesBreadCrumbLen = 50;
        private uint _modelId = 0, _cityId = 0, _totalTabCount = 3;
        private string _makeMaskingName, _modelMaskingName, _questionIdHash, _questionId;
        private readonly string nullString = string.Empty;
        private BikeInfoTabType _pageId = BikeInfoTabType.Question;
        private GlobalCityAreaEntity currentCityArea = null;

        private string _adId_Mobile = "1533549941087";
        private string _adPath_Mobile = "/1017752/BikeWale_Mobile_QNA_Detail";
        private string _adId_Desktop = "1533549744617";
        private string _adPath_Desktop = "/1017752/BikeWale_QNA_Detail";



        public QuestionDetailsModel(
            string makeMaskingName,
            string modelMaskingName,
            string questionIdHash,
            IQuestions bwQuestions,
            IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache,
            IBikeModelsCacheRepository<int> modelCache,
            IBikeInfo objGenericBike,
            ICityCacheRepository objCityCache,
            IBikeModels<BikeModelEntity, int> models,
            IBikeSeries bikeSeries
           )
        {
            _makeMaskingName = makeMaskingName;
            _modelMaskingName = modelMaskingName;
            _questionIdHash = questionIdHash;
            _bwQuestions = bwQuestions;
            _modelMaskingCache = modelMaskingCache;
            _modelCache = modelCache;
            _objGenericBike = objGenericBike;
            _objCityCache = objCityCache;
            _models = models;
            _bikeSeries = bikeSeries;
            ProcessQueryString();
            ProcessCityArea();
        }

        /// <summary>
        /// Created by : Snehal Dange on 14th August
        /// Description : Method to get global city Id and Name from cookie.
        /// </summary>
        private void ProcessCityArea()
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                {
                    _cityId = currentCityArea.CityId;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.News.NewsIndexPage.ProcessCityArea");
            }
        }

        public QuestionDetailsVM GetData()
        {
            QuestionDetailsVM objQuestionDetailsVM = new QuestionDetailsVM();
            try
            {
                GetBikeData(objQuestionDetailsVM);
                GetQuestionData(objQuestionDetailsVM);
                BindAdSlots(objQuestionDetailsVM);
                BindPageMetas(objQuestionDetailsVM);
                SetBreadCrumbs(objQuestionDetailsVM);
                SetJSONLDSchema(objQuestionDetailsVM);
                BindAskQuestionPopup(objQuestionDetailsVM);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Models.QuestionsAnswers.QuestionDetails.GetData");
            }
            return objQuestionDetailsVM;
        }


        private void GetBikeData(QuestionDetailsVM objQuestionDetailsVM)
        {
            try
            {
                if (_modelId > 0)
                {
                    BindBikeInfoWidget(objQuestionDetailsVM);
                    objQuestionDetailsVM.BikeModelAnsweredQuestions = _bwQuestions.GetQuestionCountByModelId(_modelId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Models.QuestionsAnswers.QuestionDetails.GetBikeData");
            }
        }

        private void GetQuestionData(QuestionDetailsVM objQuestionDetailsVM)
        {
            try
            {
                if (!string.IsNullOrEmpty(_questionId))
                {
                    Question ques = _bwQuestions.GetQuestionDataByQuestionId(_questionId);
                    if (ques != null && ques.Answers != null && ques.Answers.Any())
                    {
                        FormatQuestionDetails(ques);
                        objQuestionDetailsVM.Question = ques;
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    GenericBikeInfo objBikeInfo = (objQuestionDetailsVM.BikeInfoWidget != null ? objQuestionDetailsVM.BikeInfoWidget.BikeInfo : null);
                    if (objBikeInfo != null)
                    {   
                        objQuestionDetailsVM.Tags = string.Format("{0},{1}", objBikeInfo.Make.MaskingName, objBikeInfo.Model.MaskingName);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Models.QuestionsAnswers.QuestionDetails.GetQuestionData");
            }

        }

        private void BindPageMetas(QuestionDetailsVM objQuestionDetailsVM)
        {
            try
            {
                int answerCount = 0;
                string quesText = nullString, userName = nullString, url = nullString, makeName = nullString, modelName = nullString;

                GenericBikeInfo objBikeInfo = (objQuestionDetailsVM.BikeInfoWidget != null ? objQuestionDetailsVM.BikeInfoWidget.BikeInfo : null);
                if (objBikeInfo != null && objBikeInfo.Make != null && objBikeInfo.Model != null)
                {
                    makeName = objBikeInfo.Make.MakeName;
                    modelName = objBikeInfo.Model.ModelName;
                }
                Question objQuestion = objQuestionDetailsVM.Question;
                if (objQuestion != null)
                {
                    quesText = objQuestion.Text;
                    userName = objQuestion.AskedBy != null ? objQuestion.AskedBy.CustomerName : nullString;
                    url = string.Format("/{0}-bikes/{1}/questions-and-answers/{2}-{3}/", _makeMaskingName, _modelMaskingName, objQuestion.BaseUrl, _questionIdHash);
                    answerCount = objQuestion.Answers != null ? objQuestion.Answers.Count() : 0;
                }

                objQuestionDetailsVM.Platform = IsMobile ? Platforms.Mobile : Platforms.Desktop;

                PageMetaTags objPageMetas = objQuestionDetailsVM.PageMetaTags;
                if (objPageMetas != null)
                {
                    objPageMetas.Title = string.Format("Question on {0} {1} - {2}", makeName, modelName, quesText);
                    objPageMetas.Description = string.Format("Read all answers for the question posted by {0} on {1} {2}. Question goes - {3}. Read answers by all {4} users to this question on BikeWale.", userName, makeName, modelName, quesText, answerCount);
                    objPageMetas.Keywords = string.Format("{0} {1} questions, {0} {1} q&a, {0} {1} questions and answers, {0} {1} answers, {0} {1} user answers", makeName, modelName);
                    objPageMetas.CanonicalUrl = string.Format("{0}{1}", BWConfiguration.Instance.BwHostUrl, url);
                    objPageMetas.AlternateUrl = string.Format("{0}/m{1}", BWConfiguration.Instance.BwHostUrl, url);

                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Models.QuestionsAnswers.QuestionDetails.BindPageMetas");
            }
        }

        /// Modified by : Snehal Dange on 23 Nov 2018
        /// Description : Set the prop `SchemaBreadcrumbList` which is used to bind breadcrumb in web page schema. Breadcrumbs in schema has all the previous links except the current page link.
        private void SetBreadCrumbs(QuestionDetailsVM objQuestionDetailsVM)
        {
            try
            {
                IList<Bikewale.Entities.Schema.BreadcrumbListItem> breadCrumbs = new List<Bikewale.Entities.Schema.BreadcrumbListItem>();
                ushort position = 1;
                string bwUrl = string.Format("{0}/{1}", Utility.BWConfiguration.Instance.BwHostUrl, IsMobile ? "m/" : nullString), makeName = nullString, modelName = nullString, makeUrl = nullString, modelUrl = nullString, quesText = nullString, currentPageUrl = nullString;

                GenericBikeInfo objBikeInfo = (objQuestionDetailsVM.BikeInfoWidget != null ? objQuestionDetailsVM.BikeInfoWidget.BikeInfo : null);
                if (objBikeInfo != null && objBikeInfo.Make != null && objBikeInfo.Model != null)
                {
                    makeName = objBikeInfo.Make.MakeName;
                    modelName = objBikeInfo.Model.ModelName;
                    makeUrl = string.Format("{0}{1}-bikes/", bwUrl, _makeMaskingName);
                    modelUrl = string.Format("{0}{1}/", makeUrl, _modelMaskingName);

                }
                BikeSeriesEntityBase seriesDetails = _modelCache.GetSeriesByModelId(_modelId);
                if (objQuestionDetailsVM.Question != null)
                {
                    quesText = objQuestionDetailsVM.Question.Text;
                    if (IsMobile && quesText.Length > _mobQuesBreadCrumbLen)
                    {
                        quesText = string.Format("{0}...", quesText.Substring(0, _mobQuesBreadCrumbLen));
                    }
                }

                breadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bwUrl, "Home"));
                breadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", bwUrl), "New Bikes"));
                breadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, makeUrl, string.Format("{0} Bikes", makeName)));
                if (objBikeInfo != null && objBikeInfo.Make != null && objBikeInfo.BodyStyleId.Equals(EnumBikeBodyStyles.Scooter) && !(objBikeInfo.Make.IsScooterOnly))
                {
                    breadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}{1}-scooters/", bwUrl, _makeMaskingName), string.Format("{0} Scooters", makeName)));
                }
                if (seriesDetails != null && seriesDetails.IsSeriesPageUrl)
                {
                    breadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}{1}/", makeUrl, seriesDetails.MaskingName), seriesDetails.SeriesName));
                }
                breadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, modelUrl, string.Format("{0} {1}", makeName, modelName)));
                breadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}questions-and-answers/", modelUrl), string.Format("{0} Questions and Answers", modelName)));

                if (!String.IsNullOrEmpty(objQuestionDetailsVM.Question.BaseUrl))
                {
                    currentPageUrl = string.Format("{0}questions-and-answers/{1}-{2}/", modelUrl, objQuestionDetailsVM.Question.BaseUrl, _questionIdHash);
                }

                breadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, currentPageUrl, quesText));

                objQuestionDetailsVM.BreadcrumbList.BreadcrumListItem = breadCrumbs;
                objQuestionDetailsVM.SchemaBreadcrumbList.BreadcrumListItem = breadCrumbs.Take(breadCrumbs.Count - 1);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Models.QuestionsAnswers.QuestionDetailsModel.SetBreadCrumbs");
            }
        }

        /// Modified by : Snehal Dange on 23 Nov 2018
        /// Desc :  Replaced prop `BreadcrumbList` with `SchemaBreadcrumbList` in GetWebpageSchema. This is done to remove the current page link from breadcrumb in webpage schema .
        private void SetJSONLDSchema(QuestionDetailsVM objQuestionDetailsVM)
        {
            try
            {
                Question objQuestion = objQuestionDetailsVM.Question;
                Bikewale.Entities.Schema.WebPage webpage = SchemaHelper.GetWebpageSchema(objQuestionDetailsVM.PageMetaTags, objQuestionDetailsVM.SchemaBreadcrumbList);
                if (webpage != null && objQuestion != null)
                {
                    IList<Entities.Schema.Answer> answerList = new List<Entities.Schema.Answer>();
                    ushort position = 1;

                    if (objQuestion.Answers != null && objQuestion.Answers.Any())
                    {
                        foreach (Answer ans in objQuestion.Answers)
                        {
                            Entities.Schema.Answer answer = new Entities.Schema.Answer()
                            {
                                Text = ans.Text,
                                CreatedOn = ans.AnsweredOn,
                                AuthorObj = new Entities.Schema.Author()
                                {
                                    Name = ans.AnsweredBy.CustomerName
                                },
                                Position = position++
                            };
                            answerList.Add(answer);
                        }
                    }

                    Entities.Schema.Question question = new Entities.Schema.Question()
                    {
                        Text = objQuestion.Text,
                        DateCreated = objQuestion.AskedOn,
                        AuthorObj = new Entities.Schema.Author()
                        {
                            Name = objQuestion.AskedBy != null ? objQuestion.AskedBy.CustomerName : nullString,
                        },
                        AnswersCount = (uint)objQuestion.Answers.Count(),
                        AcceptedAnswerObj = new Entities.Schema.AcceptedAnswer()
                        {
                            AnswersList = answerList
                        },
                        Position = 1
                    };

                    objQuestionDetailsVM.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
                    objQuestionDetailsVM.PageMetaTags.PageSchemaJSON = SchemaHelper.JsonSerialize(question);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Models.QuestionDetailsModel.SetJsonLDSchema");
            }
        }

        private void FormatQuestionDetails(Question ques)
        {
            try
            {
                int trimLength = IsMobile ? _mobAnsTrimLen : _deskAnsTrimLen;
                ques.QuestionAge = FormatDate.GetTimeSpan(ques.AskedOn);
                foreach (Answer ans in ques.Answers)
                {
                    ans.StrippedText = ans.Text.Length > trimLength ? ans.Text.Substring(0, trimLength) : string.Empty;
                    ans.AnswerAge = FormatDate.GetTimeSpan(ans.AnsweredOn);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.QuestionsAndAnswers.Questions.FormatQuestion(Question)");
            }
        }

        private void ProcessQueryString()
        {
            try
            {
                ProcessBikeMaskingName();
                // need model id to access question id from hashtable, if model id <= 0, redirect or not found status is already set
                if (_modelId > 0)
                {
                    ProcessQuestionIdHash();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Models.QuestionsAnswers.QuestionDetailsModel.ProcessQueryString");
            }
        }

        private void ProcessBikeMaskingName()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;
                if (!string.IsNullOrEmpty(_makeMaskingName) || !string.IsNullOrEmpty(_modelMaskingName))
                {
                    string bikeMaskingName = string.Format("{0}_{1}", _makeMaskingName, _modelMaskingName);
                    ModelMaskingResponse modelMaskingResponse = _modelMaskingCache.GetModelMaskingResponse(bikeMaskingName);
                    switch (modelMaskingResponse.StatusCode)
                    {
                        case 200:
                            _modelId = modelMaskingResponse.ModelId;
                            break;
                        case 301:
                            Status = StatusCodes.RedirectPermanent;
                            RedirectUrl = request.RawUrl.Replace(_modelMaskingName, modelMaskingResponse.MaskingName);
                            break;
                        default:
                            Status = StatusCodes.ContentNotFound;
                            break;
                    }
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Models.QuestionsAnswers.QuestionDetailsModel.ProcessBikeMaskingName");
            }

        }

        private void ProcessQuestionIdHash()
        {
            try
            {
                if (!string.IsNullOrEmpty(_questionIdHash))
                {
                    _questionId = _bwQuestions.GetQuestionIdHashMapping(_questionIdHash, _modelId, EnumQuestionIdHashMappingChoice.HashToQuestionId);
                    if (string.IsNullOrEmpty(_questionId))
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Models.QuestionsAnswers.QuestionDetailsModel.ProcessQuestionIdHash");
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 13th Aug 2018
        /// Desc :      Function to bind the ad slots
        /// </summary>
        /// <param name="objQuestionDetailsVM"></param>
        private void BindAdSlots(QuestionDetailsVM objVM)
        {
            try
            {
                IDictionary<string, AdSlotModel> ads = new Dictionary<string, AdSlotModel>();
                NameValueCollection adInfo = new NameValueCollection();
                AdTags adTagsObj = objVM.AdTags;
                if (IsMobile)
                {
                    adTagsObj.AdPath = _adPath_Mobile;
                    adTagsObj.AdId = _adId_Mobile;
                    adTagsObj.Ad_320x50 = true;

                    adInfo["adId"] = _adId_Mobile;
                    adInfo["adPath"] = _adPath_Mobile;

                    if (adTagsObj.Ad_320x50)
                    {
                        ads.Add(String.Format("{0}-0", _adId_Mobile), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._320x50], 0, 320, AdSlotSize._320x50, "Top", true));
                    }
                }
                else
                {
                    adTagsObj.AdPath = _adPath_Desktop;
                    adTagsObj.AdId = _adId_Desktop;
                    adTagsObj.Ad_970x90Top = true;

                    adInfo["adId"] = _adId_Desktop;
                    adInfo["adPath"] = _adPath_Desktop;

                    if (adTagsObj.Ad_970x90Top)
                    {
                        ads.Add(String.Format("{0}-19", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._970x90 + "_D"], 19, 970, AdSlotSize._970x90, "Top", true));
                    }
                }
                objVM.AdSlots = ads;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.QuestionsAnswers.QuestionDetailsModel.BindAdSlots");
            }
        }

        /// <summary>
        /// Created by : SnehaL Dange on 14th August 2018
        /// Desc : Bind the Bike info widget on desktop
        /// </summary>
        /// <param name="objVM"></param>
        private void BindBikeInfoWidget(QuestionDetailsVM objVM)
        {
            BikeInfoWidget genericInfoWidget = new BikeInfoWidget(_objGenericBike, _objCityCache, _modelId, _cityId, _totalTabCount, _pageId, _models, _bikeSeries);
            BikeInfoVM bikeInfo = genericInfoWidget.GetData();
            if(bikeInfo != null && bikeInfo.BikeInfo != null && bikeInfo.BikeInfo.Model != null)
            {
                bikeInfo.BikeInfo.Model.ModelId = (int)_modelId;
            }
            objVM.BikeInfoWidget = bikeInfo;

        }

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 17th Oct 2018
        /// Description: Populate AskQuestionVM
        /// </summary>
        /// <param name="_objVM"></param>
        private void BindAskQuestionPopup(QuestionDetailsVM _objVM)
        {
            try
            {
                GenericBikeInfo objMakeModel = null;
                if(_objVM != null && _objVM.BikeInfoWidget != null)
                {
                   objMakeModel = _objVM.BikeInfoWidget.BikeInfo;
                }
                if (objMakeModel != null && objMakeModel.Make != null && objMakeModel.Model != null)
                {
                    _objVM.AskQuestionPopup = new AskQuestionPopupVM()
                    {
                        MakeName = objMakeModel.Make.MakeName,
                        ModelName = objMakeModel.Model.ModelName,
                        ModelId = _modelId,
                        GAPageType = GAPages.Question_Page,
                        QnaGASource = "12", //GA  categories, refer gtmCodeAppender() in _LocationPopup.cshtml
                        IsSearchActive = IsMobile,
                        QnaSearch = new QnaSearchVM()
                        {
                            CityId = _cityId,
                            ModelId = (uint)objMakeModel.Model.ModelId,
                            VersionId = (uint)objMakeModel.VersionId,
                            MakeMaskingName = _makeMaskingName,
                            ModelMaskingName = _modelMaskingName,
                            PlatformId = IsMobile ? (uint)Platforms.Mobile : (uint)Platforms.Desktop,
                            PageName = GAPages.Question_Page,
                            QnaGASource = "12",
                            MakeName = objMakeModel.Make.MakeName,
                            ModelName = objMakeModel.Model.ModelName                            
                        }
                    };
                }                
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionDetailsModel.BindAskQuestionPopup(),ModelId:{0}", _modelId));
            }

        }
    }
}