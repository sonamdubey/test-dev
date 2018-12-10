using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Interfaces.Classified;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using System.Data;
using Carwale.Notifications;
using Carwale.Entity.Classified;
using System.Web;
using Carwale.Entity.ViewModels;
using Dapper;
using Carwale.Notifications.Logs;
using Carwale.Entity;

namespace Carwale.DAL.Classified
{
    public class CommonOperationsRepository : RepositoryBase, ICommonOperationsRepository
    {
        IList<CarMakeEntityBase> ICommonOperationsRepository.GetLiveListingMakes()
        {
            List<CarMakeEntityBase> makeList = null;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    makeList = con.Query<CarMakeEntityBase>("getlivelistingmakes", commandType: CommandType.StoredProcedure).ToList();
                    LogLiveSps.LogSpInGrayLog("getlivelistingmakes");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DAL.Classified.GetLiveListingMakes()");
                objErr.LogException();
            }
            return makeList;
        }

        IList<DealerCityEntity> ICommonOperationsRepository.GetLiveListingCities()
        {
            var cityList = new List<DealerCityEntity>();
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    cityList = con.Query<DealerCityEntity>("getlivelistingcities", commandType: CommandType.StoredProcedure).AsList<DealerCityEntity>();
                    LogLiveSps.LogSpInGrayLog("getlivelistingcities");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DAL.Classified.GetLiveListingCities()");
                objErr.LogException();
                throw;
            }
            return cityList;
        }

        string ICommonOperationsRepository.GetMakeRootName(string makesList, string rootsList)
        {
            string rootsAndMakes = string.Empty;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("v_makeidlist", makesList, DbType.String);
                param.Add("v_rootidlist", rootsList, DbType.String);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    List<string> rootMakeList = con.Query<string>("usedcarfetchmakesandroots", param, commandType: CommandType.StoredProcedure).ToList();
                    LogLiveSps.LogSpInGrayLog("usedcarfetchmakesandroots");
                    if (rootMakeList != null)
                    {
                        rootsAndMakes = string.Join(", ", rootMakeList.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DAL.Classified.GetMakeRootName()");
                objErr.LogException();
            }
            return rootsAndMakes;
        }

        public UsedCarModel GetUsedCarListings()
        {
            var usedCarModel = new UsedCarModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_carcount", 20, DbType.Int32);
                LogLiveSps.LogSpInGrayLog("load_used_car_listings_15_8_2");
                using (var con = ClassifiedMySqlReadConnection)
                {
                    using (var reader = con.QueryMultiple("load_used_car_listings_15_8_2", param, commandType: CommandType.StoredProcedure))
                    {
                        usedCarModel.ListTopLiveListingCars = reader.Read<LiveListingTopEntity>().ToList();
                        usedCarModel.DealerOfTheMonth.ListDealerOfTheMonthCars = reader.Read<LiveListingTopEntity>().ToList();
                        usedCarModel.DealerOfTheMonth.CountDealerOfTheMonthCars = reader.Read<long>().Select<long, int>((count) => Convert.ToInt32(count)).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DAL.Classified.GetUsedCarListings()");
                objErr.LogException();
            }
            return usedCarModel;
        }

        public void SendErrorMail(Exception ex, string methodName)
        {
            ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : " + methodName);
            objErr.SendMail();
        }
        public CarModelMaskingResponse GetMakeDetailsByRootName(string rootName)
        {
            CarModelMaskingResponse carModelMaskingResponse = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_rootname", rootName);

                using (var con = ClassifiedMySqlReadConnection)
                {
                    carModelMaskingResponse = con.Query<CarModelMaskingResponse>("UsedGetMakeDetailsByRootName", param, commandType: CommandType.StoredProcedure).FirstOrDefault<CarModelMaskingResponse>();
                    LogLiveSps.LogSpInGrayLog("UsedGetMakeDetailsByRootName");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DAL.Classified.GetMakeDetailsByRootName()");
                objErr.LogException();
            }
            return carModelMaskingResponse;
        }
    }
}
