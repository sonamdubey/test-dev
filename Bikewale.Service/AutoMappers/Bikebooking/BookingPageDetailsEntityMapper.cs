using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Service.AutoMappers.Bikebooking
{
    public class BookingPageDetailsEntityMapper
    {
        /// <summary>
        /// Modified by : Rajan Chauhan on 26 Mar 2018
        /// Description : Added mapping for MinSpec
        /// </summary>
        /// <param name="objBookingPageDetailsEntity"></param>
        /// <returns></returns>
        internal static BookingPageDetailsDTO Convert(Entities.PriceQuote.BookingPageDetailsEntity objBookingPageDetailsEntity)
        {
            try
            {
                if (objBookingPageDetailsEntity != null)
                {
                    Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    Mapper.CreateMap<BikeVersionMinSpecs, VersionMinSpecs>();
                    Mapper.CreateMap<DealerVersionPriceItemEntity, DealerVersionPriceItemDTO>();
                    Mapper.CreateMap<BikeDealerPriceDetail, BikeDealerPriceDetailDTO>();
                    Mapper.CreateMap<BookingPageDetailsEntity, BookingPageDetailsDTO>();
                    Mapper.CreateMap<DealerDetails, DealerDetailsDTO>();
                    Mapper.CreateMap<DealerOfferEntity, DealerOfferDTO>();
                    Mapper.CreateMap<BikeVersionColorsWithAvailability, BikeVersionColorsWithAvailabilityDTO>();
                    BookingPageDetailsDTO objBookingPageDetailsDTO = Mapper.Map<BookingPageDetailsEntity, BookingPageDetailsDTO>(objBookingPageDetailsEntity);

                    if (objBookingPageDetailsEntity.Varients != null)
                    {
                        IList<BikeDealerPriceDetailDTO> VarientList = new List<BikeDealerPriceDetailDTO>();
                        foreach (BikeDealerPriceDetail bikeVersion in objBookingPageDetailsEntity.Varients)
                        {
                            BikeDealerPriceDetailDTO objbikeDealerPrice = Mapper.Map<BikeDealerPriceDetail, BikeDealerPriceDetailDTO>(bikeVersion);
                            objbikeDealerPrice.MinSpec = SpecsFeaturesMapper.ConvertToVersionMinSpecs(bikeVersion.MinSpec);
                            VarientList.Add(objbikeDealerPrice);
                        }
                        objBookingPageDetailsDTO.Varients = VarientList;

                    }
                    return objBookingPageDetailsDTO;
                }
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Service.AutoMappers.Bikebooking.Convert( BookingPageDetailsEntity {0})", objBookingPageDetailsEntity));
            }
            return null;
        }
    }
}