using System.ComponentModel.DataAnnotations;
namespace BikewaleOpr.Entities.BikeColorImages
{
    /// <summary>
    /// Created By :- Subodh Jain 09 jan 2017
    /// Summary :- Bikes Images Details 
    /// </summary>
    public class ColorImagesBikeEntities
    {
        [Required]
        public uint? Modelid;
        [Required]
        public uint? ModelColorId;
        [Required]
        public uint? UserId;

    }
}
