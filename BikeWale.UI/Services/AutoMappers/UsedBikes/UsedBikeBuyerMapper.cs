
using AutoMapper;
using Bikewale.DTO.BikeBooking.Make;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.DTO.BikeBooking.Version;
using Bikewale.DTO.Used;
using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
namespace Bikewale.Service.AutoMappers.UsedBikes
{
    public class UsedBikeBuyerMapper
    {
        internal static Entities.Used.PhotoRequest Convert(DTO.UsedBikes.PhotoRequestDTO request)
        { return Mapper.Map<DTO.UsedBikes.PhotoRequestDTO, Entities.Used.PhotoRequest>(request);
        }

        internal static DTO.UsedBikes.BikeInterestDetailsDTO Convert(Entities.Used.BikeInterestDetails interestDetails)
        {
          
            return Mapper.Map<Entities.Used.BikeInterestDetails, DTO.UsedBikes.BikeInterestDetailsDTO>(interestDetails);
        }

        internal static CustomerEntityBase Convert(DTO.Customer.CustomerBase buyer)
        {
          
            return Mapper.Map<DTO.Customer.CustomerBase, CustomerEntityBase>(buyer);
        }

        internal static PurchaseInquiryResultDTO Convert(PurchaseInquiryResultEntity inquiryresult)
        {
          
            return Mapper.Map<Entities.Used.PurchaseInquiryResultEntity, DTO.UsedBikes.PurchaseInquiryResultDTO>(inquiryresult);
        }

        internal static InquiryDetailsDTO Convert(InquiryDetails objInquiryDetails)
        {
           
            return Mapper.Map<InquiryDetailsDTO>(objInquiryDetails);
        }
        #region Automapper for sell your bike

        /// <summary>
        /// Created by : Sangram Nandkhile on 17 Oct 2016
        /// Summary: Automappers functions for Sell your bike
        /// </summary>
        /// <returns></returns>

        internal static SellBikeAdDTO Convert(SellBikeAd sellBikeAd)
        {
         
            return Mapper.Map<SellBikeAdDTO>(sellBikeAd);
        }

        internal static SellBikeAd Convert(SellBikeAdDTO sellBikeAd)
        {
          
            return Mapper.Map<SellBikeAd>(sellBikeAd);
        }

        internal static SellBikeAdOtherInformation Convert(SellBikeAdOtherInformationDTO otherInfo)
        {
           
            return Mapper.Map<SellBikeAdOtherInformation>(otherInfo);
        }

        internal static SellBikeAdOtherInformationDTO Convert(SellBikeAdOtherInformation otherInfo)
        {
          
            return Mapper.Map<SellBikeAdOtherInformationDTO>(otherInfo);
        }

        internal static SellBikeInquiryResultDTO Convert(SellBikeInquiryResultEntity inquiry)
        {
          
            return Mapper.Map<SellBikeInquiryResultDTO>(inquiry);
        }

        internal static SellBikeInquiryResultEntity Convert(SellBikeInquiryResultDTO inquiry)
        {
          
            return Mapper.Map<SellBikeInquiryResultEntity>(inquiry);
        }

        internal static SellerEntity Convert(SellerDTO seller)
        {
           
            return Mapper.Map<SellerEntity>(seller);
        }

        internal static SellerDTO Convert(SellerEntity seller)
        {
          
            return Mapper.Map<SellerDTO>(seller);
        }

        #endregion

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Oct 2016
        /// Description :   Converts Sell Bike image result entity to DTO
        /// </summary>
        /// <param name="uploadResult"></param>
        /// <returns></returns>
        internal static SellBikeImageUploadResultDTO Convert(SellBikeImageUploadResultEntity uploadResult)
        {
            return Mapper.Map<SellBikeImageUploadResultEntity, SellBikeImageUploadResultDTO>(uploadResult);
        }
    }
}