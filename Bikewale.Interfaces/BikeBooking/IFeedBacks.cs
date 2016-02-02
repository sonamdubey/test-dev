using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeBooking;

namespace Bikewale.Interfaces.Cancellation
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Date : 21 November 2016
    /// </summary>
    public interface IFeedBacks
    {
        bool SaveFeedBack(FeedBackEntity feedback); 
    }
}
