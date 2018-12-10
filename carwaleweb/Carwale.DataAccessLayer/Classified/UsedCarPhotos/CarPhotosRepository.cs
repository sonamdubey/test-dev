using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Classified;
using Carwale.Notifications;
using Dapper;
using System.Data;
using Carwale.Interfaces.Classified.UsedCarPhotos;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.Classified.UsedCarPhotos
{
    public class CarPhotosRepository : RepositoryBase, ICarPhotosRepository
    {
        public List<CarPhoto> GetCarPhotos(int inquiryId, bool isDealer)
        {
            List<CarPhoto> carPhotos = null;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryId", inquiryId, DbType.Int32);
                parameters.Add("v_isDealer", isDealer, DbType.Boolean);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    carPhotos = con.Query<CarPhoto>("GetCarPhotosByInquiryId", parameters, commandType: CommandType.StoredProcedure).AsList<CarPhoto>();
                    LogLiveSps.LogSpInGrayLog("GetCarPhotosByInquiryId");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "CarPhotosRepository.GetCarPhotos");
                objErr.SendMail();
            }
            return carPhotos;
        }

        public int SaveCarPhotos(CarPhoto carPhoto)
        {
            int photoId = -1;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryid", carPhoto.InquiryId, DbType.Int32);
                parameters.Add("v_filename", carPhoto.FileName, DbType.String);
                parameters.Add("v_imageurlfull", carPhoto.ImageUrlFull, DbType.String);
                parameters.Add("v_imageurlthumb", carPhoto.ImageUrlThumb, DbType.String);
                parameters.Add("v_imageurlthumbsmall", carPhoto.ImageUrlThumbSmall, DbType.String);
                parameters.Add("v_description", carPhoto.Description, DbType.String);
                parameters.Add("v_isdealer", carPhoto.IsDealer, DbType.Boolean);
                parameters.Add("v_ismain", carPhoto.IsMain, DbType.Boolean);
                parameters.Add("v_directorypath", carPhoto.DirectoryPath, DbType.String);
                parameters.Add("v_hosturl", carPhoto.HostUrl, DbType.String);
                parameters.Add("v_isreplicated", carPhoto.IsReplicated, DbType.String);
                parameters.Add("v_photoid", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("classified_carphotos_savephotodetails_v15_8_1", parameters, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("classified_carphotos_savephotodetails_v15_8_1");
                }
                photoId = parameters.Get<int>("v_photoid");
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "CarPhotosRepository.SaveCarPhotos");
                objErr.SendMail();
            }
            return photoId;
        }

        public bool UpdateMainImage(int inquiryId, int photoId, bool isDealer)
        {
            bool ret = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryid", inquiryId, DbType.Int32);
                parameters.Add("v_photoid", photoId, DbType.Int32);
                parameters.Add("v_isdealer", isDealer ? 1 : 0, DbType.Int32);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    ret = con.Execute("UpdateCarPhotosMainImage", parameters, commandType: CommandType.StoredProcedure) > 0;
                    LogLiveSps.LogSpInGrayLog("UpdateCarPhotosMainImage");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "CarPhotosRepository.UpdateMainImage");
                objErr.SendMail();
            }
            return ret;
        }

        public bool RemoveCarPhoto(int inquiryId, int photoId)
        {
            bool ret = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("v_inquiryid", inquiryId, DbType.Int32);
                parameters.Add("v_photoid", photoId, DbType.Int32);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    ret = con.Execute("RemoveCarPhotos", parameters, commandType: CommandType.StoredProcedure) > 0;
                    LogLiveSps.LogSpInGrayLog("RemoveCarPhotos");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "CarPhotosRepository.RemoveCarPhoto");
                objErr.SendMail();
            }
            return ret;
        }
    }
}
