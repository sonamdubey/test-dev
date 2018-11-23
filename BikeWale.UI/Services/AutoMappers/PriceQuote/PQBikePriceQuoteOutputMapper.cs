using AutoMapper;
using Bikewale.DTO.PriceQuote.Area.v2;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.DTO.PriceQuote.City.v2;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class PQBikePriceQuoteOutputMapper
    {
        internal static DTO.PriceQuote.BikeQuotation.PQBikePriceQuoteOutput Convert(Entities.PriceQuote.BikeQuotationEntity quotation)
        {
          return Mapper.Map<BikeQuotationEntity, PQBikePriceQuoteOutput>(quotation);
        }

        internal static List<DTO.PriceQuote.v2.DPQDealerBase> Convert(IEnumerable<NewBikeDealerBase> enumerable)
        {
            return Mapper.Map<IEnumerable<NewBikeDealerBase>, List<DTO.PriceQuote.v2.DPQDealerBase>>(enumerable);
        }

        internal static DTO.PriceQuote.v2.PQPrimaryDealer Convert(Entities.BikeBooking.NewBikeDealers newBikeDealers)
        {
            return Mapper.Map<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>(newBikeDealers);
        }

        internal static DTO.PriceQuote.v2.DPQuotationOutput Convert(DetailedDealerQuotationEntity objDealerQuotation, IEnumerable<PQ_BikeVarient> varients)
        {
            DTO.PriceQuote.v2.DPQuotationOutput output = new DTO.PriceQuote.v2.DPQuotationOutput();
            output.emi = ConvertEMI(objDealerQuotation.PrimaryDealer.EMIDetails);
            output.Versions = ConvertVersions(varients);

            if (objDealerQuotation != null && objDealerQuotation.PrimaryDealer != null && objDealerQuotation.PrimaryDealer.DealerDetails != null && (objDealerQuotation.PrimaryDealer.IsPremiumDealer))
            {
                output.Benefits = ConvertBenefits(objDealerQuotation.PrimaryDealer.Benefits);
                output.Offers = ConvertOffers(objDealerQuotation.PrimaryDealer.OfferList);
            }
            foreach (var version in varients)
            {
                //For App if the Price Break up components are more than 4 
                if (version.PriceList.Count > 4)
                {
                    IList<DTO.PriceQuote.v2.DPQ_Price> otherList = new List<DTO.PriceQuote.v2.DPQ_Price>();
                    IList<DTO.PriceQuote.v2.DPQ_Price> mainList = new List<DTO.PriceQuote.v2.DPQ_Price>();
                    foreach (var pl in version.PriceList)
                    {
                        switch (pl.CategoryId)
                        {
                            #region Ex-Showroom Components
                            //Basic Cost
                            case 1:
                            //Ex-showroom
                            case 3:
                                mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                            #endregion

                            #region RTO Components
                            //RTO
                            case 5:
                            //RTO Expense
                            case 6:
                            //RTO Tax. Registration & Handling Charges
                            case 7:
                            //RTO Tax. Registration + Smart Card & Handling Charges
                            case 8:
                            //R T O + Smart Card
                            case 26:
                            //Comprehensive Insurance, LTTAX & Reg. Fees
                            case 45:
                                mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                            #endregion

                            #region Insurance
                            //Insurance (Only Bike)
                            case 10:
                            //Insurance (Comprehensive)
                            case 11:
                            //Insurance (Zero Depreciation)
                            case 12:
                            //Insurance (3 Years)
                            case 43:
                                mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                            #endregion

                            default:
                                otherList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                        }
                    }
                    output.Versions.SingleOrDefault(m => m.VersionId == version.objVersion.VersionId).OtherPriceList = otherList;
                    output.Versions.SingleOrDefault(m => m.VersionId == version.objVersion.VersionId).PriceList = mainList;
                    //Add Other into main price list
                    if (otherList.Count > 0)
                    {
                        mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = "Other Charges", Price = System.Convert.ToUInt32(otherList.Sum(m => m.Price)) });
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Converts the specified object dealer quotation.
        /// Created by: Sangram Nandkhile on 13 Oct 2017
        /// </summary>
        /// <param name="objDealerQuotation">The object dealer quotation.</param>
        /// <param name="varients">The varients.</param>
        /// <returns></returns>
        internal static DTO.PriceQuote.v3.DPQuotationOutput Convert(Entities.PriceQuote.v2.DetailedDealerQuotationEntity objDealerQuotation, IEnumerable<PQ_BikeVarient> varients)
        {
            DTO.PriceQuote.v3.DPQuotationOutput dealerPriceQuote = null;
            if (objDealerQuotation != null)
            {
                dealerPriceQuote = new DTO.PriceQuote.v3.DPQuotationOutput();
                dealerPriceQuote.emi = ConvertEMI(objDealerQuotation.PrimaryDealer.EMIDetails);
                dealerPriceQuote.Versions = ConvertVersions(varients);

                if (objDealerQuotation != null && objDealerQuotation.PrimaryDealer != null && objDealerQuotation.PrimaryDealer.DealerDetails != null && (objDealerQuotation.PrimaryDealer.IsPremiumDealer))
                {
                    dealerPriceQuote.Benefits = ConvertBenefits(objDealerQuotation.PrimaryDealer.Benefits);
                }
            }
            #region foreach version list

            foreach (var version in varients)
            {
                //For App if the Price Break up components are more than 4 
                if (version.PriceList.Count > 4)
                {
                    IList<DTO.PriceQuote.v2.DPQ_Price> otherList = new List<DTO.PriceQuote.v2.DPQ_Price>();
                    IList<DTO.PriceQuote.v2.DPQ_Price> mainList = new List<DTO.PriceQuote.v2.DPQ_Price>();
                    foreach (var pl in version.PriceList)
                    {
                        switch (pl.CategoryId)
                        {
                            #region Ex-Showroom Components
                            //Basic Cost
                            case 1:
                            //Ex-showroom
                            case 3:
                                mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                            #endregion

                            #region RTO Components
                            //RTO
                            case 5:
                            //RTO Expense
                            case 6:
                            //RTO Tax. Registration & Handling Charges
                            case 7:
                            //RTO Tax. Registration + Smart Card & Handling Charges
                            case 8:
                            //R T O + Smart Card
                            case 26:
                            //Comprehensive Insurance, LTTAX & Reg. Fees
                            case 45:
                                mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                            #endregion

                            #region Insurance
                            //Insurance (Only Bike)
                            case 10:
                            //Insurance (Comprehensive)
                            case 11:
                            //Insurance (Zero Depreciation)
                            case 12:
                            //Insurance (3 Years)
                            case 43:
                                mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                            #endregion

                            default:
                                otherList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                        }
                    }
                    if (dealerPriceQuote != null)
                    {
                        dealerPriceQuote.Versions.SingleOrDefault(m => m.VersionId == version.objVersion.VersionId).OtherPriceList = otherList;
                        dealerPriceQuote.Versions.SingleOrDefault(m => m.VersionId == version.objVersion.VersionId).PriceList = mainList;
                    }
                    //Add Other into main price list
                    if (otherList.Count > 0)
                    {
                        mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = "Other Charges", Price = System.Convert.ToUInt32(otherList.Sum(m => m.Price)) });
                    }
                }
            }

            #endregion
            return dealerPriceQuote;
        }

        internal static DTO.PriceQuote.v4.DPQuotationOutput ConvertV2(DetailedDealerQuotationEntity objDealerQuotation, IEnumerable<PQ_BikeVarient> varients)
        {
            DTO.PriceQuote.v4.DPQuotationOutput output = new DTO.PriceQuote.v4.DPQuotationOutput();
            output.emi = ConvertEMI(objDealerQuotation.PrimaryDealer.EMIDetails);
            output.Versions = ConvertVersions(varients);

            if (objDealerQuotation != null && objDealerQuotation.PrimaryDealer != null && objDealerQuotation.PrimaryDealer.DealerDetails != null && (objDealerQuotation.PrimaryDealer.IsPremiumDealer))
            {
                output.Benefits = ConvertBenefits(objDealerQuotation.PrimaryDealer.Benefits);
                output.Offers = ConvertOffers(objDealerQuotation.PrimaryDealer.OfferList);
            }
            foreach (var version in varients)
            {
                //For App if the Price Break up components are more than 4 
                if (version.PriceList.Count > 4)
                {
                    IList<DTO.PriceQuote.v2.DPQ_Price> otherList = new List<DTO.PriceQuote.v2.DPQ_Price>();
                    IList<DTO.PriceQuote.v2.DPQ_Price> mainList = new List<DTO.PriceQuote.v2.DPQ_Price>();
                    foreach (var pl in version.PriceList)
                    {
                        switch (pl.CategoryId)
                        {
                            #region Ex-Showroom Components
                            //Basic Cost
                            case 1:
                            //Ex-showroom
                            case 3:
                                mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                            #endregion

                            #region RTO Components
                            //RTO
                            case 5:
                            //RTO Expense
                            case 6:
                            //RTO Tax. Registration & Handling Charges
                            case 7:
                            //RTO Tax. Registration + Smart Card & Handling Charges
                            case 8:
                            //R T O + Smart Card
                            case 26:
                            //Comprehensive Insurance, LTTAX & Reg. Fees
                            case 45:
                                mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                            #endregion

                            #region Insurance
                            //Insurance (Only Bike)
                            case 10:
                            //Insurance (Comprehensive)
                            case 11:
                            //Insurance (Zero Depreciation)
                            case 12:
                            //Insurance (3 Years)
                            case 43:
                                mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                            #endregion

                            default:
                                otherList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = pl.CategoryName, Price = pl.Price });
                                break;
                        }
                    }
                    output.Versions.SingleOrDefault(m => m.VersionId == version.objVersion.VersionId).OtherPriceList = otherList;
                    output.Versions.SingleOrDefault(m => m.VersionId == version.objVersion.VersionId).PriceList = mainList;
                    //Add Other into main price list
                    if (otherList.Count > 0)
                    {
                        mainList.Add(new DTO.PriceQuote.v2.DPQ_Price() { CategoryName = "Other Charges", Price = System.Convert.ToUInt32(otherList.Sum(m => m.Price)) });
                    }
                }
            }
            return output;
        }

        private static IEnumerable<DTO.PriceQuote.v2.DPQVersionBase> ConvertVersions(IEnumerable<PQ_BikeVarient> varients)
        {
            return Mapper.Map<IEnumerable<PQ_BikeVarient>, IEnumerable<DTO.PriceQuote.v2.DPQVersionBase>>(varients);
        }

        private static IEnumerable<DTO.PriceQuote.v2.DPQ_Price> ConvertPriceList(IEnumerable<Entities.BikeBooking.PQ_Price> enumerable)
        {
         
            return Mapper.Map<IEnumerable<Entities.BikeBooking.PQ_Price>, IEnumerable<DTO.PriceQuote.v2.DPQ_Price>>(enumerable);
        }

        private static IEnumerable<DTO.PriceQuote.v2.DPQOffer> ConvertOffers(IEnumerable<Entities.OfferEntityBase> enumerable)
        {
           
            return Mapper.Map<IEnumerable<Entities.OfferEntityBase>, IEnumerable<DTO.PriceQuote.v2.DPQOffer>>(enumerable);
        }

        private static IEnumerable<DTO.PriceQuote.v2.DPQBenefit> ConvertBenefits(IEnumerable<Entities.DealerBenefitEntity> enumerable)
        {
           
            return Mapper.Map<IEnumerable<Entities.DealerBenefitEntity>, IEnumerable<DTO.PriceQuote.v2.DPQBenefit>>(enumerable);
        }


        private static DTO.PriceQuote.v2.EMI ConvertEMI(Entities.BikeBooking.EMI eMI)
        {
           
            return Mapper.Map<Entities.BikeBooking.EMI, DTO.PriceQuote.v2.EMI>(eMI);
        }

        internal static IEnumerable<DTO.Version.VersionBase> Convert(IEnumerable<OtherVersionInfoEntity> enumerable)
        {
          
            return Mapper.Map<IEnumerable<OtherVersionInfoEntity>, IEnumerable<DTO.Version.VersionBase>>(enumerable);
        }

        internal static List<DTO.PriceQuote.v3.DPQDealerBase> Convert(IEnumerable<Entities.PriceQuote.v2.NewBikeDealerBase> enumerable)
        {

            return Mapper.Map<IEnumerable<Entities.PriceQuote.v2.NewBikeDealerBase>, List<DTO.PriceQuote.v3.DPQDealerBase>>(enumerable);
        }

        internal static DTO.PriceQuote.BikePQOutput Convert(Entities.PriceQuote.v2.PQByCityAreaEntity pqOut)
        {
          
            return Mapper.Map<Entities.PriceQuote.v2.PQByCityAreaEntity, DTO.PriceQuote.BikePQOutput>(pqOut);
        }


        internal static Bikewale.DTO.PriceQuote.v2.BikePQOutput Convert(Bikewale.Entities.PriceQuote.v4.PQByCityAreaEntity pqOut)
        {
            return Mapper.Map<Entities.PriceQuote.v4.PQByCityAreaEntity, DTO.PriceQuote.v2.BikePQOutput>(pqOut);
        }
    }
}