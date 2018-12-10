using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ImageUpload
{
    public class Image
    {
        public string Extension { get; set; }

        public long? Id { get; set; }

        public uint CategoryId { get; set; }

        public uint? ItemId { get; set; }

        public string HostUrl { get; set; }

        public string OriginalPath { get; set; }

        public bool? IsReplicated { get; set; }

        public uint? ReplicatedId { get; set; }

        public decimal AspectRatio { get; set; }

        public bool? IsWaterMark { get; set; }

        public bool? IsMain { get; set; }

        public bool? IsMaster { get; set; }

        public uint? ProcessedId { get; set; }
    }

}
