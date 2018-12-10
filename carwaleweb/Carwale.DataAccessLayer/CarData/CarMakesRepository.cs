using Carwale.DAL.CoreDAL;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Interfaces;
using Carwale.Notifications;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Linq;
using Carwale.Notifications.Logs;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Geolocation;

namespace Carwale.DAL.CarData
{
    public class CarMakesRepository : RepositoryBase, ICarMakesRepository
    {
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;

        public List<CarMakeEntityBase> GetCarMakesByType(string type, Modules? module = null, bool? isPopular = null, int filter = 0,bool isCriticalRead = false)
        {
            var carMakesList = new List<CarMakeEntityBase>();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeCond", type.ToString().ToLower());
                param.Add("v_module", module != null ? module : null);
                if (isPopular != null) param.Add("v_filter", isPopular);
                else if(filter > 0) param.Add("v_filter", filter);
                else param.Add("v_filter", null);

                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    carMakesList = con.Query<CarMakeEntityBase>("GetCarMakes_v17_9_1", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return carMakesList ?? new List<CarMakeEntityBase>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<MakeLogoEntity> GetCarMakesWithLogo(string type,bool isCriticalRead = false)
        {
            var carMakesList = new List<MakeLogoEntity>();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeCond", type.ToString().ToLower());
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    carMakesList = con.Query<MakeLogoEntity>("GetCarMakes_v16_11_7", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarMakesRepository.GetCarMakesByType()");
                objErr.LogException();
                throw;
            }
            return carMakesList ?? new List<MakeLogoEntity>() ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carMake"></param>
        /// <returns></returns>
        public CarMakesEntity GetMakeDetailsByName(string carMake, bool isCriticalRead = false)
        {
            var makeDetails = new CarMakesEntity();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeName", carMake);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    return con.Query<CarMakesEntity>("GetMakeIdFromMakeName_v16_11_7",param,commandType:CommandType.StoredProcedure).First();
                }                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarMakesRepository.GetCarMakesByType()");
                objErr.LogException();
                throw;
            }
        }

        ///<summary>
        ///Returns car make description based on makeId passed
        ///Written by : Shalini on 09/07/14
        ///</summary>

        public CarMakeDescription GetCarMakeDescription(int makeId, bool isCriticalRead = false)
        {
            var makeDesc = new CarMakeDescription();

            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeId", makeId > 0 ? makeId : 0);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    makeDesc.MakeDescription = con.Query<string>("GetCarMakeDesc_v16_11_7", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarMakesRepository.GetCarMakeDescription()");
                objErr.LogException();
                throw;
            }
            return makeDesc;
        }


        /// <summary>
        /// Created By: Kirtan Shetty
        /// Returns Car Makes details for the give Year
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        public List<ValuationMake> GetValuationMakes(int year, bool isCriticalRead = false)
        {
            var carMakeList = new List<ValuationMake>();
            string hostUrl = "https://" + ConfigurationManager.AppSettings["WebApi"].ToString() + "/webapi/carmodeldata/getvaluationmodels/?year=" + year + "&make=";
            try
            {
                var param = new DynamicParameters();
                param.Add("v_caryear", year);

                using (var con = isCriticalRead ? ClassifiedMySqlMasterConnection : ClassifiedMySqlReadConnection)
                {
                    carMakeList = con.Query<ValuationMake>("GetValuationMakes", param, commandType: CommandType.StoredProcedure).Select<ValuationMake, ValuationMake>(vm => { vm.Url = hostUrl + vm.Id; return vm; }).AsList();
                    LogLiveSps.LogSpInGrayLog("GetValuationMakes");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsRepository.GetValuationMakes()");
                objErr.LogException();
                throw;
            }
            return carMakeList;
        }       

        /// <summary>
        ///  Returns the PageMetaTag Values 
        ///  Written By : Shalini on 24/11/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public PageMetaTags GetMakePageMetaTags(int makeId, int pageId, bool isCriticalRead = false)
        {
            var pageMetaTags = new PageMetaTags();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_PageId", pageId > 0 ? pageId : 0);
                param.Add("v_MakeId", makeId > 0 ? makeId : 0);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    pageMetaTags = con.Query<PageMetaTags>("PageMetaTagsByMake_v16_11_7", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarMakesRepository.GetMakePageMetaTags()");
                objErr.LogException();
                throw;
            }
            return (pageMetaTags==null)?new PageMetaTags():pageMetaTags;
        }

        /// <summary>
        /// Returns the Car Make Details based on MakeId passed 
        /// Written By : Shalini on 25/11/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public CarMakeEntityBase GetCarMakeDetails(int makeId, bool isCriticalRead = false)
        {
            var makeDetails = new CarMakeEntityBase();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeId", makeId>0?makeId:0);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    makeDetails = con.Query<CarMakeEntityBase>("GetMakeDetails_v17_7_2", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return makeDetails;
        }

        public IEnumerable<CarMakeEntityBase> GetMakes(Modules module, string makeIds, bool isCriticalRead = false)
        {
            IEnumerable<CarMakeEntityBase> makeDetails = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeIds", makeIds);
                param.Add("v_Module", module);

                using (var con = isCriticalRead ? ClassifiedMySqlMasterConnection : ClassifiedMySqlReadConnection)
                {
                    makeDetails = con.Query<CarMakeEntityBase>("cwmasterdb.GetMakesDetails", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return makeDetails;
        }

        /// <summary>
        /// Returns the No.of Models and Model Segments required for MakePage summary
        /// Written By:Shalini on 26/11/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<CarMakeDescription> GetSummary(int makeId, bool isCriticalRead = false)
        {
            var makeSummary = new List<CarMakeDescription>();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeId", makeId > 0 ? makeId : 0);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    makeSummary = con.Query<CarMakeDescription>("GetSummaryForMake_v16_11_7", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarMakesRepository.GetSummary()");
                objErr.LogException();
                throw;
            }
            return makeSummary;
        }

        public List<CarMakeEntityBase> GetMakeListWithAvailableDealer(bool isCriticalRead = false)
        {
            List<CarMakeEntityBase> makeIds = null;
            try
            {
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    makeIds = con.Query<CarMakeEntityBase>("GetMakeIdsWithAvailableDealer_v17_8_4", commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return makeIds;
        }
        public List<Cities> GetAllCitiesHavingDealerByMake(int makeId, bool isCriticalRead = false)
        {
            List<Cities> cities = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeId", makeId > 0 ? makeId : 0 );
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    cities = con.Query<Cities>("GetCitiesHavingDealersByMake_v17_8_4", param,commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return cities;
        }
        public bool GetDealerAvailabilityForMakeCity(int make, int city, bool isCriticalRead = false)
        {
            bool isDealerAvailable = false;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeId", make > 0 ? make : 0);
                param.Add("v_CityId", city > 0 ? city : 0);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    isDealerAvailable = con.Query<int>("GetDealerListByMakeCity", param, commandType: CommandType.StoredProcedure).Single() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isDealerAvailable;
        }
    }// class
}
