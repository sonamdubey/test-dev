using System;

namespace BikeWaleOpr
{
    public class AjaxCompareFeaturedBike
	{
		
		[AjaxPro.AjaxMethod()]
        public bool SaveCompareFeaturedBike(string comparisonBikes, string featuredBike)
		{
            bool status = false;
      
			string[] arrCompareBike = comparisonBikes.Split(',');
			for (int i=0; i<arrCompareBike.Length; i++)
			{
                 if (SaveData(arrCompareBike[i], featuredBike))
                     status = true;
                  else
                      status = false;
            }

            return status;
		}
        /*
         * Modified :   Vaibhav K (2-May-2012)
         * Summary  :   Copy compared Bikes from one featuredversion against new featured version
        */
        [AjaxPro.AjaxMethod()]
        public bool CopyComparedBikes(string copyBike, string featuredBike)
        {
            throw new Exception("Method not used/commented");            
        }
        private bool SaveData(string compareBike, string featuredBike)
		{
            throw new Exception("Method not used/commented");
          
		}
		
		[AjaxPro.AjaxMethod()]		
		public bool DeleteCompareFeaturedBike(string comparisonBike, string featuredBike)
		{
            throw new Exception("Method not used/commented");

           
		}
		
		
		[AjaxPro.AjaxMethod()]		
		public bool DeleteFeaturedBike(string featuredBike)
		{
            throw new Exception("Method not used/commented");

          
		}
	}
}		