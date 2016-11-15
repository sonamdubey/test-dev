using Bikewale.Entities.Images;
using Bikewale.Interfaces.Images;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.Images
{
    public class ImageRepository<T, U> : IImageRepository<T, U> where T : Image, new()
    {
        public bool Update(T t)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<T> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public T GetById(U id)
        {
            throw new System.NotImplementedException();
        }

        public ulong Add(T t)
        {
            ulong id = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "img_savephotos";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_categoryId", DbType.Int32, t.CategoryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_itemId", DbType.Int32, t.ItemId.HasValue ? t.ItemId.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hostUrl", DbType.String, t.HostUrl));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalPath", DbType.String, t.OriginalPath));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isProcessed", DbType.Int16, (t.IsReplicated.HasValue ? (t.IsReplicated.Value ? 1 : 0) : 0)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_aspectRatio", DbType.Decimal, t.AspectRatio));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isWaterMark", DbType.Int16, (t.IsWaterMark.HasValue ? (t.IsWaterMark.Value ? 1 : 0) : 0)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isMain", DbType.Int16, (t.IsMain.HasValue ? (t.IsMain.Value ? 1 : 0) : 0)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isMaster", DbType.Int16, (t.IsMaster.HasValue ? (t.IsMaster.Value ? 1 : 0) : 0)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int64, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    id = Utility.SqlReaderConvertor.ToUInt64(cmd.Parameters["par_id"].Value);
                    t.Id = id;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("ImageRepository.Add({0})", Newtonsoft.Json.JsonConvert.SerializeObject(t)));
                objErr.SendMail();
            }
            return id;
        }

        U Interfaces.IRepository<T, U>.Add(T t)
        {
            throw new System.NotImplementedException();
        }
    }
}
