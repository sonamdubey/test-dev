using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Models.QuestionAndAnswers;
using Bikewale.Models.QuestionsAnswers;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class QuestionAndAnswersController : Controller
    {
        private readonly IQuestions _objQuestions;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly IBikeSeriesCacheRepository _seriesCache;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionAndAnswersController(IBikeMakesCacheRepository objMakeCache, IQuestions objQuestions, IBikeModelsCacheRepository<int> modelCache, IBikeSeriesCacheRepository seriesCache, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache, IBikeVersions<BikeVersionEntity, uint> objVersion, IBikeModels<BikeModelEntity, int> objModelEntity)
        {
            _objMakeCache = objMakeCache;
            _objQuestions = objQuestions;
            _modelCache = modelCache;
            _seriesCache = seriesCache;
            _modelMaskingCache = modelMaskingCache;
            _objVersion = objVersion;
            _objModelEntity = objModelEntity;

        }

        [Route("m/qna/{makeMaskingName}-bikes/{modelMaskingName}/")]
        public ActionResult Model_Index_Mobile(string makeMaskingName, string modelMaskingName)
        {
            ModelQuestionsAnswersVM objVM = null;
            QuestionAnswerModel modelobj = new QuestionAnswerModel(makeMaskingName, modelMaskingName, _objMakeCache, _objQuestions, _modelCache, _seriesCache, _modelMaskingCache, _objVersion, _objModelEntity);

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

        [Route("qna/{makeMaskingName}-bikes/{modelMaskingName}/")]
        public ActionResult Model_Index(string makeMaskingName, string modelMaskingName)
        {
            ModelQuestionsAnswersVM objVM = null;
            QuestionAnswerModel modelobj = new QuestionAnswerModel(makeMaskingName, modelMaskingName, _objMakeCache, _objQuestions, _modelCache, _seriesCache, _modelMaskingCache, _objVersion, _objModelEntity);
            objVM = modelobj.GetData();
            return View(objVM);
        }
    }
}