using Bikewale.Interfaces.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeBooking;
using Bikewale.Notifications;
using Bikewale.CoreDAL;
using System.Data.SqlClient;
using System.Data;

namespace Bikewale.DAL.BikeBooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Feb 2016
    /// Description :   Booking Listing Repository
    /// </summary>
    public class BookingListingRepository : IBookingListing
    {
        public IEnumerable<Entities.BikeBooking.BikeBookingListingEntity> FetchBookingList(uint areaId, Entities.BikeBooking.BookingListingFilterEntity filter)
        {
            List<BikeBookingListingEntity> lstBikeBookingListingEntity = null;
            //IList<>
            Database db = null;
            try
            {
                db = new Database();

                if (areaId > 0)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetBookingListing";
                        cmd.Parameters.Add("@AreaId", SqlDbType.Int).Value = Convert.ToInt32(areaId);
                        #region Filters
                        if (filter != null)
                        {
                            if (!String.IsNullOrEmpty(filter.MakeIds))
                            {
                                cmd.Parameters.AddWithValue("@paramMakeIds", filter.MakeIds.Replace('+', ','));
                            }
                            if (!String.IsNullOrEmpty(filter.MakeIds))
                            {
                                cmd.Parameters.AddWithValue("@paramMinValBudget", filter.Budget.Split('-')[0]);
                            }
                            if (!String.IsNullOrEmpty(filter.MakeIds))
                            {
                                cmd.Parameters.AddWithValue("@paramMinValBudget", filter.Budget.Split('-')[1]);
                            }
                            if (!String.IsNullOrEmpty(filter.Mileage))
                            {
                                cmd.Parameters.AddWithValue("@paramMileageCategoryIds", filter.Mileage.Replace('+', ','));
                            }
                            if (!String.IsNullOrEmpty(filter.Displacement))
                            {
                                cmd.Parameters.AddWithValue("@paramDisplacementFilterIds", filter.Displacement.Replace('+', ','));
                            }
                            if (!String.IsNullOrEmpty(filter.RideStyle))
                            {
                                cmd.Parameters.AddWithValue("@paramRideStyleId", filter.RideStyle.Replace('+', ','));
                            }
                            if (!String.IsNullOrEmpty(filter.AntiBreakingSystem))
                            {
                                cmd.Parameters.AddWithValue("@paramHasABS", Convert.ToBoolean(filter.AntiBreakingSystem));
                            }
                            if (!String.IsNullOrEmpty(filter.BrakeType))
                            {
                                cmd.Parameters.AddWithValue("@paramDrumDisc", Convert.ToBoolean(filter.BrakeType));
                            }
                            if (!String.IsNullOrEmpty(filter.AlloyWheel))
                            {
                                cmd.Parameters.AddWithValue("@paramSpokeAlloy", filter.AlloyWheel);
                            }
                            if (!String.IsNullOrEmpty(filter.StartType))
                            {
                                cmd.Parameters.AddWithValue("@paramHasElectric", Convert.ToBoolean(filter.StartType));
                            }
                            if (!String.IsNullOrEmpty(filter.sc))
                            {
                                cmd.Parameters.AddWithValue("@paramSortCategoryId", filter.sc);
                            }
                            if (!String.IsNullOrEmpty(filter.so))
                            {
                                cmd.Parameters.AddWithValue("@paramSortOrder", filter.so);
                            }
                        } 
                        #endregion

                        using (SqlDataReader dr = db.SelectQry(cmd))
                        {
                            lstBikeBookingListingEntity = new List<BikeBookingListingEntity>();
                            if (dr != null && dr.HasRows)
                            {
                                #region Bike details
                                while (dr.Read())
                                {
                                    BikeBookingListingEntity objBikeBookingListingEntity = new BikeBookingListingEntity();
                                    objBikeBookingListingEntity.MakeEntity = new Entities.BikeData.BikeMakeEntityBase()
                                    {
                                        MakeId = Convert.ToInt32(dr["MakeId"]),
                                        MakeName = Convert.ToString(dr["MakeName"]),
                                        MaskingName = Convert.ToString(dr["MakeMaskingName"])
                                    };
                                    objBikeBookingListingEntity.ModelEntity = new Entities.BikeData.BikeModelEntityBase()
                                    {
                                        ModelId = Convert.ToInt32(dr["ModelId"]),
                                        ModelName = Convert.ToString(dr["ModelName"]),
                                        MaskingName = Convert.ToString(dr["ModelMaskingName"])
                                    };
                                    objBikeBookingListingEntity.VersionEntity = new Entities.BikeData.BikeVersionEntityBase()
                                    {
                                        VersionId = Convert.ToInt32(dr["VersionId"]),
                                        VersionName = Convert.ToString(dr["VersionName"])
                                    };
                                    objBikeBookingListingEntity.BikeName = Convert.ToString(dr["Bike"]);
                                    objBikeBookingListingEntity.ExShowroom = Convert.ToUInt32(dr["VersionPrice"]);
                                    objBikeBookingListingEntity.HostUrl = Convert.ToString(dr["HostUrl"]);
                                    objBikeBookingListingEntity.OriginalImagePath = Convert.ToString(dr["ImgPath"]);
                                    objBikeBookingListingEntity.DealerId = Convert.ToUInt32(dr["Dealer"]);
                                    objBikeBookingListingEntity.Displacement = Convert.ToSingle(dr["Displacement"]);
                                    objBikeBookingListingEntity.Mileage = Convert.ToUInt16(dr["Mileage"]);
                                    objBikeBookingListingEntity.HasABS = Convert.ToBoolean(dr["hasABS"]);
                                    objBikeBookingListingEntity.HasDisc = Convert.ToBoolean(dr["discDrum"]);
                                    objBikeBookingListingEntity.HasAlloyWheels = Convert.ToBoolean(dr["hasAlloyWheels"]);
                                    objBikeBookingListingEntity.HasElectricStart = Convert.ToBoolean(dr["hasElectricStart"]);
                                    objBikeBookingListingEntity.BookingAmount = Convert.ToUInt32(dr["BookingAmount"]);
                                    objBikeBookingListingEntity.PopularityIndex = Convert.ToUInt32(dr["PopularityIndex"]);
                                } 
                                #endregion

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BookingListingRepository.FetchBookingList");
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return lstBikeBookingListingEntity;
        }
    }
}
