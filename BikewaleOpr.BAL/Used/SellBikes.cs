using Bikewale.Entities.Used;
using BikewaleOpr.Interface.Used;
using System.Collections.Generic;


namespace BikewaleOpr.BAL.Used
{
    /// <summary>
    /// Created by:Sangram Nandkhile on 24 Oct 2016
    /// Desc: Repository for Sell your bike 
    /// </summary>
    public class SellBikes : ISellBikes
    {
        private ISellerRepository _sellerRepo;
        public SellBikes(ISellerRepository sellerRepository)
        {
            _sellerRepo = sellerRepository;
        }

        public IEnumerable<SellBikeAd> GetClassifiedPendingInquiries()
        {
            return _sellerRepo.GetClassifiedPendingInquiries();
        }
    }
}
