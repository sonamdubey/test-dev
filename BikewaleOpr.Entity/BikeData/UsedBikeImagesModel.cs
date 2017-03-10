using BikewaleOpr.Entities;

namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 08-03-2017
    /// Description : Entity to hold Make wise model count whose used images have not been uploaded.
    /// </summary>
    public class UsedBikeImagesModel : BikeMakeEntityBase
    {
        public string ModelName { get; set; }
    }
}
