using BikewaleOpr.Entities;

namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// to store models with it's respective make.
    /// 
    /// created by: vivek singh tomar on 27/07/2017.
    /// </summary>
    public class BikeMakeModelData
    {
        public BikeMakeEntityBase BikeMake { get; set; }
        public BikeModelEntityBase BikeModel { get; set; }

    }
}
