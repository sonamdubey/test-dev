using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Pages;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
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
    /// Model for dedicated qna page
    /// </summary>
    public class QuestionAnswerModel
    {
        public StatusCodes Status { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsMobile { get; internal set; }

        private readonly IQuestions _objQuestions;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly IBikeSeriesCacheRepository _seriesCache;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;

        private uint _modelId = 0;
        private string makeMasking = string.Empty, model = string.Empty, series = string.Empty;
        private string modelMasking = string.Empty;
        private int curPageNo = 1;
        private uint MakeId;
        private BikeMakeEntityBase objMake = null;
        private bool isMakeLive, isSeriesAvailable;
        private BikeSeriesEntityBase objSeries;
        private EnumBikeBodyStyles BodyStyle = EnumBikeBodyStyles.AllBikes;
        private BikeModelEntity objModel = null;
        private string _adId_Mobile = "1529992685612";
        private string _adPath_Mobile = "/1017752/BikeWale_Mobile_QNA_Listing";



        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionAnswerModel(string makeMasking, string modelMasking, IBikeMakesCacheRepository objMakeCache, IQuestions objQuestions, IBikeModelsCacheRepository<int> modelCache, IBikeSeriesCacheRepository seriesCache, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache, IBikeVersions<BikeVersionEntity, uint> objVersion, IBikeModels<BikeModelEntity, int> objModelEntity)
        {
            _objMakeCache = objMakeCache;
            _objQuestions = objQuestions;
            _modelCache = modelCache;
            _seriesCache = seriesCache;
            _modelMaskingCache = modelMaskingCache;
            _objVersion = objVersion;
            _objModelEntity = objModelEntity;
            ProcessQueryString(makeMasking, modelMasking);
        }

        /// <summary>
        /// Desc :  Get the data for question and answer dedicated page 
        /// </summary>
        /// <returns></returns>
        public ModelQuestionsAnswersVM GetData()
        {
            ModelQuestionsAnswersVM objVM = null;

            try
            {
                if (_modelId > 0)
                {
                    objVM = new ModelQuestionsAnswersVM();
                    objVM.MakeModelBase = _modelCache.GetBikeInfo(_modelId);
                    if (objVM.MakeModelBase == null)
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    else
                    {
                        GetQuestionAnswerData(objVM);
                        BindAdSlots(objVM);
                        SetBreadcrumbList(objVM);
                        BindPageMetas(objVM);
                        BindAskQuestionPopup(objVM);
                        SetPageJSONLDSchema(objVM);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.GetData():ModelId:- {0}", _modelId));
                Status = StatusCodes.ContentNotFound;
            }
            return objVM;
        }

        /// <summary>
        /// Created by : Snehal Dange on 28th June 2018
        /// Desc :  Get list of question and answers
        /// </summary>
        /// <param name="objVM"></param>
        private void GetQuestionAnswerData(ModelQuestionsAnswersVM objVM)
        {
            ushort pageNo = 1;
            ushort questionsShown = 10;
            try
            {
                if (objVM != null)
                {
                    objVM.QuestionAnswerWrapper = _objQuestions.GetQuestionAnswerList(_modelId, pageNo, questionsShown);
                    if (objVM.QuestionAnswerWrapper == null)
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    else if (objVM.QuestionAnswerWrapper.QuestionList == null)
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    else
                    {
                        FormatQuestionsAnswers(objVM.QuestionAnswerWrapper.QuestionList);
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.GetQuestionAnswerData() : Modelid:{0}", _modelId));
            }
        }


        /// <summary>
        /// Created by : Snehal Dange on 22nd Jan 2018
        /// Desc : Method created to format the answers into shortanswers
        /// </summary>
        /// <param name="enumerable"></param>
        private void FormatQuestionsAnswers(IEnumerable<Entities.QuestionAndAnswers.QuestionAnswer> questionAnsList)
        {
            try
            {
                if (questionAnsList != null)
                {
                    int trimLength = 150;
                    foreach (var quesAnsObj in questionAnsList)
                    {
                        var answerValue = quesAnsObj.Answer;
                        answerValue.StrippedText = (!string.IsNullOrEmpty(answerValue.Text) && (answerValue.Text.Length > trimLength) ? answerValue.Text.Substring(0, trimLength) : string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.FormatQuestionsAnswers(),ModelId:{0}", _modelId));
            }
        }



        private void ProcessQueryString(string makeMasking, string modelMasking)
        {
            try
            {
                var request = HttpContext.Current.Request;
                var queryString = request != null ? request.QueryString : null;

                if (queryString != null)
                {

                    if (!string.IsNullOrEmpty(queryString["pn"]))
                    {
                        string _pageNo = queryString["pn"];
                        if (!string.IsNullOrEmpty(_pageNo))
                        {
                            int.TryParse(_pageNo, out curPageNo);
                        }
                    }

                    if (!string.IsNullOrEmpty(makeMasking))
                    {
                        ProcessMakeMaskingName(request, makeMasking);
                        ProcessModelSeriesMaskingName(request, String.Format("{0}_{1}", makeMasking, modelMasking));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.ProcessQueryString(),ModelId:{0}", _modelId));
            }

        }

        private void ProcessMakeMaskingName(HttpRequest request, string make)
        {
            try
            {
                MakeMaskingResponse makeResponse = null;
                if (!string.IsNullOrEmpty(make))
                {
                    makeResponse = _objMakeCache.GetMakeMaskingResponse(make);
                }
                if (makeResponse != null)
                {
                    if (makeResponse.StatusCode == 200)
                    {
                        MakeId = makeResponse.MakeId;
                        objMake = _objMakeCache.GetMakeDetails(MakeId);
                        isMakeLive = objMake.IsNew && !objMake.IsFuturistic;
                    }
                    else if (makeResponse.StatusCode == 301)
                    {
                        Status = StatusCodes.RedirectPermanent;
                        RedirectUrl = request.RawUrl.Replace(make, makeResponse.MaskingName);
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.ProcessMakeMaskingName(),ModelId:{0}", _modelId));
            }

        }

        private void ProcessModelSeriesMaskingName(HttpRequest request, string maskingName)
        {
            try
            {
                SeriesMaskingResponse objResponse = null;
                if (!string.IsNullOrEmpty(maskingName))
                {
                    objResponse = _seriesCache.ProcessMaskingName(maskingName);
                }
                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        if (objResponse.IsSeriesPageCreated)
                        {
                            series = objResponse.MaskingName;
                            objSeries = new BikeSeriesEntityBase
                            {
                                SeriesId = objResponse.SeriesId,
                                BodyStyle = objResponse.BodyStyle,
                                SeriesName = objResponse.Name,
                                MaskingName = series,
                                IsSeriesPageUrl = true
                            };
                            this.BodyStyle = objSeries.BodyStyle;
                            isSeriesAvailable = true;
                        }
                        else
                        {
                            model = objResponse.MaskingName;
                            _modelId = objResponse.ModelId;
                            objModel = _modelMaskingCache.GetById((int)objResponse.ModelId);
                            isSeriesAvailable = objModel.ModelSeries.IsSeriesPageUrl;

                            IEnumerable<BikeVersionMinSpecs> objVersionsList = _objVersion.GetVersionMinSpecs(_modelId, false);
                            if (objVersionsList != null && objVersionsList.Any())
                            {
                                BodyStyle = objVersionsList.FirstOrDefault().BodyStyle;
                            }
                        }
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        Status = StatusCodes.RedirectPermanent;
                        RedirectUrl = request.RawUrl.Replace(objResponse.MaskingName, objResponse.NewMaskingName);
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.ProcessModelSeriesMaskingName(),ModelId:{0}", _modelId));
            }
        }
        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 25th June 2018
        /// Description: Populate AskQuestionVM
        /// </summary>
        /// <param name="_objVM"></param>
        private void BindAskQuestionPopup(ModelQuestionsAnswersVM _objVM)
        {
            try
            {
                var objMakeModel = _objVM.MakeModelBase;
                if (objMakeModel != null && objMakeModel.Make != null && objMakeModel.Model != null)
                {
                    _objVM.AskQuestionPopup = new AskQuestionPopupVM()
                    {
                        MakeName = objMakeModel.Make.MakeName,
                        ModelName = objMakeModel.Model.ModelName,
                        GAPageType = GAPages.QnA_Page
                    };
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.BindAskQuestionPopup(),ModelId:{0}", _modelId));
            }

        }

        /// <summary>
        /// Created By : Deepak Israni on 26 June 2018
        /// Description : To bind Ad Slots on page.
        /// </summary>
        /// <param name="_objVM"></param>
        private void BindAdSlots(ModelQuestionsAnswersVM _objVM)
        {
            AdTags adTagsObj = _objVM.AdTags;
            adTagsObj.AdPath = _adPath_Mobile;
            adTagsObj.AdId = _adId_Mobile;
            adTagsObj.Ad_320x50 = true;

            IDictionary<string, AdSlotModel> ads = new Dictionary<string, AdSlotModel>();

            NameValueCollection adInfo = new NameValueCollection();
            adInfo["adId"] = _adId_Mobile;
            adInfo["adPath"] = _adPath_Mobile;

            if (adTagsObj.Ad_320x50)
            {
                ads.Add(String.Format("{0}-0", _adId_Mobile), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._320x50], 0, 320, AdSlotSize._320x50, "Top", true));
            }

            _objVM.AdSlots = ads;
        }

        /// <summary>
        /// Created By : Deepak Israni on 26 June 2018
        /// Description : Add breadcrumbs information to page.
        /// </summary>
        /// <param name="objPage"></param>
        private void SetBreadcrumbList(ModelQuestionsAnswersVM _objVM)
        {
            try
            {
                if (_objVM.MakeModelBase != null && _objVM.MakeModelBase.Make != null && _objVM.MakeModelBase.Model != null)
                {
                    string makeMaskingName = _objVM.MakeModelBase.Make.MaskingName;
                    string modelMaskingName = _objVM.MakeModelBase.Model.MaskingName;

                    BikeSeriesEntityBase seriesDetails = _objModelEntity.GetSeriesByModelId(_modelId);

                    IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                    string bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                    string scooterUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                    string seriesUrl = string.Empty;
                    ushort position = 1;
                    if (IsMobile)
                    {
                        bikeUrl += "m/";
                    }

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", bikeUrl), "New Bikes"));

                    bikeUrl = string.Format("{0}{1}-bikes/", bikeUrl, makeMaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} Bikes", _objVM.MakeModelBase.Make.MakeName)));


                    if (_objVM.MakeModelBase.BodyStyleId.Equals(EnumBikeBodyStyles.Scooter) && !(_objVM.MakeModelBase.Make.IsScooterOnly))
                    {
                        if (IsMobile)
                        {
                            scooterUrl += "m/";
                        }
                        scooterUrl = string.Format("{0}{1}-scooters/", scooterUrl, makeMaskingName);

                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, scooterUrl, string.Format("{0} Scooters", _objVM.MakeModelBase.Make.MakeName)));
                    }

                    if (seriesDetails != null && seriesDetails.IsSeriesPageUrl)
                    {
                        seriesUrl = string.Format("{0}{1}/", bikeUrl, seriesDetails.MaskingName);

                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, seriesUrl, seriesDetails.SeriesName));
                    }


                    bikeUrl = string.Format("{0}{1}/", bikeUrl, modelMaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} {1}", _objVM.MakeModelBase.Make.MakeName, _objVM.MakeModelBase.Model.ModelName)));
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, _objVM.MakeModelBase.Model.ModelName + " Questions & Answers"));

                    _objVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.SetBreadcrumbList(),ModelId:{0}", _modelId));
            }
        }

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 27th June 2018
        /// Description: Bind Page Metas for QnA Page
        /// </summary>
        /// <param name="_objVM"></param>
        private void BindPageMetas(ModelQuestionsAnswersVM _objVM)
        {
            try
            {
                GenericBikeInfo genericInfoObj = _objVM.MakeModelBase;
                _objVM.Platform = IsMobile ? Platforms.Mobile : Platforms.Desktop;
                if (genericInfoObj != null && genericInfoObj.Make != null && genericInfoObj.Model != null)
                {
                    string makeName = genericInfoObj.Make.MakeName;
                    string modelName = genericInfoObj.Model.ModelName;

                    _objVM.ModelId = _modelId;
                    _objVM.Tags = string.Format("{0},{1}", genericInfoObj.Make.MaskingName, genericInfoObj.Model.MaskingName);

                    PageMetaTags objMetas = _objVM.PageMetaTags;
                    objMetas.Title = String.Format("{0} {1} User Questions and Answers | BikeWale", makeName, modelName);
                    objMetas.Description = String.Format("Get answers to your specific questions on {0} from other users. Read questions and answers posted by them to get complete clarity on any of your doubts related to {0}.", modelName);
                    objMetas.Keywords = String.Format("{0} {1} questions, {0} {1} q&a, {0} {1} questions and answers, {0} {1} answers, {0} {1} user answers", makeName, modelName);
                }



            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.BindPageMetas(),ModelId:{0}", _modelId));
            }
        }


        /// <summary>
        /// Created By : Sumit Kate on 28 Jun 2018
        /// Description : Webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(ModelBase objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList, SchemaHelper.QAPage);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }


    }
}