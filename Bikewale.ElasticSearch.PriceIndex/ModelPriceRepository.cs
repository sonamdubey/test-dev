﻿using Bikewale.ElasticSearch.Entities;
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
            
            uint _lastModelId = 0;
            uint _lastCityId = 0;
            uint _currentModelId = 0;
            uint _currentCityId = 0;

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
                                _currentModelId = SqlReaderConvertor.ToUInt32(dr["BikeModelId"]);
                                _currentCityId = SqlReaderConvertor.ToUInt32(dr["CityId"]);
                                
                                if (_currentModelId != _lastModelId || _currentCityId != _lastCityId)
                                {
                                    if (docObj != null)
                                    {
                                        docObj.VersionPrice = versions;
                                        objList.Add(docObj);
                                    }

                                    docObj = InitializeDocumentObj(_currentModelId, _currentCityId);
                                    versions = new List<VersionEntity>();

                                    _lastModelId = _currentModelId;
                                    _lastCityId = _currentCityId;

                                    docObj.BikeModel = new ModelEntity()
                                    {
                                        ModelId = _currentModelId,
                                        ModelName = Convert.ToString(dr["ModelName"]),
                                        ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                        ModelStatus = GetStatus(Convert.ToBoolean(dr["IsNewModel"]), Convert.ToBoolean(dr["IsFuturisticModel"]))
                                    };

                                    docObj.BikeMake = new MakeEntity()
                                    {
                                        MakeId = SqlReaderConvertor.ToUInt32(dr["BikeMakeId"]),
                                        MakeName = Convert.ToString(dr["MakeName"]),
                                        MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                        MakeStatus = GetStatus(Convert.ToBoolean(dr["IsNewMake"]), Convert.ToBoolean(dr["IsFuturisticMake"]))
                                    };

                                    docObj.City = new CityEntity()
                                    {
                                        CityId = _currentCityId,
                                        CityName = Convert.ToString(dr["CityName"]),
                                        CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                                    };

                                }

                                verObj = InitializeVersionEntity(SqlReaderConvertor.ToUInt32(dr["VersionId"]), Convert.ToString(dr["VersionName"]));
                                verObj.Exshowroom = SqlReaderConvertor.ToUInt32(dr["Price"]);
                                verObj.PriceList = SetVersionPrice(verObj.Exshowroom, SqlReaderConvertor.ToUInt32(dr["RTO"]), SqlReaderConvertor.ToUInt32(dr["Insurance"]));
                                verObj.Onroad = (uint) verObj.PriceList.Sum(prc => prc.PriceValue);
                                verObj.VersionStatus = GetStatus(Convert.ToBoolean(dr["IsNewVersion"]), Convert.ToBoolean(dr["IsFuturisticVersion"]));

                                versions.Add(verObj);
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("ModelPriceRepository: ", ex);
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
        private static IEnumerable<PriceEntity> SetVersionPrice(uint exShowroom, uint rto, uint insurance)
        {
            IList<PriceEntity> prices = new List<PriceEntity>();
            
            prices.Add(new PriceEntity()
            {
                PriceType = "Exshowroom",
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

        /// <summary>
        /// Created By : Deepak Israni on 22 Feb 2018
        /// Description: To get the status of make/model/version depending on they are new or futuristic.
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="isFuturistic"></param>
        /// <returns></returns>
        private static BikeStatus GetStatus(bool isNew, bool isFuturistic)
        {
            return !isNew ? (!isFuturistic ? BikeStatus.Discontinued : BikeStatus.Upcoming) : (!isFuturistic ? BikeStatus.New : BikeStatus.Invalid);
        }
    }
}
