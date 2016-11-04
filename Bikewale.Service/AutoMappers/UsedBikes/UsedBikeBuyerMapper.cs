
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
        {
            AutoMapper.Mapper.CreateMap<DTO.UsedBikes.PhotoRequestDTO, Entities.Used.PhotoRequest>();
            AutoMapper.Mapper.CreateMap<DTO.Customer.CustomerBase, CustomerEntityBase>();
            return AutoMapper.Mapper.Map<DTO.UsedBikes.PhotoRequestDTO, Entities.Used.PhotoRequest>(request);
        }

        internal static DTO.UsedBikes.BikeInterestDetailsDTO Convert(Entities.Used.BikeInterestDetails interestDetails)
        {
            AutoMapper.Mapper.CreateMap<Entities.Used.BikeInterestDetails, DTO.UsedBikes.BikeInterestDetailsDTO>();
            AutoMapper.Mapper.CreateMap<CustomerEntityBase, DTO.Customer.CustomerBase>();
            AutoMapper.Mapper.CreateMap<UsedBikeSellerBase, UsedBikeSellerBaseDTO>();
            return AutoMapper.Mapper.Map<Entities.Used.BikeInterestDetails, DTO.UsedBikes.BikeInterestDetailsDTO>(interestDetails);
        }

        internal static CustomerEntityBase Convert(DTO.Customer.CustomerBase buyer)
        {
            AutoMapper.Mapper.CreateMap<DTO.Customer.CustomerBase, CustomerEntityBase>();
            return AutoMapper.Mapper.Map<DTO.Customer.CustomerBase, CustomerEntityBase>(buyer);
        }

        internal static PurchaseInquiryResultDTO Convert(PurchaseInquiryResultEntity inquiryresult)
        {
            AutoMapper.Mapper.CreateMap<Entities.Used.PurchaseInquiryStatusEntity, DTO.UsedBikes.PurchaseInquiryStatusDTO>();
            AutoMapper.Mapper.CreateMap<CustomerEntityBase, DTO.Customer.CustomerBase>();
            AutoMapper.Mapper.CreateMap<Entities.Used.PurchaseInquiryResultEntity, DTO.UsedBikes.PurchaseInquiryResultDTO>();
            return AutoMapper.Mapper.Map<Entities.Used.PurchaseInquiryResultEntity, DTO.UsedBikes.PurchaseInquiryResultDTO>(inquiryresult);
        }

        internal static InquiryDetailsDTO Convert(InquiryDetails objInquiryDetails)
        {
            AutoMapper.Mapper.CreateMap<InquiryDetails, InquiryDetailsDTO>();
            return AutoMapper.Mapper.Map<InquiryDetailsDTO>(objInquiryDetails);
        }
        #region Automapper for sell your bike

        /// <summary>
        /// Created by : Sangram Nandkhile on 17 Oct 2016
        /// Summary: Automappers functions for Sell your bike
        /// </summary>
        /// <returns></returns>

        internal static SellBikeAdDTO Convert(SellBikeAd sellBikeAd)
        {
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.BikeData.BikeMakeEntityBase, BBMakeBase>();
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.BikeData.BikeModelEntityBase, BBModelBase>();
            AutoMapper.Mapper.CreateMap<BikeVersionEntityBase, BBVersionBase>();
            AutoMapper.Mapper.CreateMap<Bikewale.Entities.Used.SellAdStatus, Bikewale.DTO.UsedBikes.SellAdStatus>();
            AutoMapper.Mapper.CreateMap<SellBikeAdOtherInformation, SellBikeAdOtherInformationDTO>();
            AutoMapper.Mapper.CreateMap<SellerEntity, SellerDTO>();
            AutoMapper.Mapper.CreateMap<BikePhoto, Bikewale.DTO.Used.Search.BikePhoto>();
            AutoMapper.Mapper.CreateMap<SellBikeAd, SellBikeAdDTO>();
            return AutoMapper.Mapper.Map<SellBikeAdDTO>(sellBikeAd);
        }

        internal static SellBikeAd Convert(SellBikeAdDTO sellBikeAd)
        {
            AutoMapper.Mapper.CreateMap<BBMakeBase, Bikewale.Entities.BikeData.BikeMakeEntityBase>();
            AutoMapper.Mapper.CreateMap<BBModelBase, Bikewale.Entities.BikeData.BikeModelEntityBase>();
            AutoMapper.Mapper.CreateMap<BBVersionBase, BikeVersionEntityBase>();
            AutoMapper.Mapper.CreateMap<Bikewale.DTO.UsedBikes.SellAdStatus, Bikewale.Entities.Used.SellAdStatus>();
            AutoMapper.Mapper.CreateMap<SellBikeAdOtherInformationDTO, SellBikeAdOtherInformation>();
            AutoMapper.Mapper.CreateMap<SellerDTO, SellerEntity>();
            AutoMapper.Mapper.CreateMap<Bikewale.DTO.Used.Search.BikePhoto, BikePhoto>();
            AutoMapper.Mapper.CreateMap<SellBikeAdDTO, SellBikeAd>();
            return AutoMapper.Mapper.Map<SellBikeAd>(sellBikeAd);
        }

        internal static SellBikeAdOtherInformation Convert(SellBikeAdOtherInformationDTO otherInfo)
        {
            AutoMapper.Mapper.CreateMap<SellBikeAdOtherInformationDTO, SellBikeAdOtherInformation>();
            return AutoMapper.Mapper.Map<SellBikeAdOtherInformation>(otherInfo);
        }

        internal static SellBikeAdOtherInformationDTO Convert(SellBikeAdOtherInformation otherInfo)
        {
            AutoMapper.Mapper.CreateMap<SellBikeAdOtherInformation, SellBikeAdOtherInformationDTO>();
            return AutoMapper.Mapper.Map<SellBikeAdOtherInformationDTO>(otherInfo);
        }

        internal static SellBikeInquiryResultDTO Convert(SellBikeInquiryResultEntity inquiry)
        {
            AutoMapper.Mapper.CreateMap<SellBikeInquiryResultEntity, SellBikeInquiryResultDTO>();
            AutoMapper.Mapper.CreateMap<SellBikeAdStatusEntity, SellBikeAdStatusDTO>();
            return AutoMapper.Mapper.Map<SellBikeInquiryResultDTO>(inquiry);
        }

        internal static SellBikeInquiryResultEntity Convert(SellBikeInquiryResultDTO inquiry)
        {
            AutoMapper.Mapper.CreateMap<SellBikeInquiryResultDTO, SellBikeInquiryResultEntity>();
            return AutoMapper.Mapper.Map<SellBikeInquiryResultEntity>(inquiry);
        }

        internal static SellerEntity Convert(SellerDTO seller)
        {
            AutoMapper.Mapper.CreateMap<SellerDTO, SellerEntity>();
            return AutoMapper.Mapper.Map<SellerEntity>(seller);
        }

        internal static SellerDTO Convert(SellerEntity seller)
        {
            AutoMapper.Mapper.CreateMap<SellerEntity, SellerDTO>();
            return AutoMapper.Mapper.Map<SellerDTO>(seller);
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
            AutoMapper.Mapper.CreateMap<SellBikeImageUploadResultEntity, SellBikeImageUploadResultDTO>();
            AutoMapper.Mapper.CreateMap<SellBikeImageUploadResultBase, SellBikeImageUploadResultDTOBase>();
            AutoMapper.Mapper.CreateMap<ImageUploadStatus, ImageUploadStatusDTO>();
            AutoMapper.Mapper.CreateMap<ImageUploadResultStatus, ImageUploadResultStatusDTO>();
            return AutoMapper.Mapper.Map<SellBikeImageUploadResultEntity, SellBikeImageUploadResultDTO>(uploadResult);
        }
    }
}