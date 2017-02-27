using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DAL.Videos
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Feb 2017
    /// Summary    : DAL for videos
    /// </summary>
    public class ModelVideoRepository :IVideoRepository
    {
       public ICollection<BikeVideoModelEntity> GetModelVideos(uint makeId)
        {
            ICollection<BikeVideoModelEntity> modelVideoList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getvideomodels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            modelVideoList = new Collection<BikeVideoModelEntity>();

                            while (dr.Read())
                            {

                                BikeVideoModelEntity objModelVideo = new BikeVideoModelEntity();
                                objModelVideo.objMake = new BikeMakeEntityBase();
                                objModelVideo.objMake.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                objModelVideo.objMake.MakeName = Convert.ToString(dr["MakeName"]);
                                objModelVideo.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objModelVideo.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                objModelVideo.ModelName = Convert.ToString(dr["ModelName"]);
                                objModelVideo.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objModelVideo.VideoCount = SqlReaderConvertor.ToInt32(dr["VideosCount"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Bikewale.DAL.Videos.GetModelVideos MakeId:{0}", makeId));
            }
            return modelVideoList;
        }
    }
}
