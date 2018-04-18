using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.ElasticSearch.DocumentBuilderConsumer.Interfaces;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Utility;
using Consumer;
using log4net;
using MySql.CoreDAL;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using VehicleData.Service.ProtoClass;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;

namespace Bikewale.ElasticSearch.DocumentBuilderConsumer.DocumentBuilders
{
    public class ModelIndexDocumentBuilder : IDocumentBuilder
    {
        private IApiGatewayCaller _apiGatewayCaller;
        IUnityContainer container = null;
        private static IEnumerable<EnumSpecsFeaturesItems> requiredSpec = new List<EnumSpecsFeaturesItems>{
                                        EnumSpecsFeaturesItems.Displacement,
                                        EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                                        EnumSpecsFeaturesItems.MaxPowerBhp,
                                        EnumSpecsFeaturesItems.KerbWeight,
                                        EnumSpecsFeaturesItems.RearBrakeType,
                                        EnumSpecsFeaturesItems.WheelType,
                                        EnumSpecsFeaturesItems.AntilockBrakingSystem,
                                        EnumSpecsFeaturesItems.StartType
                                    };


        public ModelIndexDocumentBuilder()
        {
            using (container = new UnityContainer())
            {
                container.RegisterType<IApiGatewayCaller, ApiGatewayCaller>();
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: Function to get newly created documents and push them to BWEsIndexUpdater's queue for insertion of new documents in ES Index.
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public bool InsertDocuments(NameValueCollection nvc)
        {
            NameValueCollection inputs = new NameValueCollection();
            inputs["modelids"] = nvc["ids"];
            IEnumerable<BikeModelDocument> indexDocs = GetDocuments(inputs);

            if (indexDocs != null)
            {
                foreach (BikeModelDocument doc in indexDocs)
                {
                    NameValueCollection packet = new NameValueCollection();
                    packet["indexName"] = nvc["indexName"];
                    packet["documentType"] = nvc["documentType"];
                    packet["documentId"] = doc.Id;
                    packet["operationType"] = nvc["operationType"];
                    packet["documentJson"] = JsonConvert.SerializeObject(doc);

                    PushToQueue(packet);

                    Logs.WriteInfoLog("RabbitMQExecution :Pushed job : " + packet["indexName"] + ", Document Type: " + packet["documentType"] + ", Operation Type: " + packet["operationType"] + ", Document ID: " + packet["documentId"] + ", Document: " + packet["documentJson"]);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: Function to get recently updated documents and push them to BWEsIndexUpdater's queue for updation of entries in ES Index.
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public bool UpdateDocuments(NameValueCollection nvc)
        {
            NameValueCollection inputs = new NameValueCollection();
            inputs["modelids"] = nvc["ids"];
            IEnumerable<BikeModelDocument> indexDocs = GetDocuments(inputs);

            if (indexDocs != null)
            {
                foreach (BikeModelDocument doc in indexDocs)
                {
                    NameValueCollection packet = new NameValueCollection();
                    packet["indexName"] = nvc["indexName"];
                    packet["documentType"] = nvc["documentType"];
                    packet["documentId"] = doc.Id;
                    packet["operationType"] = nvc["operationType"];
                    packet["documentJson"] = JsonConvert.SerializeObject(doc);

                    PushToQueue(packet);

                    Logs.WriteInfoLog("RabbitMQExecution :Pushed job : " + packet["indexName"] + ", Document Type: " + packet["documentType"] + ", Operation Type " + packet["operationType"] + ", Document ID: " + packet["documentId"] + ", Document: " + packet["documentJson"]);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: Function to push the model ids of recently deleted models to BWEsIndexUpdater's queue for deletion in the ES Index.
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public bool DeleteDocuments(NameValueCollection nvc)
        {
            string[] models = nvc["ids"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (String model in models)
            {
                NameValueCollection packet = new NameValueCollection();
                packet["indexName"] = nvc["indexName"];
                packet["documentType"] = nvc["documentType"];
                packet["documentId"] = model;
                packet["operationType"] = nvc["operationType"];

                PushToQueue(packet);

                Logs.WriteInfoLog("RabbitMQExecution :Pushed job : " + packet["indexName"] + ", Document Type: " + packet["documentType"] + ", Operation Type: " + packet["operationType"] + ", Document ID: " + packet["documentId"]);
            }
            return true;
        }


        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: To create index documents for specific modelids.
        /// Modified by : Kartik Rathod on 12 apr 2018 fetched minspecs from microservice
        /// </summary>
        /// <param name="inputParameters"></param>
        /// <returns></returns>
        private IEnumerable<BikeModelDocument> GetDocuments(NameValueCollection inputParameters)
        {
            string[] _pricestypes = { "RTO", "Insurance", "Exshowroom" };
            String spName = "getnewmodelforbikeindex";

            IList<BikeModelDocument> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(spName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelids", DbType.String, inputParameters["modelids"]));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<BikeModelDocument>();
                            uint weight_count = 1;
                            while (dr.Read())
                            {
                                IList<PriceEntity> objPrices = new List<PriceEntity>();

                                //price components
                                for (int i = 0; i < _pricestypes.Length; i++)
                                {
                                    objPrices.Add(new PriceEntity()
                                    {
                                        PriceType = _pricestypes[i],
                                        PriceValue = SqlReaderConvertor.ToUInt32(dr[_pricestypes[i]])
                                    });
                                }

                                objList.Add(new BikeModelDocument()
                                    {
                                        //doc id and Weight
                                        Id = Convert.ToString(dr["ModelId"]),
                                        Weight = weight_count++,

                                        //make details
                                        BikeMake = new MakeEntity()
                                        {
                                            MakeId = SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                            MakeName = Convert.ToString(dr["MakeName"]),
                                            MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                            MakeStatus = GetStatus(SqlReaderConvertor.ToBoolean(dr["IsNewMake"]), Convert.ToBoolean(dr["IsFuturisticMake"]))
                                        },

                                        //model details
                                        BikeModel = new ModelEntity()
                                        {
                                            ModelId = SqlReaderConvertor.ToUInt32(dr["ModelId"]),
                                            ModelName = Convert.ToString(dr["ModelName"]),
                                            ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                            ModelStatus = GetStatus(SqlReaderConvertor.ToBoolean(dr["IsNewModel"]), Convert.ToBoolean(dr["IsFuturisticModel"]))
                                        },

                                        //top version
                                        TopVersion = new VersionEntity()
                                        {
                                            VersionId = SqlReaderConvertor.ToUInt32(dr["TopVersionId"]),
                                            VersionName = Convert.ToString(dr["VersionName"]),
                                            PriceList = objPrices,
                                            Exshowroom = SqlReaderConvertor.ToUInt32(dr["Exshowroom"]),
                                            Onroad = SqlReaderConvertor.ToUInt32(dr["RTO"]) + SqlReaderConvertor.ToUInt32(dr["Insurance"]) + SqlReaderConvertor.ToUInt32(dr["Exshowroom"]),
                                            VersionStatus = GetStatus(SqlReaderConvertor.ToBoolean(dr["IsNewVersion"]), SqlReaderConvertor.ToBoolean(dr["IsFuturisticVersion"]))
                                        },

                                        BikeName = Convert.ToString(dr["MakeName"]) + " " + Convert.ToString(dr["ModelName"]),
                                        BodyStyleId = SqlReaderConvertor.ToUInt32(dr["BodyStyleId"]),

                                        //bike media/reviews
                                        BikeImage = new ImageEntity()
                                        {
                                            ImageURL = Convert.ToString(dr["ImageURL"]),
                                            HostURL = Convert.ToString(dr["HostURL"])
                                        },
                                        ImageCount = SqlReaderConvertor.ToUInt32(dr["ImageCount"]),
                                        VideosCount = SqlReaderConvertor.ToUInt32(dr["VideosCount"]),
                                        ExpertReviewsCount = SqlReaderConvertor.ToUInt32(dr["ExpertReviewsCount"]),
                                        UserReviewsCount = SqlReaderConvertor.ToUInt32(dr["UserReviewsCount"]),
                                        ReviewRatings = SqlReaderConvertor.ParseToDouble(dr["ReviewRatings"]),
                                        RatingsCount = SqlReaderConvertor.ToUInt32(dr["RatingsCount"]),

                                    });
                            }
                            dr.Close();
                        }
                    }
                }

                
                objList = GetMinSpecs(objList); // get min specs for version

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception at Bikewale.ElasticSearch.DocumentBuilderConsumer.DocumentBuilders: GetDocuments() ", ex);
            }
            return objList;
        }

        /// <summary>
        /// Created By : Deepak Israni on 8 March 2018
        /// Description: To get the status of make/model/version depending on they are new or futuristic.
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="isFuturistic"></param>
        /// <returns></returns>
        private static BikeStatus GetStatus(bool isNew, bool isFuturistic)
        {
            return !isNew ? (!isFuturistic ? BikeStatus.Discontinued : BikeStatus.Upcoming) : (!isFuturistic ? BikeStatus.New : BikeStatus.Invalid);
        }

        /// <summary>
        /// Created By : Deepak Israni on 9 March 2018
        /// Description: Function to push to BWEsIndexUpdater queue.
        /// </summary>
        /// <param name="nvc"></param>
        private static void PushToQueue(NameValueCollection nvc)
        {
            RabbitMqPublish publish = new RabbitMqPublish();
            publish.PublishToQueue(ConfigurationManager.AppSettings["BWEsIndexUpdaterQueue"], nvc);
        }

        /// <summary>
        /// Author  : Kartik Rathod on 11 Apr 2018 
        /// Desc    : fetch minspecs for topversion from microservice
        /// </summary>
        /// <param name="objList">BikeModelDocument document </param>
        /// <returns> IList of BikeModelDocument document </returns>
        private IList<BikeModelDocument> GetMinSpecs(IList<BikeModelDocument> objList)
        {
            try
            {
                if (objList != null && objList.Count > 0)
                {
                    _apiGatewayCaller = container.Resolve<IApiGatewayCaller>();

                    IEnumerable<int> versionIds = objList.Select(r => Convert.ToInt32(r.TopVersion.VersionId));

                    VersionsDataByItemIds_Input input = new VersionsDataByItemIds_Input()
                    {
                        Versions = versionIds,
                        Items = requiredSpec

                    };

                    GetVersionSpecsByItemIdAdapter adapter = new GetVersionSpecsByItemIdAdapter();

                    adapter.AddApiGatewayCall(_apiGatewayCaller, input);

                    _apiGatewayCaller.Call();

                    IEnumerable<VersionMinSpecsEntity> minSpec = adapter.Output;

                    if (minSpec != null)
                    {
                        var objEnumerator = objList.GetEnumerator();
                        var versionEnumerator = minSpec.GetEnumerator();

                        while (objEnumerator.MoveNext() && versionEnumerator.MoveNext())
                        {
                            objEnumerator.Current.TopVersion.Power = Convert.ToDouble(GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.MaxPowerBhp));
                            objEnumerator.Current.TopVersion.Mileage = Convert.ToUInt16(GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.FuelEfficiencyOverall));
                            objEnumerator.Current.TopVersion.KerbWeight = Convert.ToUInt16(GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.KerbWeight));
                            objEnumerator.Current.TopVersion.Displacement = Convert.ToDouble(GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.Displacement));

                            objEnumerator.Current.TopVersion.ABS = GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.AntilockBrakingSystem) == "1";
                            objEnumerator.Current.TopVersion.RearBrakeType = Convert.ToInt16(GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.RearBrakeType));
                            objEnumerator.Current.TopVersion.Wheels = Convert.ToInt16(GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.WheelType));
                            objEnumerator.Current.TopVersion.StartType = Convert.ToInt16(GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.StartType));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception at Bikewale.ElasticSearch.DocumentBuilderConsumer.DocumentBuilders: GetMinSpecs() ", ex);
            }
            return objList;
        }

        /// <summary>
        /// Author  : Kartik Rathod on 11 Apr 2018 
        ///  Desc    : gets value of passed specsId
        /// </summary>
        /// <param name="objSpec">list of SpecsItem</param>
        /// <param name="propertyId">specsId for which value is needed</param>
        /// <returns></returns>
        private string GetSpecsValue(IEnumerable<SpecsItem> objSpec, int propertyId)
        {
            try
            {
                string value = string.Empty;
                if (objSpec != null)
                {
                    value = (from data in objSpec
                             where data.Id == propertyId
                             select (data.DataType == EnumSpecDataType.Custom) ? Convert.ToString(data.CustomTypeId) : data.Value).FirstOrDefault();
                }
                return !string.IsNullOrEmpty(value) ? value : null;
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception at Bikewale.ElasticSearch.DocumentBuilderConsumer.DocumentBuilders: GetSpecsValue() ", ex);
                return null;
            }
        }

    }
}
