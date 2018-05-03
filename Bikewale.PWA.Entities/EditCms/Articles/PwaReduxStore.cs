using Bikewale.PWA.Entities.Images;
using System.Collections.Generic;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By: Prasad Gawde
    /// Modified by : Ashutosh Sharma on 14 Feb 2018
    /// Description : Added reducer for Widgets.
    /// </summary>
    public class PwaReduxStore
    {
        public PwaNewsReducer News { get; private set; }
        public PwaVideosReducer Videos { get; private set; }
        public PwaWidgetsReducer Widgets { get; private set; }
		public PwaFinanceReducer Finance { get; set; }
        public PwaEmiReducer Emi { get; set; }

        public PwaReduxStore()
        {
            News = new PwaNewsReducer();
            Videos = new PwaVideosReducer();
            Widgets = new PwaWidgetsReducer();
			Finance = new PwaFinanceReducer();
            Emi = new PwaEmiReducer();
        }

    }

	public class PwaFinanceReducer
	{
		public object SelectBikePopup;
		public object FinanceCityPopup;
		public object SimilarBikesEMI;

		public PwaFinanceReducer()
		{
			SelectBikePopup = new object();
			FinanceCityPopup = new object();
			SimilarBikesEMI = new object();
		}
	}

    public class PwaEmiReducer
    {
        public object VehicleDownPayment;
        public object VehicleTenure;
        public object VehicleInterest;

        public PwaEmiReducer()
        {
            VehicleDownPayment = new object();
            VehicleTenure = new object();
            VehicleInterest = new object();
        }
    }
}