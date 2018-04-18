using Bikewale.ElasticSearch.Entities;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Consumer;
using Bikewale.Utility;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.Entities.BikeData;
using Microsoft.Practices.Unity;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;


namespace BikeIndex
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: DAL Layer
    /// </summary>
    public class BikeModelRepository
    {
        
        private string[] _pricestypes = { "RTO", "Insurance", "Exshowroom" };
        private IApiGatewayCaller _apiGatewayCaller;
        IUnityContainer container = null;

        public BikeModelRepository()
        {
            using (container = new UnityContainer())
            {
                container.RegisterType<IApiGatewayCaller, ApiGatewayCaller>();
            }
        }

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 20th Feb 2018
        /// Description: Assigns status according to the new and futuristic flags associated with it
        /// Modified by: Dhruv Joshi
        /// Dated: 21st Feb 2018
        /// Description: Storing data in the specs individually instead of an entity, also brought out exshowroom and onroad in version entity
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="isFuturistic"></param>
        /// <returns></returns>
        private BikeStatus GetStatus(bool isNew, bool isFuturistic)
        {
            if(!isNew)
            {
                if(!isFuturistic)
                {
                    return BikeStatus.Discontinued;
                }
                else 
                {
                    return BikeStatus.Upcoming;
                }
            }
            else
            {
                if(!isFuturistic)
                {
                    return BikeStatus.New;
                }
                else
                {
                    return BikeStatus.Invalid;
                }
            }
            
        }

        /// <summary>
        /// Created by: Dhruv Joshi
        /// Dated: 20th Feb 2018
        /// Description: Fetching data for bikeindex from db
        /// Modified by : Kartik Rathod on 12 apr 2018 fetched minspecs from microservice
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeModelDocument> GetBikeModelList()
        {
        
            IList<BikeModelDocument> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdataforbikeindex"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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
                                            MakeStatus = GetStatus(SqlReaderConvertor.ToBoolean(dr["IsNewMake"]), Bikewale.Utility.SqlReaderConvertor.ToBoolean(dr["IsFuturisticMake"]))
                                        },

                                        //model details
                                        BikeModel = new ModelEntity()
                                        {
                                            ModelId = SqlReaderConvertor.ToUInt32(dr["ModelId"]),
                                            ModelName = Convert.ToString(dr["ModelName"]),
                                            ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                            ModelStatus = GetStatus(SqlReaderConvertor.ToBoolean(dr["IsNewModel"]), SqlReaderConvertor.ToBoolean(dr["IsFuturisticModel"]))
                                        },

                                        //top version
                                        TopVersion = new VersionEntity()
                                        {
                                            VersionId = SqlReaderConvertor.ToUInt32(dr["TopVersionId"]),
                                            VersionName = Convert.ToString(dr["VersionName"]),
                                            PriceList = objPrices,
                                            Exshowroom = SqlReaderConvertor.ToUInt32(dr["Exshowroom"]),
                                            Onroad = SqlReaderConvertor.ToUInt32(dr["RTO"]) + Bikewale.Utility.SqlReaderConvertor.ToUInt32(dr["Insurance"]) + Bikewale.Utility.SqlReaderConvertor.ToUInt32(dr["Exshowroom"]),
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
                Logs.WriteErrorLog("Exception at Bikewale.ElasticSearch.BikeIndex.BikeModelRepository ->GetBikeList() ", ex);
                Console.WriteLine("Exception Message  : " + ex.Message);
            }
            return objList;
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
                        Items = new List<EnumSpecsFeaturesItems>{
                                        EnumSpecsFeaturesItems.Displacement,
                                        EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                                        EnumSpecsFeaturesItems.MaxPowerBhp,
                                        EnumSpecsFeaturesItems.KerbWeight,
                                        EnumSpecsFeaturesItems.RearBrakeType,
                                        EnumSpecsFeaturesItems.WheelType,
                                        EnumSpecsFeaturesItems.AntilockBrakingSystem,
                                        EnumSpecsFeaturesItems.StartType
                                    }
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
                            objEnumerator.Current.TopVersion.BrakeType = GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.RearBrakeType);
                            objEnumerator.Current.TopVersion.Wheels = GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.WheelType);
                            objEnumerator.Current.TopVersion.StartType = GetSpecsValue(versionEnumerator.Current.MinSpecsList, (int)EnumSpecsFeaturesItems.StartType);
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
                value = objSpec.Where(d => d.Id == propertyId).Select(k => k.Value).FirstOrDefault();
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
