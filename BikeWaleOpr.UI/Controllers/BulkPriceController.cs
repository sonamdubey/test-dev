using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bikewale.Notifications;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Models.ManagePrices;
using BikewaleOpr.Interface.BikePricing;
using System.IO;
using BikewaleOpr.Interface.Location;
using System.Xml;
using Bikewale.Utility;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created By : Prabhu Puredla on 18 May 2018
    /// Description : Controller to manage bulkpriceupload page
    /// </summary>
    [Authorize]
    public class BulkPriceController : Controller
    {
        private readonly IBikeMakesRepository _bikeMakeRepos;
        private readonly IBulkPriceRepository _bulkPriceRepos;
        private readonly ILocation _locationRepos;
        private readonly IBulkPrice _bulkPrice;

        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// </summary>
        /// <returns></returns>
        public BulkPriceController(IBikeMakesRepository bikeMakeRepos, IBulkPriceRepository bulkPriceRepos, ILocation locationRepos, IBulkPrice bulkprice)
        {
            _bulkPriceRepos = bulkPriceRepos;
            _bikeMakeRepos = bikeMakeRepos;
            _locationRepos = locationRepos;
            _bulkPrice = bulkprice;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// Description : Method which gives landing page for the bulk price upload
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("prices/bulkupload/")]
        public ActionResult Index()
        {
            try
            {
                BulkPriceModel bulkPriceModel = new BulkPriceModel(_bulkPriceRepos, _bikeMakeRepos, _locationRepos, _bulkPrice);

                return View(bulkPriceModel.GetMakesAndStates());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.Index");
                return View((BulkPriceModel)null);
            }

        }

        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// Description : Method to return mapped bikes page
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="makeName"></param>
        /// <returns></returns>
        [HttpGet, Route("prices/bulkupload/mappedbikes/make/{makeId:int}/")]
        public ActionResult GetMappedBikes(uint makeId, string makeName)
        {
            if (makeId <= 0)
            {
                return RedirectToAction("Index");
            }
            MappedBikesVM mappedBikesVM = null;
            try
            {
                BulkPriceModel bulkPriceModel = new BulkPriceModel(_bulkPriceRepos, _bikeMakeRepos, _locationRepos, _bulkPrice);

                mappedBikesVM = bulkPriceModel.GetMappedBikesData(makeId);

                if (mappedBikesVM != null)
                {
                    mappedBikesVM.MakeId = makeId;
                    mappedBikesVM.MakeName = makeName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.GetMappedBikes");
            }

            return View("MappedBikes", mappedBikesVM);
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// Description : Method to delete mapped bikes of a make
        /// </summary>
        /// <param name="mappingId"> id to identify a mapping in the database</param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [Route("prices/bulkupload/mappedbikes/make/{makeId:int}/{mappingId:int}/delete/")]
        public ActionResult DeleteMappedBike(uint makeId, uint mappingId,string makeName)
        {
            try
            {
                if(mappingId <= 0)
                {
                    return RedirectToAction("GetMappedBikes", new { makeId = makeId, makeName = makeName });
                }
                _bulkPriceRepos.DeleteMappedBike(mappingId, UInt32.Parse(BikeWaleOpr.Common.CurrentUser.Id));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.DeleteMappedBike");
            }
            return RedirectToAction("GetMappedBikes", new { makeId = makeId,makeName = makeName });
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// Description : Method to get all mapped cities of a state
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="stateName"></param>
        /// <returns></returns>
        [HttpGet, Route("prices/bulkupload/mappedcities/state/{stateId:int}/")]
        public ActionResult GetMappedCities(uint stateId, string stateName)
        {
            if (stateId <= 0)
            {
                return RedirectToAction("Index");
            }
            MappedCitiesVM viewModel = null;
            try
            {
                BulkPriceModel bulkPriceModel = new BulkPriceModel(_bulkPriceRepos, _bikeMakeRepos, _locationRepos, _bulkPrice);

                viewModel = bulkPriceModel.GetMappedCitiesData(stateId);

                if (viewModel != null)
                {
                    viewModel.StateId = stateId;
                    viewModel.StateName = stateName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.GetMappedCities");
            }
            return View("MappedCities", viewModel);
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 23 May 2018
        /// Description : Method to delete a mapped city
        /// </summary>
        /// <param name="mappingId"></param>
        /// <param name="stateId"></param>
        /// <param name="stateName"></param>
        /// <returns></returns>
        [Route("prices/bulkupload/mappedcities/state/{stateId:int}/{mappingId:int}/delete/")]
        public ActionResult DeleteMappedCity(uint mappingId, uint stateId, string stateName)
        {
            try
            {
                if (mappingId <= 0)
                {
                    return RedirectToAction("GetMappedCities", new { stateId = stateId, stateName = stateName });
                }
                _bulkPriceRepos.DeleteMappedCity(mappingId, UInt32.Parse(BikeWaleOpr.Common.CurrentUser.Id));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.DeleteMappedCity");
            }
            return RedirectToAction("GetMappedCities", new { stateId = stateId, stateName = stateName });
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 25 May 2018
        /// Description : Method to return upload price page which takes prices file as input
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="makeName"></param>
        /// <returns></returns>
        [Route("prices/bulkupload/make/{makeId:int}/")]
        public ActionResult UploadPricesForMake(uint makeId, string makeName)
        {
            if (makeId <= 0)
            {
                return RedirectToAction("Index");
            }
            UploadPricesVM uploadPricesVM = null;
            try
            {
                BulkPriceModel bulkPriceModel = new BulkPriceModel(_bulkPriceRepos, _bikeMakeRepos, _locationRepos, _bulkPrice);

                TempData["Data"] = null;

                uploadPricesVM = bulkPriceModel.GetUploadPricesVM();

                if (uploadPricesVM != null)
                {
                    uploadPricesVM.MakeId = makeId;
                    uploadPricesVM.MakeName = makeName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.UploadPricesForMake");
            }
            return View("UploadPricesFile", uploadPricesVM);
        }

        /// <summary>
        ///  Created By : Prabhu Puredla on 5 june 2018
        /// Description : Method to save uploaded prices file
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="makeName"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        [HttpPost, Route("prices/bulkupload/make/{makeId:int}/pricefile/save/")]
        public ActionResult SavePriceFile(uint makeId, string makeName, HttpPostedFileBase xmlData)
        {
            if (makeId <= 0)
            {
                return RedirectToAction("Index");
            }
            CompositeBulkPriceVM compositeBulkPriceVM = null;
            try
            {
                //convert the httppostedfilebase to xml reader
                XmlReader reader = null;

                using (Stream inputStream = xmlData.InputStream)
                {
                    if (inputStream != null)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        memoryStream = new MemoryStream();

                        inputStream.CopyTo(memoryStream);
                        if (memoryStream.CanRead)
                        {
                            memoryStream.Position = 0;
                            reader = XmlReader.Create(memoryStream);
                        }
                    }
                }
                // redirect to step 1 with error msg
                if (reader == null)
                {
                    TempData["Message"] = "";
                    return RedirectToAction("UploadPricesForMake", new { makeId = makeId, makeName = makeName });
                }

                BulkPriceModel bulkPriceModel = new BulkPriceModel(_bulkPriceRepos, _bikeMakeRepos, _locationRepos, _bulkPrice);
                compositeBulkPriceVM = bulkPriceModel.GetProcessedData(makeId, reader);

                TempData["Data"] = compositeBulkPriceVM;

                if (compositeBulkPriceVM != null && compositeBulkPriceVM.UnmappedBikes != null && compositeBulkPriceVM.UnmappedBikes.Count() == 0)
                {
                    return RedirectToAction("GetUnmappedCities");
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.SavePriceFile");
            }
            return RedirectToAction("GetUnmappedBikes");
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Method to return unmapped bikes in the uploaded file
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("prices/bulkupload/make/{makeId:int}/unmappedbikes/")]
        public ActionResult GetUnmappedBikes()
        {
            try
            {
                if (TempData["Data"] != null)
                {
                    CompositeBulkPriceVM compositeBulkPriceVM = (CompositeBulkPriceVM)TempData["Data"];
                    TempData["Data"] = compositeBulkPriceVM;
                    return View("UnmappedBikes", compositeBulkPriceVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.GetUnmappedBikes");
            }

            return View("UnmappedBikes", null);
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Method to map the  unmapped bikes in the uploaded file
        /// </summary>
        /// <param name="oemBikeName"></param>
        /// <param name="versionid"></param>
        /// <returns></returns>
        [HttpPost, Route("prices/bulkupload/make/{makeId:int}/unmappedbikes/map/")]
        public bool MapUnmappedBikes(string oemBikeName, uint versionid)
        {
            try
            {
                if (TempData["Data"] != null)
                {
                    CompositeBulkPriceVM compositeBulkPriceVM = (CompositeBulkPriceVM)TempData["Data"];
                    TempData["Data"] = compositeBulkPriceVM;

                    return _bulkPrice.MapUnmappedBike(oemBikeName, versionid, compositeBulkPriceVM.UnmappedOemPricesList, compositeBulkPriceVM.UpdatedPriceList, compositeBulkPriceVM.UnmappedBikes, UInt32.Parse(BikeWaleOpr.Common.CurrentUser.Id));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.MapUnmappedBikes");
            }
            return false;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Method to get unmapped cities in the uploaded file
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("prices/bulkupload/make/{makeId:int}/unmappedcities/")]
        public ActionResult GetUnmappedCities()
        {
            try
            {
                if (TempData["Data"] != null)
                {
                    CompositeBulkPriceVM compositeBulkPriceVM = (CompositeBulkPriceVM)TempData["Data"];
                    TempData["Data"] = compositeBulkPriceVM;

                    if (compositeBulkPriceVM != null && compositeBulkPriceVM.UnmappedCities != null && !compositeBulkPriceVM.UnmappedCities.Any())
                    {
                        return RedirectToAction("GetUpdatablePrices");
                    }
                    return View("UnmappedCities", compositeBulkPriceVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.GetUnmappedCities");
            }
            return View("UnmappedCities", null);
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Method to map unmapped cities 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="oemCityName"></param>
        /// <returns></returns>
        [HttpPost, Route("prices/bulkupload/make/{makeId:int}/unmappedcities/map/")]
        public bool MapUnmappedCities(uint cityId, string oemCityName)
        {
            try
            {
                if (TempData["Data"] != null)
                {
                    CompositeBulkPriceVM compositeBulkPriceVM = (CompositeBulkPriceVM)TempData["Data"];
                    TempData["Data"] = compositeBulkPriceVM;

                    return _bulkPrice.MapUnmappedCity(oemCityName, cityId, compositeBulkPriceVM.UnmappedOemPricesList, compositeBulkPriceVM.UpdatedPriceList, compositeBulkPriceVM.UnmappedCities, UInt32.Parse(BikeWaleOpr.Common.CurrentUser.Id));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.MapTheUnmappedBikes");
            }
            return false;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Method to get updatable prices of a make which are in the uploaded file
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("prices/bulkupload/make/{makeId:int}/uploadprices/")]
        public ActionResult GetUpdatablePrices()
        {
            try
            {
                if (TempData["Data"] != null)
                {
                    CompositeBulkPriceVM compositeBulkPriceVM = (CompositeBulkPriceVM)TempData["Data"];
                    TempData["Data"] = compositeBulkPriceVM;

                    var price = Format.FormatNumeric(compositeBulkPriceVM.UpdatedPriceList.Count().ToString());

                    return View("UpdatablePrices", null, price);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.GetUpdatablePrices");
            }
            return View("UpdatablePrices", null);
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : action method to update all prices of a make
        /// </summary>
        /// <returns></returns>
        [Route("prices/bulkupload/make/{makeId:int}/uploadprices/save/")]
        public bool UploadPrices()
        {
            try
            {
                if (TempData["Data"] != null)
                {
                    CompositeBulkPriceVM compositeBulkPriceVM = (CompositeBulkPriceVM)TempData["Data"];
                    
                    return _bulkPrice.SavePrices(compositeBulkPriceVM.UpdatedPriceList, UInt32.Parse(BikeWaleOpr.Common.CurrentUser.Id));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Presentation.Controllers.UploadPrices");
            }
            return false;
        }
    }
}