using Carwale.Entity.Stock;
using Carwale.Interfaces;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using static Carwale.Entity.Stock.StockImageStatus;

namespace Carwale.DAL.Stock
{
    public class StockImageRepository : RepositoryBase, IStockImageRepository
    {
        public int Create(CarPhotos entity)
        {
            int photoId = -1;
            try
            {
                if (entity != null)
                {
                    var param = new DynamicParameters();
                    param.Add("v_sourceid", entity.SourceId, DbType.Int32);
                    param.Add("v_isdealer", entity.SellerType == 1, DbType.Int32);
                    param.Add("v_stockid", entity.StockId, DbType.Int32);
                    param.Add("v_description", entity.Description, DbType.String);
                    param.Add("v_ismain", entity.IsMain, DbType.Boolean);
                    param.Add("v_title", entity.Title, DbType.String);
                    param.Add("v_hosturl", entity.HostUrl, DbType.String);
                    param.Add("v_isreplicated", entity.IsReplicated, DbType.Boolean);
                    param.Add("v_isapproved", entity.IsApproved, DbType.Boolean);
                    param.Add("v_tc_carphotoid", entity.TC_CarPhotoId, DbType.Int32);
                    param.Add("v_originalimgpath", entity.OriginalImgPath, DbType.String);
                    param.Add("v_imagetype", entity.ImageType, DbType.Int32);
                    param.Add("v_photoid", DbType.Int32, direction: ParameterDirection.Output);

                    using (var con = ClassifiedMySqlMasterConnection)
                    {
                        con.Execute("carphotos_insertstockimagedetails_v18_6_1", param, commandType: CommandType.StoredProcedure);
                        photoId = param.Get<int>("v_photoid");
                    } 
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return photoId;
        }

        public int Update(CarPhotos entity)
        {
            int photoId = -1;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_sourceid", entity.SourceId, DbType.Int32);
                param.Add("v_sellertype", entity.SellerType, DbType.Int32);
                param.Add("v_stockid", entity.StockId, DbType.Int32);
                param.Add("v_description", entity.Description, DbType.String);
                param.Add("v_ismain", entity.IsMain, DbType.Boolean);
                param.Add("v_title", entity.Title, DbType.String);
                param.Add("v_isreplicated", entity.IsReplicated, DbType.Boolean);
                param.Add("v_isapproved", entity.IsApproved, DbType.Boolean);
                param.Add("v_tc_carphotoid", entity.TC_CarPhotoId, DbType.Int32);
                param.Add("v_originalimgpath", entity.OriginalImgPath, DbType.String);
                param.Add("v_photoid", DbType.Int32, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("carphotos_updatestockimagedetails_v17_3_3", param, commandType: CommandType.StoredProcedure);
                    photoId = param.Get<int>("v_photoid");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return photoId;
        }

        public bool Delete(CarPhotos entity)
        {
            bool isDeleted = false;
            var param = new DynamicParameters();
            param.Add("v_stockid", entity.StockId);
            param.Add("v_isdealer", entity.SellerType == 1);
            param.Add("v_sourceid", entity.SourceId);
            param.Add("v_tc_carphotoid", entity.TC_CarPhotoId);
            param.Add("v_deleted", 0, DbType.Int32, direction: ParameterDirection.Output);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("carphotos_deletestockimagedetails_v18_2_2", param, commandType: CommandType.StoredProcedure);
                isDeleted = param.Get<int>("v_deleted") == 1;
            }
            return isDeleted;
        }

        public List<CarPhotos> GetProcessedImage(string imageIdList)
        {
            List<CarPhotos> carphotos = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_imagelist", imageIdList, DbType.String);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    carphotos = con.Query<CarPhotos>("GetClassifiedProcessedImages", parameters, commandType: CommandType.StoredProcedure).AsList<CarPhotos>();
                    LogLiveSps.LogSpInGrayLog("GetClassifiedProcessedImages");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return carphotos;
        }

        public bool UpdateStatus(StockImageStatus statusDetail, int stockId, int? sellerType)
        {
            bool isUpdated = false;
            try
            {
                if (statusDetail != null)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("v_id", statusDetail.Id);
                    parameters.Add("v_originalimgpath", statusDetail.OriginalImagePath);
                    parameters.Add("v_isapproved", statusDetail.Action == ActionType.Approve ? (int?)1 : null);
                    parameters.Add("v_isactive", statusDetail.Action == ActionType.Delete ? (int?)0 : null);
                    parameters.Add("v_sellertype", sellerType);
                    parameters.Add("v_inquiryid", stockId);
                    parameters.Add("v_isupdated", 0 , DbType.Int16,direction: ParameterDirection.Output);
                    using (var con = ClassifiedMySqlMasterConnection)
                    {
                        con.Execute("updatecarphotos_patch_v1", parameters, commandType: CommandType.StoredProcedure);
                    }
                    isUpdated = parameters.Get<short>("v_isupdated") > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isUpdated;
        }

        public bool setMainImage(int inquiryid, int sellerType)
        {
            bool isSet = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryid", inquiryid);
                parameters.Add("v_isdealer", sellerType == 1 ? 1 : 0);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    isSet = con.Execute("setmainimage", parameters, commandType: CommandType.StoredProcedure) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isSet;
        }
    }
}
