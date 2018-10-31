using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Pager;
using Bikewale.Entities.Pages;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Models.QuestionAndAnswers;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IBikeModelsCacheRepository<int> _objModelCache = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly IBikeSeriesCacheRepository _seriesCache;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        private readonly IPager _pager = null;

        private uint _modelId = 0;
        private string makeMaskingName = string.Empty, model = string.Empty, series = string.Empty;
        private string modelMaskingName = string.Empty;
        private ushort curPageNo = 1;
        private uint MakeId;
        private BikeMakeEntityBase objMake = null;
        private bool isMakeLive, isSeriesAvailable;
        private BikeSeriesEntityBase objSeries;
        private EnumBikeBodyStyles BodyStyle = EnumBikeBodyStyles.AllBikes;
        private BikeModelEntity objModel = null;
        private string _adId_Mobile = "1529992685612";
        private string _adPath_Mobile = "/1017752/BikeWale_Mobile_QNA_Listing";
        private string _adId_Desktop = "1530623766284";
        private string _adPath_Desktop = "/1017752/BikeWale_QNA_Listing";
        private const int pageSize = 10, pagerSlotSize = 5;
        private uint _totalPagesCount;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionAnswerModel(string makeMasking, string modelMasking, IBikeMakesCacheRepository objMakeCache, IQuestions objQuestions, IBikeModelsCacheRepository<int> modelCache, IBikeSeriesCacheRepository seriesCache, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache, IBikeVersions<BikeVersionEntity, uint> objVersion, IBikeModels<BikeModelEntity, int> objModelEntity, IPager pager)
        {
            _objMakeCache = objMakeCache;
            _objQuestions = objQuestions;
            _objModelCache = modelCache;
            _seriesCache = seriesCache;
            _objModelMaskingCache = modelMaskingCache;
            _objVersion = objVersion;
            _objModelEntity = objModelEntity;
            _pager = pager;
            ParseQueryString(makeMasking, modelMasking);
            makeMaskingName = makeMasking;
            modelMaskingName = modelMasking;

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
                    int _startIndex = 0, _endIndex = 0;
                    objVM = new ModelQuestionsAnswersVM();
                    _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);
                    objVM.MakeModelBase = _objModelCache.GetBikeInfo(_modelId);
                    if (objVM.MakeModelBase == null)
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    else
                    {
                        GetQuestionAnswerData(objVM);
                        BindAdSlots(objVM);
                        SetBreadcrumbList(objVM);
                        BindAskQuestionPopup(objVM);

                        if (objVM.QuestionAnswerWrapper != null)
                        {

                            int recordCount = Convert.ToInt32(objVM.QuestionAnswerWrapper.TotalAnsweredQuestions);
                            objVM.StartIndex = _startIndex;
                            objVM.EndIndex = Math.Min(recordCount, _endIndex);
                            _totalPagesCount = (uint)_pager.GetTotalPages(recordCount, pageSize);
                            BindLinkPager(objVM, recordCount, makeMaskingName, modelMaskingName);
                            CreatePrevNextUrl(objVM, recordCount);
                        }
                        BindPageMetas(objVM);
                        SetPageJSONLDSchema(objVM);
                        String siteBaseUrl = IsMobile ? String.Format("{0}/m", BWConfiguration.Instance.BwHostUrl) : BWConfiguration.Instance.BwHostUrl;

                        objVM.BaseUrl = String.Format("{0}{1}", siteBaseUrl, UrlFormatter.FormatQnAUrl(makeMaskingName, modelMaskingName));

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
            ushort questionsShown = 10;
            try
            {
                if (objVM != null)
                {
                    objVM.QuestionAnswerWrapper = _objQuestions.GetQuestionAnswerList(_modelId, curPageNo, questionsShown);
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
                    int trimLength = IsMobile ? 150 : 360;
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



        private void ParseQueryString(string makeMasking, string modelMasking)
        {
            ModelMaskingResponse objResponse = null;
            Status = StatusCodes.ContentNotFound;
            string newMakeMasking = string.Empty;
            bool isMakeRedirection = false;
            try
            {
                newMakeMasking = ProcessMakeMaskingName(makeMasking, out isMakeRedirection);
                if (!string.IsNullOrEmpty(newMakeMasking) && !string.IsNullOrEmpty(makeMasking) && !string.IsNullOrEmpty(modelMasking))
                {
                    objResponse = _objModelMaskingCache.GetModelMaskingResponse(string.Format("{0}_{1}", makeMasking, modelMasking));

                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            _modelId = objResponse.ModelId;
                            Status = StatusCodes.ContentFound;
                        }
                        else if (objResponse.StatusCode == 301 || isMakeRedirection)
                        {
                            RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(modelMasking, objResponse.MaskingName).Replace(makeMasking, newMakeMasking);
                            Status = StatusCodes.RedirectPermanent;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.ParseQueryString(),ModelId:{0}", _modelId));
            }
        }

        private string ProcessMakeMaskingName(string make, out bool isMakeRedirection)
        {
            MakeMaskingResponse makeResponse = null;
            Common.MakeHelper makeHelper = new Common.MakeHelper();
            isMakeRedirection = false;
            if (!string.IsNullOrEmpty(make))
            {
                makeResponse = makeHelper.GetMakeByMaskingName(make);
                if (makeResponse != null)
                {
                    if (makeResponse.StatusCode == 200)
                    {
                        return makeResponse.MaskingName;
                    }
                    else if (makeResponse.StatusCode == 301)
                    {
                        isMakeRedirection = true;
                        return makeResponse.MaskingName;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            return string.Empty;
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
        /// Modified by: Dhruv Joshi
        /// Dated: 5th July 2018
        /// Description: Desktop AdSlots
        /// </summary>
        /// <param name="_objVM"></param>
        private void BindAdSlots(ModelQuestionsAnswersVM _objVM)
        {
            try
            {
                IDictionary<string, AdSlotModel> ads = new Dictionary<string, AdSlotModel>();
                NameValueCollection adInfo = new NameValueCollection();
                AdTags adTagsObj = _objVM.AdTags;
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
                _objVM.AdSlots = ads;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.BindAdSlots");
            }
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
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, String.Format("{0}questions-and-answers/", bikeUrl), _objVM.MakeModelBase.Model.ModelName + " Questions & Answers"));

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
        /// Modified by : Snehal Dange on 4th July 2018
        /// Desc : Modified page metas for desktop
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
                    if (objMetas != null)
                    {
                        objMetas.Title = String.Format("{0} {1} User Questions and Answers | BikeWale", makeName, modelName);
                        objMetas.Description = String.Format("Get answers to your specific questions on {0} from other users. Read questions and answers posted by them to get complete clarity on any of your doubts related to {0}.", modelName);
                        objMetas.Keywords = String.Format("{0} {1} questions, {0} {1} q&a, {0} {1} questions and answers, {0} {1} answers, {0} {1} user answers", makeName, modelName);

                        objMetas.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrl, UrlFormatter.FormatQnAUrl(makeMaskingName, modelMaskingName), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
                        objMetas.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrl, UrlFormatter.FormatQnAUrl(makeMaskingName, modelMaskingName), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));

                        if (curPageNo > 1)
                        {
                            objMetas.Description = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objMetas.Description);
                            objMetas.Title = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objMetas.Title);
                        }
                    }

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
        /// Modified By : Kumar Swapnil on 18 July 2018
        /// Description : Added Questions Schema
        /// </summary>
        private void SetPageJSONLDSchema(ModelQuestionsAnswersVM objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            ushort pos = 1;
            if (webpage != null)
            {
                ICollection<Question> questionList = new Collection<Question>();

                foreach (var currentQuestion in objPageMeta.QuestionAnswerWrapper.QuestionList)
                {
                    Answer answer = new Answer
                    {
                        AuthorObj = new Author { Name = currentQuestion.Answer.AnsweredBy.CustomerName },
                        Text = currentQuestion.Answer.Text,
                        Position = 1,
                        CreatedOn = currentQuestion.Answer.AnsweredOn
                    };

                    ICollection<Answer> answerList = new Collection<Answer>();
                    answerList.Add(answer);

                    Question question = new Question
                    {
                        Text = currentQuestion.Question.Text,
                        DateCreated = currentQuestion.Question.AskedOn,
                        AuthorObj = new Author { Name = currentQuestion.Question.AskedBy.CustomerName },
                        AnswersCount = (uint)answerList.Count,
                        Position = pos++,
                        AcceptedAnswerObj = new AcceptedAnswer { AnswersList = answerList }
                    };

                    questionList.Add(question);
                }

                QuestionAnswerWrapper questionAnswerWrapper = new QuestionAnswerWrapper { QuestionList = questionList };

                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
                objPageMeta.PageMetaTags.PageSchemaJSON = SchemaHelper.JsonSerialize(questionAnswerWrapper);
            }
        }

        private void BindLinkPager(ModelQuestionsAnswersVM objData, int recordCount, string make, string model)
        {
            try
            {
                PagerEntity objPager = new PagerEntity();
                {
                    objPager.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatQnAUrl(make, model));
                }
                objPager.PageNo = curPageNo;
                objPager.PagerSlotSize = pagerSlotSize;
                objPager.PageUrlType = "page/";
                objPager.TotalResults = recordCount;
                objPager.PageSize = pageSize;

                objData.PagerEntity = objPager;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.BindLinkPager()");
            }
        }


        private void CreatePrevNextUrl(ModelQuestionsAnswersVM objData, int recordCount)
        {
            try
            {
                string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
                string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
                int totalPages = _pager.GetTotalPages(recordCount, pageSize);
                if (totalPages > 1)
                {
                    if (curPageNo == 1)
                    {
                        nextPageNumber = "2";
                        objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                    }
                    else if (curPageNo == totalPages)
                    {
                        prevPageNumber = Convert.ToString(curPageNo - 1);
                        objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                    }
                    else
                    {
                        prevPageNumber = Convert.ToString(curPageNo - 1);
                        objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                        nextPageNumber = Convert.ToString(curPageNo + 1);
                        objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.QuestionsAnswers.QuestionAnswerModel.CreatePrevNextUrl()");
            }
        }
    }
}