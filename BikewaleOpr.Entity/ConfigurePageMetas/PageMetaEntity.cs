using System;

namespace BikewaleOpr.Entity.ConfigurePageMetas
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 14-Aug-2017
    /// Summary: Entity for page metas 
    /// </summary>
    public class PageMetaEntity
    {
        public uint Id { get; set; }
        public uint PageId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ushort Enterdby { get; set; }
        public ushort UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}