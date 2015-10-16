using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DTO
{
    public class Review : ReviewBase
    {
        public string Comments { get; set; }
        public string Pros { get; set; }
        public string Cons { get; set; }
        public ushort Liked { get; set; }
        public ushort Disliked { get; set; }
        public uint Viewed { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public ReviewRatingBase OverAllRating { get; set; }
        public ReviewTaggedBike TaggedBike { get; set; }
    }
}
