
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.BikeColorImages;
using BikewaleOpr.Interface.BikeColorImages;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
namespace BikewaleOpr.DALs.BikeColorImages
{
    /// <summary>
    /// Created By :- Subodh Jain 09 jan 2017
    /// Summary :- Bikes Images Details 
    /// </summary>
    public class ColorImagesBikeRepository : IColorImagesBikeRepository
    {
        /// <summary>
        /// Created By :- Subodh Jain 9 Jan 2017
        /// Summary :- Fetch Photo Id 
        /// </summary>
        /// <param name="objBikeColorDetails"></param>
        /// <returns></returns>
        public uint FetchPhotoId(ColorImagesBikeEntities objBikeColorDetails)
        {
            uint photoId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("savebikemodelcolorphoto_02032017"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelId", DbType.Int32, objBikeColorDetails.Modelid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelColorId", DbType.Int32, objBikeColorDetails.ModelColorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, objBikeColorDetails.UserId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {

                            while (dr.Read())
                            {
                                photoId = SqlReaderConvertor.ToUInt32(dr["Id"]);
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ColorImagesBikeRepository.SaveBikeColorDetails");
            }

            return photoId;
        }
        /// <summary>
        /// Created By :- Subodh Jain 09 jan 2017
        /// Summary :- To delete Bike Color details 
        /// </summary>
        /// <param name="photoid"></param>
        /// <returns></returns>
        public bool DeleteBikeColorDetails(uint photoId)
        {
            bool isDeleted = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deletebikemodelcolorphoto_02032017"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoid", DbType.Int32, photoId));
                    Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase));
                    isDeleted = true;

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ColorImagesBikeRepository.DeleteBikeColorDetails");
            }

            return isDeleted;
        }
    }
}
