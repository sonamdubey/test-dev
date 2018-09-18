using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Models.QuestionAndAnswers;
using Bikewale.Models.QuestionsAnswers;
using Bikewale.Utility;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class QuestionAndAnswersController : Controller
    {
        private readonly IQuestions _objQuestions;
        private readonly IBikeModelsCacheRepository<int> _modelCache;
        private readonly IBikeMakesCacheRepository _objMakeCache;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly IBikeSeriesCacheRepository _seriesCache;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity;
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IPager _pager;
        private readonly IAnswers _objAnswers;
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly QuestionsAnswers.BAL.IQuestions _objQNAQuestions = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionAndAnswersController(IBikeMakesCacheRepository objMakeCache,
            IQuestions objQuestions,
            IBikeModelsCacheRepository<int> modelCache,
            IBikeSeriesCacheRepository seriesCache,
            IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache,
            IBikeVersions<BikeVersionEntity, uint> objVersion,
            IBikeModels<BikeModelEntity, int> objModelEntity,
            IPager pager,
            IAnswers objAnswers,
            IBikeInfo bikeInfo,
            QuestionsAnswers.BAL.IQuestions objQNAQuestions,
            ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            ICityCacheRepository objCityCache)
        {
            _objMakeCache = objMakeCache;
            _objQuestions = objQuestions;
            _modelCache = modelCache;
            _seriesCache = seriesCache;
            _modelMaskingCache = modelMaskingCache;
            _objVersion = objVersion;
            _objModelEntity = objModelEntity;
            _pager = pager;
            _objAnswers = objAnswers;
            _objQNAQuestions = objQNAQuestions;
            _bikeInfo = bikeInfo;
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objCityCache = objCityCache;
        }

        [Route("m/qna/{makeMaskingName}-bikes/{modelMaskingName}/")]
        public ActionResult Model_Index_Mobile(string makeMaskingName, string modelMaskingName)
        {
            ModelQuestionsAnswersVM objVM = null;
            QuestionAnswerModel modelobj = new QuestionAnswerModel(makeMaskingName, modelMaskingName, _objMakeCache, _objQuestions, _modelCache, _seriesCache, _modelMaskingCache, _objVersion, _objModelEntity, _pager);

            if (modelobj.Status == StatusCodes.ContentNotFound)
            {
                return HttpNotFound();
            }
            else if (modelobj.Status == StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(modelobj.RedirectUrl);
            }
            else
            {
                modelobj.IsMobile = true;
                objVM = modelobj.GetData();
                if (modelobj.Status.Equals(StatusCodes.ContentNotFound))
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(objVM);
                }
            }

        }

        [Bikewale.Filters.DeviceDetection]
        [Route("qna/{makeMaskingName}-bikes/{modelMaskingName}/")]
        public ActionResult Model_Index(string makeMaskingName, string modelMaskingName)
        {
            ModelQuestionsAnswersVM objVM = null;
            QuestionAnswerModel modelobj = new QuestionAnswerModel(makeMaskingName, modelMaskingName, _objMakeCache, _objQuestions, _modelCache, _seriesCache, _modelMaskingCache, _objVersion, _objModelEntity, _pager);

            if (modelobj.Status == StatusCodes.ContentNotFound)
            {
                return HttpNotFound();
            }
            else if (modelobj.Status == StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(modelobj.RedirectUrl);
            }
            else
            {
                objVM = modelobj.GetData();
                if (modelobj.Status.Equals(StatusCodes.ContentNotFound))
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(objVM);
                }
            }
        }


        /// <summary>
        /// Created by : Snehal Dange on 12th July 2018
        /// Desc : Actionmethod created to get `Answer page` for external users
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="answerObj"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("m/questions-and-answers/answer/")]
        public ActionResult AnswerPage_Mobile(string q, Entities.QuestionAndAnswers.Sources source = Sources.Invalid, string returnUrl = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(q))
                {
                    // Passing `queryString` in constructor is necessary for ParseQueryString() function in SaveAnswersPageModel()
                    SaveAnswerPageModel objModel = new SaveAnswerPageModel(q, _modelMaskingCache, _objQNAQuestions, _objAnswers, _objAuthCustomer, _objCustomer);
                    objModel.IsMobile = true;
                    objModel.Source = source;
                    if (objModel.Status == StatusCodes.ContentNotFound)
                    {
                        return Redirect(string.Format("{0}/m/", BWConfiguration.Instance.BwHostUrl));
                    }
                    else if (objModel.IsDuplicate)
                    {
                        return Redirect(string.Format("{0}/m/questions-and-answers/thank-you/?q={1}&source={2}&returnUrl={3}", BWConfiguration.Instance.BwHostUrl, q,(ushort)source,returnUrl));
                    }
                    else
                    {
                        SaveUserAnswerVM objVM = objModel.GetData();
                        objVM.ReturnUrl = returnUrl;
                        if (objVM != null)
                        {
                            return View("~/UI/views/QuestionAndAnswers/AnswerPage_Mobile.cshtml", objVM);
                        }
                    }
                }
                return Redirect((string.Format("{0}/m/", BWConfiguration.Instance.BwHostUrl)));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Controllers.QuestionAndAnswersController.SaveUserAnswer()");
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 23th July 2018
        /// Desc : Actionmethod created to get `Answer page` for external users for desktop
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="answerObj"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("questions-and-answers/answer/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult AnswerPage(string q, Entities.QuestionAndAnswers.Sources source = Sources.Invalid, string returnUrl = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(q))
                {
                    // Passing `queryString` in constructor is necessary for ParseQueryString() function in SaveAnswersPageModel()
                    SaveAnswerPageModel objModel = new SaveAnswerPageModel(q, _modelMaskingCache, _objQNAQuestions, _objAnswers, _objAuthCustomer, _objCustomer);
                    objModel.Source = source;
                    if (objModel.Status == StatusCodes.ContentNotFound)
                    {
                        return Redirect(BWConfiguration.Instance.BwHostUrl);
                    }
                    else if (objModel.IsDuplicate)
                    {
                        return Redirect(string.Format("{0}/questions-and-answers/thank-you/?q={1}&source={2}&returnUrl={3}", BWConfiguration.Instance.BwHostUrl, q, (ushort)source, returnUrl));
                    }
                    else
                    {
                        SaveUserAnswerVM objVM = objModel.GetData();
                        objVM.ReturnUrl = returnUrl;
                        if (objVM != null)
                        {
                            return View("~/UI/views/QuestionAndAnswers/AnswerPage.cshtml", objVM);
                        }
                    }
                }
                return Redirect(BWConfiguration.Instance.BwHostUrl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Controllers.QuestionAndAnswersController.SaveUserAnswer()");
                return HttpNotFound();
            }
        }


        /// <summary>
        /// Created by : Snehal Dange on 12th July 2018
        /// Desc: Method created to save the answer by external users through mail
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="answerObj"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost, Route("questions-and-answers/save/answer/"), ValidateAntiForgeryToken]
        public ActionResult SaveUserAnswer(string submittedAnswerText, string encryptedUrl, ushort platformId, ushort sourceId, string returnUrl = "")
        {
            try
            {
                CustomerEntityBase objCust = null;
                string questionId = "";
                int modelId = 0;
                if (TryParseQueryString(encryptedUrl, out objCust,out questionId,out modelId) && !string.IsNullOrEmpty(submittedAnswerText))
                {
                    bool saveAnswer = false;
                    BWClientInfo clientInfo = new BWClientInfo()
                    {
                        ApplicationId = 2,
                        PlatformId = platformId,
                        SourceId = sourceId,
                        ClientIp = Bikewale.Utility.CurrentUser.GetClientIP()
                    };
                    saveAnswer = _objAnswers.SubmitUserAnswer(questionId,submittedAnswerText,objCust.CustomerName,objCust.CustomerEmail, clientInfo);
                    if (saveAnswer)
                    {
                        return Redirect(string.Format("{0}/{1}questions-and-answers/thank-you/?q={2}&source={3}&returnUrl={4}", BWConfiguration.Instance.BwHostUrl, (platformId == Convert.ToUInt16(Platforms.Mobile) ? "m/" : string.Empty), encryptedUrl, sourceId,returnUrl));
                    }
                }
                return Redirect(string.Format("{0}{1}", BWConfiguration.Instance.BwHostUrl, (platformId == Convert.ToUInt16(Platforms.Mobile) ? "/m/" : string.Empty)));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Controllers.QuestionAndAnswersController.SaveUserAnswer():POST");
                return HttpNotFound();

            }

        }

        /// <summary>
        /// CREATED BY : Snehal Dange on 17th July 2018
        /// DESC : Parse qna query string to get modelid and questionid
        /// </summary>
        /// <param name="queryString"></param>
        private bool TryParseQueryString(string queryString, out CustomerEntityBase objCust, out string questionId, out int modelId)
        {
            bool isUrlCorrect = false;
            NameValueCollection queryCollection = null;
            try
            {
                string decodedString = Bikewale.Utility.TripleDES.DecryptTripleDES(queryString);
                queryCollection = HttpUtility.ParseQueryString(decodedString);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("TryParseQueryString({0})", queryString));
            }
            isUrlCorrect = (queryCollection != null && !string.IsNullOrEmpty(queryCollection["userName"]) && !string.IsNullOrEmpty(queryCollection["userEmail"]) & !string.IsNullOrEmpty(queryCollection["questionId"]) && !string.IsNullOrEmpty(queryCollection["modelId"]));

            if (isUrlCorrect)
            {
                objCust = new CustomerEntityBase();
                objCust.CustomerName = queryCollection["userName"];
                objCust.CustomerEmail = queryCollection["userEmail"];
                questionId = queryCollection["questionId"];
                modelId = Convert.ToInt32(queryCollection["modelId"]);
            }
            else
            {
                objCust = null;
                questionId = "";
                modelId = 0;
            }

            return isUrlCorrect;
        }


        /// <summary>
        /// Created By : Deepak Israni on 12 July 2018
        /// Description : Controller for thank you page after submission of answer.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [HttpGet, Route("m/questions-and-answers/thank-you/")]
        public ActionResult Index_ThankYou_Mobile(string q, Entities.QuestionAndAnswers.Sources source = Sources.Invalid, string returnUrl = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(q))
                {
                    ThankYouPageModel modelObj = new ThankYouPageModel(_objQuestions, _bikeInfo);
                    modelObj.IsMobile = true;
                    modelObj.Source = source;
                    modelObj.ReturnUrl = returnUrl;
                    ThankYouPageVM objVM = modelObj.GetData(q);
                    return View(objVM);
                }

                return Redirect(string.Format("{0}/m/", BWConfiguration.Instance.BwHostUrl));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Controllers.QuestionAndAnswersController.ThankYou()");
            }
            return HttpNotFound();
        }

        /// <summary>
        /// Created By : Snehal Dange on 23 July 2018
        /// Description : Controller for Desktop thank you page after submission of answer.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [HttpGet, Route("questions-and-answers/thank-you/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult Index_ThankYou(string q, Entities.QuestionAndAnswers.Sources source = Sources.Invalid,string returnUrl = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(q))
                {
                    ThankYouPageModel modelObj = new ThankYouPageModel(_objQuestions, _bikeInfo);
                    modelObj.Source = source;
                    modelObj.ReturnUrl = returnUrl;
                    ThankYouPageVM objVM = modelObj.GetData(q);
                    return View(objVM);
                }

                return Redirect(BWConfiguration.Instance.BwHostUrl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Controllers.QuestionAndAnswersController.ThankYou()");
            }
            return HttpNotFound();
        }
        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 8th August 2018
        /// Description: Action method for question details mobile page
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="modelMaskingName"></param>
        /// <param name="questionIdHash"></param>
        /// <returns></returns>
        [Route("m/qna/{makeMaskingName}-bikes/{modelMaskingName}/{questionIdHash}/")]
        public ActionResult Question_Details_Mobile(string makeMaskingName, string modelMaskingName, string questionIdHash)
        {
            try
            {
                QuestionDetailsVM objQuestionDetailsVM;
                QuestionDetailsModel objQuestionDetailsModel = new QuestionDetailsModel(makeMaskingName, modelMaskingName, questionIdHash, _objQuestions, _modelMaskingCache, _modelCache, _bikeInfo, _objCityCache);
                objQuestionDetailsModel.IsMobile = true;
                switch (objQuestionDetailsModel.Status)
                {
                    case StatusCodes.ContentNotFound:
                        return HttpNotFound();
                    case StatusCodes.RedirectPermanent:
                        return RedirectPermanent(objQuestionDetailsModel.RedirectUrl);
                    default:
                        objQuestionDetailsVM = objQuestionDetailsModel.GetData();
                        break;
                }
                if (objQuestionDetailsVM != null && objQuestionDetailsModel.Status != StatusCodes.ContentNotFound)
                {
                    return View(objQuestionDetailsVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Controllers.QuestionAnswerController.Question_Details_Mobile()");
            }
            return HttpNotFound();
        }



        /// <summary>
        /// Created by : Snehal Dange on 14th August 2018
        /// Desc : Action method for Question Dedicated page
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="modelMaskingName"></param>
        /// <param name="questionIdHash"></param>
        /// <returns></returns>
        [Route("qna/{makeMaskingName}-bikes/{modelMaskingName}/{questionIdHash}/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult Question_Details(string makeMaskingName, string modelMaskingName, string questionIdHash)
        {
            try
            {
                QuestionDetailsVM objQuestionDetailsVM;
                QuestionDetailsModel objQuestionDetailsModel = new QuestionDetailsModel(makeMaskingName, modelMaskingName, questionIdHash, _objQuestions, _modelMaskingCache, _modelCache, _bikeInfo, _objCityCache);
                switch (objQuestionDetailsModel.Status)
                {
                    case StatusCodes.ContentNotFound:
                        return HttpNotFound();
                    case StatusCodes.RedirectPermanent:
                        return RedirectPermanent(objQuestionDetailsModel.RedirectUrl);
                    default:
                        objQuestionDetailsVM = objQuestionDetailsModel.GetData();
                        break;
                }
                if (objQuestionDetailsVM != null && objQuestionDetailsModel.Status != StatusCodes.ContentNotFound)
                {
                    return View(objQuestionDetailsVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.UI.Controllers.QuestionAnswerController.Question_Details()");
            }
            return HttpNotFound();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 03 Sep 2018
        /// Description :   Save User answer
        /// </summary>
        /// <param name="answerData"></param>
        /// <returns></returns>
        [HttpPost, Route("qna/save/answer/")]
        public ActionResult SaveUserAnswer(AnswerSubmitData answerData)
        {
            try
            {
                bool saveAnswer = false;
                if (answerData != null && !string.IsNullOrEmpty(answerData.AnswerText))
                {
                    BWClientInfo clientInfo = new BWClientInfo()
                    {
                        ApplicationId = 2,
                        PlatformId = answerData.PlatformId,
                        SourceId = answerData.SourceId,
                        ClientIp = Bikewale.Utility.CurrentUser.GetClientIP()
                    };
                    saveAnswer = _objAnswers.SubmitUserAnswer(answerData.QuestionId,answerData.AnswerText, answerData.CustomerName, answerData.CustomerEmail, clientInfo);
                }
                return Json(saveAnswer);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Controllers.QuestionAndAnswersController.SaveUserAnswer():POST");
                return HttpNotFound();

            }

        }

    }
}