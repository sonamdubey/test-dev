using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 16 Feb 2017
    /// Summary : Class will have functions related to bikemodels
    /// </summary>
    public class ModelsRepository : IModelsRepository
    {
        /// <summary>
        /// Function to get the all upcoming models
        /// modified By :- Subodh Jain 09 March 2017
        /// Summary :- added body style
        /// </summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UpcomingBikeEntity> GetUpcomingModels()
        {
            IList<UpcomingBikeEntity> objModelList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getupcomingbikeslist_new_11042017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;



                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objModelList = new List<UpcomingBikeEntity>();

                            while (dr.Read())
                            {
                                UpcomingBikeEntity objModel = new UpcomingBikeEntity();


                                objModel.ExpectedLaunchDate = Convert.ToString(dr["LaunchDate"]);
                                objModel.EstimatedPriceMin = Convert.ToUInt64(dr["EstimatedPriceMin"]);
                                objModel.EstimatedPriceMax = Convert.ToUInt64(dr["EstimatedPriceMax"]);
                                objModel.HostUrl = Convert.ToString(dr["HostURL"]);
                                objModel.LargePicImagePath = Convert.ToString(dr["LargePicImagePath"]);
                                objModel.MakeBase.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                                objModel.ModelBase.ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]);
                                objModel.ExpectedLaunchedDate = SqlReaderConvertor.ToDateTime(dr["LaunchDate"]);

                                objModel.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                                objModel.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);

                                objModel.ModelBase.ModelName = Convert.ToString(dr["ModelName"]);
                                objModel.ModelBase.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objModel.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objModel.BodyStyleId = SqlReaderConvertor.ToUInt32(dr["BodyStyleId"]);
                                objModel.BikeName = string.Format("{0} {1}", objModel.MakeBase.MakeName, objModel.ModelBase.ModelName);
                                objModelList.Add(objModel);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ModelsRepository.GetUpcomingModels");
            }

            return objModelList;
        }
    }
}
