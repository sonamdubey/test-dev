using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.DAL.PriceQuote;

namespace Bikewale.BAL.PriceQuote
{
    /// <summary>
    /// Created By: Ashish G. Kamble
    /// Summary : Class have functions for the price quote business layer.
    /// </summary>
    public class PriceQuote : IPriceQuote
    {
        private readonly IPriceQuote objPQ = null;

        public PriceQuote()
        {
            using (IUnityContainer objPQCont = new UnityContainer())
            {
                objPQCont.RegisterType<IPriceQuote, PriceQuoteRepository>();
                objPQ = objPQCont.Resolve<IPriceQuote>();
            }
        }

        /// <summary>
        /// Function to save the price quote
        /// </summary>
        /// <param name="pqParams">All parameters necessory to save the price quote.</param>
        /// <returns>Returns price quote id</returns>
        public ulong RegisterPriceQuote(PriceQuoteParametersEntity pqParams)
        {
            ulong pqId = 0;
            
            pqId = objPQ.RegisterPriceQuote(pqParams);

            return pqId;
        }

        /// <summary>
        /// Function to get the price quote by price quote id.
        /// </summary>
        /// <param name="pqId">Price quote id. Only positive numbers are allowed.</param>
        /// <returns>Returns price quote information in the PriceQuoteEntity object.</returns>
        public BikeQuotationEntity GetPriceQuoteById(ulong pqId)
        {
            BikeQuotationEntity objQuotation = null;

            objQuotation = objPQ.GetPriceQuoteById(pqId);

            return objQuotation;
        }

        /// <summary>
        /// Function to get the price quote by price quote information. Price quote will be registered automatically and returns price quote.
        /// </summary>
        /// <param name="pqParams">all parameters necessory to save the price quote.</param>
        /// <returns>Retunrs price quote in the PricQuoteEntity object.</returns>
        public BikeQuotationEntity GetPriceQuote(PriceQuoteParametersEntity pqParams)
        {
            BikeQuotationEntity objQuotation = null;
            
            objQuotation = objPQ.GetPriceQuote(pqParams);

            return objQuotation;
        }

        /// <summary>
        /// Function to get the other versions of the model whose price quote is taken.
        /// </summary>
        /// <param name="pqId">Price quote id</param>
        /// <returns>Returns list containing versions with on road prices.</returns>
        public List<OtherVersionInfoEntity> GetOtherVersionsPrices(ulong pqId)
        {
            List<OtherVersionInfoEntity> objVersionsList = null;

            objVersionsList = objPQ.GetOtherVersionsPrices(pqId);

            return objVersionsList;
        }

    }   // class
}   // namespace
