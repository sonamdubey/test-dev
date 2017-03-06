
using System.ComponentModel.DataAnnotations;
namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 03-03-2017
    /// Description: Used bike model image entity    
    /// </summary>
    public class UsedBikeModelImageEntity
    {
        [Required]
        public uint Modelid { get; set; }
        [Required]
        public uint UserId { get; set; }
    }
}
