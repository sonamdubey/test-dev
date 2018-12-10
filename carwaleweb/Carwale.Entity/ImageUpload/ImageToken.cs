using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ImageUpload
{
    public class ImageToken : Token
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public bool ServerError { get; set; }
        public uint PhotoId { get; set; }
        public string OriginalImgPath { get; set; }
    }
}
