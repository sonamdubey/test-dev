using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.DealerLocator;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.DAL.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 22 march 2016
    /// Description : for Dealer Locator Functionality.
    /// </summary>
    public class DealerRepository : Bikewale.Interfaces.DealerLocator.IDealer
    {
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 22 march 2016
        /// Description : for getting dealer detail and bike detail w.r.t dealer.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>   
        public DealerBikesEntity GetDealerBikes(UInt16 dealerId)
        {
            DealerBikesEntity dealers = new DealerBikesEntity();
            
            //IList<DealersList> dealerList = new List<DealersList>();
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerBikeDetails";
                    cmd.Parameters.Add("@DealerId", SqlDbType.Int).Value = dealerId;
                    
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            if (dr.Read() && dr.HasRows)
                            {
                                DealerPackageTypes dpType;
                                dealers.DealerDetail = new DealerDetailEntity();
                                dealers.DealerDetail.Name = Convert.ToString(dr["DealerName"]);
                                dealers.DealerDetail.Address = Convert.ToString(dr["Address"]);
                                dealers.DealerDetail.Area = new AreaEntityBase
                                        {
                                            AreaName = Convert.ToString(dr["Area"]),
                                            Longitude = Convert.ToDouble(dr["Longitude"]),
                                            Latitude = Convert.ToDouble(dr["Lattitude"])
                                        };
                                dealers.DealerDetail.City = Convert.ToString(dr["City"]);
                                dealers.DealerDetail.DealerPkgType = Enum.TryParse<DealerPackageTypes>(Convert.ToString(dr["DealerType"]), out dpType) ? dpType : DealerPackageTypes.Invalid;
                                dealers.DealerDetail.EMail = Convert.ToString(dr["EMail"]);
                                dealers.DealerDetail.MaskingNumber = Convert.ToString(dr["MaskingNumber"]);
                            }
                            if (dr.NextResult())
                            {
                                IList<MostPopularBikesBase> models = new List<MostPopularBikesBase>();
                                MostPopularBikesBase bikes = new MostPopularBikesBase();
                                BikeMakeEntityBase objMake;
                                BikeModelEntityBase objModel;
                                BikeVersionsListEntity objVersion;
                                MinSpecsEntity specs;
                                while (dr.Read())
                                {
                                    bikes = new MostPopularBikesBase();
                                    bikes.BikeName = Convert.ToString(dr["Bike"]);
                                    bikes.HostURL = Convert.ToString(dr["HostURL"]);
                                    bikes.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                    bikes.VersionPrice = SqlReaderConvertor.ToNullableInt64(dr["OnRoadPrice"]);

                                    objMake = new BikeMakeEntityBase();
                                    objModel = new BikeModelEntityBase();
                                    objVersion = new BikeVersionsListEntity();
                                    specs = new MinSpecsEntity();

                                    objMake.MakeId = Convert.ToInt32(dr["MakeId"]);
                                    objMake.MakeId = !Convert.IsDBNull(dr["MakeId"]) ? Convert.ToInt32(dr["MakeId"]) : default(Int32);
                                    objMake.MakeName = Convert.ToString(dr["Make"]);
                                    objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);

                                    objModel.ModelId = !Convert.IsDBNull(dr["ModelId"]) ? Convert.ToInt32(dr["ModelId"]) : default(Int32);
                                    objModel.ModelName = Convert.ToString(dr["Model"]);
                                    objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                    objVersion.VersionId = !Convert.IsDBNull(dr["VersionId"]) ? Convert.ToInt32(dr["VersionId"]) : default(Int32);
                                    objVersion.VersionName = Convert.ToString(dr["Version"]);

                                    specs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                    specs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                    specs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["MaxPower"]);
                                    specs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaxPowerRPM"]);

                                    bikes.objMake = objMake;
                                    bikes.objModel = objModel;
                                    bikes.objVersion = objVersion;
                                    bikes.Specs = specs;

                                    models.Add(bikes);    
                                }
                                dealers.Models = models;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerByMakeCity sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerByMakeCity ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return dealers;
        }
    }
}
