using Bikewale.Notifications;
using BikewaleOpr.Entities.Images;
using BikewaleOpr.Interface.Images;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.DALs.Images
{
    public class ImageRepository : IImageRepository
    {
        public ulong Add(Image image)
        {
            ulong id = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "img_savephotos";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categoryId", DbType.Int32, image.CategoryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_itemId", DbType.Int32, image.ItemId.HasValue ? image.ItemId.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hostUrl", DbType.String, image.HostUrl));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalPath", DbType.String, image.OriginalPath));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isProcessed", DbType.Int16, (image.IsReplicated.HasValue ? (image.IsReplicated.Value ? 1 : 0) : 0)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_aspectRatio", DbType.Decimal, image.AspectRatio));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isWaterMark", DbType.Int16, (image.IsWaterMark.HasValue ? (image.IsWaterMark.Value ? 1 : 0) : 0)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isMain", DbType.Int16, (image.IsMain.HasValue ? (image.IsMain.Value ? 1 : 0) : 0)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isMaster", DbType.Int16, (image.IsMaster.HasValue ? (image.IsMaster.Value ? 1 : 0) : 0)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int64, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    id = Bikewale.Utility.SqlReaderConvertor.ToUInt64(cmd.Parameters["par_id"].Value);
                    image.Id = id;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ImageRepository.Add()");
            }
            return id;
        }
    }
}
