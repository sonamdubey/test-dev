using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.common;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace BikeWaleOpr.MVC.UI.Controllers.Content
{
    /// <summary>
    /// Created By : Ashish G. kamble on 1 Feb 2017
    /// </summary>
    [Authorize]
    public class MakesController : Controller
    {
        private readonly IBikeMakesRepository _makesRepo;
        private readonly IBikeModelsRepository _modelsRepo;
        private readonly IBikeModels _bikeModels;

        public MakesController(IBikeMakesRepository makesRepo, IBikeModelsRepository modelsRepo, IBikeModels bikeModels)
        {
            _makesRepo = makesRepo;
            _modelsRepo = modelsRepo;
            _bikeModels = bikeModels;
        }

        /// <summary>
        /// Action method to show the default view for the makes page
        /// </summary>
        /// <returns></returns>        
        public ActionResult Index()
        {
            IEnumerable<BikeMakeEntity> objMakes = null;

            string msg = Convert.ToString(TempData["msg"]);

            try
            {
                if (!String.IsNullOrEmpty(msg))
                {
                    ViewBag.SuccessMsg = msg;
                }
                else
                {
                    ViewBag.SuccessMsg = "";
                }

                objMakes = _makesRepo.GetMakesList();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MakesController/Index");
            }

            return View(objMakes);
        }

        /// <summary>
        /// Function to add the new make to the database
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(BikeMakeEntity make)
        {
            try
            {
                short isMakeExist = 0;
                int makeId = 0;

                make.UpdatedBy = BikeWaleOpr.Common.CurrentUser.Id;
                _makesRepo.AddMake(make, ref isMakeExist, ref makeId);

                if (isMakeExist == 1)
                    TempData["msg"] = "Make name or make masking name already exists. Can not insert duplicate name.";
                else if (isMakeExist == 0 && !string.IsNullOrEmpty(make.MakeName))
                    TempData["msg"] = make.MakeName + " make added successfully";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MakesController/Add");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Function to update the given make details
        /// Modified by : Aditi Srivastava on 24 May 2017
        /// Summary     : Send mail when make masking name is changed
        /// Modified By : Deepak Israni on 14 March 2018
        /// Description : Added call to update bikemodel ES Index
        /// Modified by : Sanskar Gupta on 27 April 2018
        /// Description : Send mail when Make name is changed.
        /// </summary>
        /// <returns></returns>         
        public ActionResult Update(BikeMakeEntity make)
        {
            try
            {
                if (make != null && make.MakeId > 0)
                {
                    string hostUrl = BWOprConfiguration.Instance.BwHostUrl;
                    IEnumerable<BikeModelMailEntity> models = null;
                    make.UpdatedBy = BikeWaleOpr.Common.CurrentUser.Id;
                    _makesRepo.UpdateMake(make);
                    if (string.Compare(make.OldMakeMasking, make.MaskingName) != 0)
                    {
                        models = _modelsRepo.GetModelsByMake((uint)make.MakeId, hostUrl, make.OldMakeMasking, make.MaskingName);

                        if (models != null)
                        {
                            String updatedIds = String.Join(",", models.Select(obj => Convert.ToString(obj.ModelId)));
                            _bikeModels.UpdateModelESIndex(updatedIds, "update");

                            IEnumerable<string> emails = Bikewale.Utility.GetEmailList.FetchMailList();
                            foreach (var mail in emails)
                            {
                                SendEmailOnModelChange.SendMakeMaskingNameChangeMail(mail, make.MakeName, models);
                            } 
                        }
                    }
                    else
                    {
                        if(string.Compare(make.OldMakeName, make.MakeName) != 0)
                        {
                            IEnumerable<string> emails = Bikewale.Utility.GetEmailList.FetchMailList(BWOprConfiguration.Instance.EmailsForMakeModelNameChange);
                            if (emails != null)
                            {
                                SendInternalEmail.OnFieldChanged(emails, "Name", make.OldMakeName, make.MakeName);
                            }
                        }

                        IEnumerable<BikeModelEntityBase> updatedModels = _modelsRepo.GetModelsByMake((uint)make.MakeId);
                        if (updatedModels != null)
                        {
                            String updatedIds = String.Join(",", updatedModels.Select(obj => Convert.ToString(obj.ModelId)));
                            _bikeModels.UpdateModelESIndex(updatedIds, "update"); 
                        }
                    }
                    TempData["msg"] = make.MakeName + " Make Updated Successfully";
                }
                else
                {
                    TempData["msg"] = "Please provide valid inputs";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MakesController/Update");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Function to delete the given make by using makeid
        /// </summary>
        /// <returns></returns>        
        public ActionResult Delete(int makeId)
        {
            try
            {
                if (makeId > 0)
                {
                    int updatedBy = Convert.ToInt32(BikeWaleOpr.Common.CurrentUser.Id);
                    _makesRepo.DeleteMake(makeId, updatedBy);

                    TempData["msg"] = "Make Deleted Successfully";
                }
                else
                {
                    TempData["msg"] = "Please provide valid make";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MakesController/Delete");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Created by Sajal Gupta on 20-11-2017
        /// Desc : Action method for page addfooterdata
        /// Modified by: Snehal Dange on 23rd Nov 2017
        /// Description: Added refresh cache logic
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public ActionResult AddFooterData(uint makeId, string makeName)
        {
            try
            {
                if (makeId > 0)
                {
                    MakeFooterPageModel objMakeFooter = new MakeFooterPageModel();
                    objMakeFooter.MakeFooterData = _makesRepo.GetMakeFooterCategoryData(makeId);
                    objMakeFooter.MakeName = makeName;
                    objMakeFooter.MakeId = makeId;

                    MemCachedUtility.Remove(string.Format("BW_FooterCategoriesandPrice_MK_{0}", makeId));
                    return View(objMakeFooter);
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "MakesController/AddFooterData");
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Created by Sajal Gupta on 20-11-2017
        /// Desc : Action method for saving addfooterdata
        /// Modified by: Snehal Dange on 23rd Nov 2017
        /// Description: Added refresh cache logic
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [Route("make/save/footerdata/")]
        public ActionResult SaveFooterData([System.Web.Http.FromBody]MakeFooterDto footerData)
        {
            try
            {
                if (footerData.MakeId > 0)
                {
                    _makesRepo.SaveMakeFooterData(footerData.MakeId, footerData.CategoryId, footerData.CategoryDescription, footerData.UserId);
                    MemCachedUtility.Remove(string.Format("BW_FooterCategoriesandPrice_MK_{0}", footerData.MakeId));
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "MakesController/SaveFooterData");
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Created by Sajal Gupta on 20-11-2017
        /// Desc : Action method for deleting footerdata
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [Route("make/delete/footerdata/")]
        public ActionResult DisableFooterData(uint makeId, string userId)
        {
            try
            {
                _makesRepo.DisableAllMakeFooterCategories(makeId, userId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "MakesController/DisableFooterData");
            }
            return RedirectToAction("Index");
        }

    }   // class
}   // namespace