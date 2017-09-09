﻿namespace Bikewale.Models
{
    public class PoupCityAreaVM : ModelBase
    {
        public uint ModelId { get; set; }

        public string MakeName { get; set; }

        public string ModelName { get; set; }

        public bool IsPersistent { get; set; }

        public uint PQSourceId { get; set; }

        public uint PageCategoryId { get; set; }

        public uint PreSelectedCity { get; set; }

        public bool IsReload { get; set; }

        public string Url { get; set; }


    }
}
