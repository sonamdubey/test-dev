using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Photos
{
    public class ModelImage
    {                                  
        public uint ImageId { get; set; }         
        public string HostUrl { get; set; }             
        public string ImagePathThumbnail { get; set; }          
        public string ImagePathLarge { get; set; }              
        public short MainImgCategoryId { get; set; }             
        public string ImageCategory { get; set; }                
        public string Caption { get; set; }                 
        public string ImageName { get; set; }               
        public string AltImageName { get; set; }                 
        public string ImageTitle { get; set; }                 
        public string ImageDescription { get; set; }              
        public BikeMakeEntityBase MakeBase { get; set; }               
        public BikeModelEntityBase ModelBase { get; set; }             
        public string OriginalImgPath{ get; set; }
    }

}
