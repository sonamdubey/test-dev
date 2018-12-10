using Carwale.Entity.Classified.SellCarUsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces.Classified.SellCar;

namespace Carwale.BL.Classified.UsedSellCar
{
    public class CarDetailsBL : ICarDetailsBL
    {
        private readonly ICarDetailsRepository _carDetailsRepo;
        private readonly IConsumerToBusinessBL _consumerToBusinessBL;
        private readonly ISellCarRepository _sellCarRepository;

        public CarDetailsBL(
            ICarDetailsRepository carDetailsRepo,
            IConsumerToBusinessBL consumerToBusinessBL,
            ISellCarRepository sellCarRepository
            )
        {
            _carDetailsRepo = carDetailsRepo;
            _consumerToBusinessBL = consumerToBusinessBL;
            _sellCarRepository = sellCarRepository;
        }

        public int ProcessCarDetails(SellCarInfo sellCarInfo)
        {
            sellCarInfo.TempInquiryId = _sellCarRepository.SaveTempSellCarInquiryDetails(sellCarInfo);
            _consumerToBusinessBL.PushToIndividualStockQueue(sellCarInfo.TempInquiryId, C2BActionType.AddCarDetails);
            return sellCarInfo.TempInquiryId;
        }
    }
}
