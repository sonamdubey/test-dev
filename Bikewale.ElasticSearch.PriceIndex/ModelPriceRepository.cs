using Bikewale.ElasticSearch.Entities;
using Bikewale.Utility;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bikewale.ElasticSearch.PriceIndex
{
    class ModelPriceRepository
    {
        /// <summary>
        /// Created By : Deepak Israni on 19 Feb 2018
        /// Description: Function reads the data from the DB and maps it to ModelPriceDocument entity.
        /// </summary>
        /// <returns></returns>
        public static List<ModelPriceDocument> GetData()
        {
            List<ModelPriceDocument> objList = null;
            ModelPriceDocument docObj = null;
            VersionEntity verObj = null;
            
            uint LastModelId = 0;
            uint LastCityId = 0;
            uint CurrentModelId = 0;
            uint CurrentCityId = 0;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("createesmodelpriceindex"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<ModelPriceDocument>();
                            List<VersionEntity> versions = null;
                            

                            while (dr.Read())
                            {
                                CurrentModelId = SqlReaderConvertor.ToUInt32(dr["BikeModelId"]);
                                CurrentCityId = SqlReaderConvertor.ToUInt32(dr["CityId"]);
                                
                                if (CurrentModelId != LastModelId && CurrentCityId != LastCityId)
                                {
                                    if (docObj != null)
                                    {
                                        docObj.VersionPrice = versions;
                                        objList.Add(docObj);
                                    }

                                    docObj = InitializeDocumentObj(CurrentModelId, CurrentCityId);
                                    versions = new List<VersionEntity>();

                                    LastModelId = CurrentModelId;
                                    LastCityId = CurrentCityId;

                                    docObj.ModelName = Convert.ToString(dr["ModelName"]);
                                    docObj.ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                    docObj.MakeId = SqlReaderConvertor.ToUInt32(dr["BikeMakeId"]);
                                    docObj.MakeName = Convert.ToString(dr["MakeName"]);
                                    docObj.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                    docObj.CityName = Convert.ToString(dr["CityName"]);
                                    docObj.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);

                                }

                                verObj = InitializeVersionEntity(SqlReaderConvertor.ToUInt32(dr["VersionId"]), Convert.ToString(dr["VersionName"]));
                                verObj.PriceList = SetVersionPrice(SqlReaderConvertor.ToUInt32(dr["Price"]), SqlReaderConvertor.ToUInt32(dr["RTO"]), SqlReaderConvertor.ToUInt32(dr["Insurance"]));
                                
                                versions.Add(verObj);
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name, ex);
                Console.WriteLine("Exception Message  : " + ex.Message);
            }

            return objList;
        }

        /// <summary>
        /// Created By : Deepak Israni on 19 Feb 2018
        /// Description: Initializes the ModelPriceDocument object and creates the ID for the same.
        /// </summary>
        /// <returns></returns>
        private static ModelPriceDocument InitializeDocumentObj(uint modelId, uint cityId)
        {
            ModelPriceDocument docObj = new ModelPriceDocument();
            
            docObj.Id = modelId + "_" + cityId;
            docObj.ModelId = modelId;
            docObj.CityId = cityId;

            return docObj;
        }

        /// <summary>
        /// Created By : Deepak Israni on 19 Feb 2018
        /// Description: Initializes the VersionEntity which contains the pricing information.
        /// </summary>
        /// <returns></returns>
        private static VersionEntity InitializeVersionEntity(uint versionId, string versionName)
        {
            VersionEntity verObj = new VersionEntity();

            verObj.VersionId = versionId;
            verObj.VersionName = versionName;

            return verObj;
        }

        /// <summary>
        /// Created By : Deepak Israni on 19 Feb 2018
        /// Description: Sets the ExShowroom, RTO and Insurance prices in a list.
        /// </summary>
        /// <returns></returns>
        private static List<PriceEntity> SetVersionPrice(uint exShowroom, uint rto, uint insurance)
        {
            List<PriceEntity> prices = new List<PriceEntity>();
            
            prices.Add(new PriceEntity()
            {
                PriceType = "Ex-Showroom",
                PriceValue = exShowroom
            });

            prices.Add(new PriceEntity()
            {
                PriceType = "RTO",
                PriceValue = rto
            });
            prices.Add(new PriceEntity()
            {
                PriceType = "Insurance",
                PriceValue = insurance
            });

            return prices;
        }
    }
}
