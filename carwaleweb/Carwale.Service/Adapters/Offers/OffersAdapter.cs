using System;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.Offers;
using AutoMapper;
using Carwale.DTOs.OffersV1;
using Carwale.Entity.OffersV1;
using Carwale.Notifications.Logs;

namespace Carwale.Service.Adapters.Offers
{
    public class OffersAdapter : IOffersAdapter
    {
        private readonly IOfferBL _offersBL;
        public OffersAdapter(IOfferBL offersBL)
        {
            _offersBL = offersBL;
        }

        public OfferDto GetOffers(OfferInput offerInput)
        {
            try
            {
                var offer = _offersBL.GetOffer(offerInput);
                var offersDto = Mapper.Map<OfferDto>(offer);
                if (offer != null && offer.OfferDetails != null)
                {
                    offersDto.OfferDetails.ValidityText = GetValidityText(offer.OfferDetails.IsExtended, offersDto.OfferDetails);
                }
                return offersDto;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }        

        public string GetValidityText(bool isOfferExtended, OfferDetailsDto offerDetails)
        {
            if (isOfferExtended && DateTime.Now.Date > offerDetails.EndDate)
            {
                var extendedDateTxt = "This offer may have expired on " + offerDetails.EndDate.ToString("dd MMMM") + ". Kindly contact the dealer for current offers";
                offerDetails.ValidityText = extendedDateTxt;
            }
            else
            {
                var offerValidityText = "Offer Valid Till : ";
                offerDetails.ValidityText = offerValidityText + offerDetails.EndDate.ToString("dd MMM, yyyy");
            }
            return offerDetails.ValidityText;
        }
    }
}
