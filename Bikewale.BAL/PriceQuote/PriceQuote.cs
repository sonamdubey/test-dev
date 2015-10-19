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
    /// Modified By :   Sumit Kate
    /// Date        :   16 Oct 2015
    /// Description :   Implemented newly added method of IPriceQuote interface
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

        /// <summary>
        /// Author  :   Sumit Kate
        /// Created On  :   16 Oct 2015
        /// Description :   Updates the price quote data
        /// </summary>
        /// <param name="pqId">Price Quote Id is mandatory</param>
        /// <param name="pqParams">Price Quote data that needs to be updated</param>
        /// <returns></returns>
        public bool UpdatePriceQuote(UInt32 pqId, PriceQuoteParametersEntity pqParams)
        {
            if (pqId > 0)
            {
                return objPQ.UpdatePriceQuote(pqId, pqParams);
            }
            else
            {
                return false;
            }
        }
    }   // class
}   // namespace
