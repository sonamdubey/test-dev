using System;

namespace Carwale.Entity
{
    //public class CarModelMaskingResponse
    //{
    //    public string MaskingName { get; set; }
    //    public string MakeName { get; set; }
    //    public int ModelId { get; set; }
    //    public int MakeId { get; set; }
    //    public bool Redirect { get; set; }
    //}

    public class CarModelEntity : MakeModelEntity
    {
        public bool Used { get; set; }
        public bool New { get; set; }
        public bool Indian { get; set; }
        public bool Imported { get; set; }
        public bool Futuristic { get; set; }
        public bool Classic { get; set; }
        public bool Modified { get; set; }             
        public bool IsDeleted{ get;set; }       
        public decimal ReviewRate{ get;set; }
        public int ReviewCount{ get;set; }
        public decimal Looks{ get;set; }
        public decimal Performance{ get;set; }
        public decimal Comfort{ get;set; }
        public decimal ValueForMoney{ get;set; }
        public decimal FuelEconomy{ get;set; }
        public string SmallPic{ get;set; }
        public string LargePic{ get;set; }
        public DateTime MoCreatedOn{ get;set; }
        public string MoUpdatedBy{ get;set; }
        public DateTime MoUpdatedOn{ get;set; }
        public bool IsReplicated{ get;set; }
        public string HostURL{ get;set; }
        public string comment{ get;set; }
        public DateTime Discontinuation_date{ get;set; }
        public int ReplacedByModelId{ get;set; }
        public int DiscontinuationId{ get;set; }
        public int MinPrice{ get;set; }
        public int MaxPrice{ get;set; }
        public int CarVersionID_Top{ get;set; }
        public int SubSegmentID{ get;set; }
        public string Summary{ get;set; }
        public string MaskingName{ get;set; }
        public Int16 RootId{ get;set; }
        public string Platform{ get;set; }
        public Int16 Generation{ get;set; }
        public Int16 Upgrade{ get;set; }
        public float UsedCarRating{ get;set; }
        public DateTime ModelLaunchDate { get; set; }
        public string PageUrl { get; set; } // url :  /<Make name>-cars/masking name/
        public string PageUrl1 { get; set;}
    }
}