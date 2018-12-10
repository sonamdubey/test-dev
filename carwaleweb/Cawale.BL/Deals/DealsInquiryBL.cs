using AutoMapper;
using Carwale.DTOs;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.Entity;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Dealers;
using Carwale.Entity.Deals;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Offers;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces;
using Carwale.Interfaces.Deals;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Deals
{
    public class DealsInquiryBL : IDealsUserInquiry<DropOffInquiryDetailDTO>
    {
        private readonly IDealsUserInquiry<DropOffInquiryDetailEntity> _dealsInquiryRepo;

        public DealsInquiryBL(IDealsUserInquiry<DropOffInquiryDetailEntity> dealsInquiryRepo,IDealsRepository dealsRepository)
        {
            _dealsInquiryRepo = dealsInquiryRepo;
           
        }

        public List<DropOffInquiryDetailDTO> GetDealsDroppedUsers()
        {
            List<DropOffInquiryDetailDTO> Users = new List<DropOffInquiryDetailDTO>();
            try
            {
                Mapper.CreateMap<DropOffInquiryDetailEntity, DropOffInquiryDetailDTO>()
                    .ForMember(x => x.ShortDescription, o => o.MapFrom(s => s.Offer.ShortDescription))
                    .ForMember(x => x.InteriorColor, o => o.MapFrom(s => s.StockDetail.InteriorColor))
                    .ForMember(x => x.TCStockId, o => o.MapFrom(s => s.StockDetail.TCStockId))
                    .ForMember(x => x.Color, o => o.MapFrom(s => s.StockDetail.Color));
                Mapper.CreateMap<TransactionDetails, CustomerBaseDTO>()
                    .ForMember(x => x.Name, o => o.MapFrom(s => s.CustomerName))
                    .ForMember(x => x.Mobile, o => o.MapFrom(s => s.CustMobile))
                    .ForMember(x => x.Email, o => o.MapFrom(s => s.CustEmail));
                Mapper.CreateMap<CustLocation, CustLocationDTO>()
                    .ForMember(x => x.CityName, o => o.MapFrom(s => s.CityName));
                Mapper.CreateMap<DealerSummary, DealerSummaryDTO>();
                Mapper.CreateMap<CarEntity, CarDetailBase>();

                Users = Mapper.Map<List<DropOffInquiryDetailEntity>, List<DropOffInquiryDetailDTO>>(_dealsInquiryRepo.GetDealsDroppedUsers());
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealsBL.GetDealsDroppedUsers()");
                objErr.LogException();
            }
            return Users;
        }      

    }
}
