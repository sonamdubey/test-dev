using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarDetails
{
	/// <summary>
	/// Created By : Sadhana on 27 May 2015
	/// Summary : entity for basic car details
	/// </summary>
	[Serializable]
	public class BasicCarInfo
	{
		public string ProfileId { get; set; }
		public uint InquiryId { get; set; }
		public int TCStockId { get; set; }

		public string Price { get; set; }
		public string PriceFormatted { get; set; }
		public string Color { get; set; }
		public string InteriorColor { get; set; }
		public string Kilometers { get; set; }

		public string FuelName { get; set; }
		public ushort FuelType { get; set; }

		public string TransmissionType { get; set; }
		public ushort TransmissionTypeId { get; set; }

		public string SellerType { get; set; }
		public ushort SellerId { get; set; }

		public string Insurance { get; set; }
		public string InsuranceExpiry { get; set; }
		public string LifeTimeTax { get; set; }
		public bool ShowInsuranceLink { get; set; }

		public string AdditionalFuel { get; set; }
		public string FuelEconomy { get; set; }

		public string NoOfOwners { get; set; }
		public short? OwnerNumber { get; set; }
		public int PhotoCount { get; set; }
		public int VideoCount { get; set; }
		public string VideoURL { get; set; }

		public string SellerContact { get; set; }
		public string SellerName { get; set; }

		public DateTime MakeYear { get; set; }
		public int MakeYearFormatted { get; set; }
		public string MakeMonthFormatted { get; set; }
		public int MakeMonth { get; set; }
		public string LastUpdatedDate { get; set; }

		public string MakeName { get; set; }
		public string MakeNameFormatted { get; set; }
		public uint MakeId { get; set; }

		public string RootName { get; set; }
		public uint RootId { get; set; }

		public string ModelName { get; set; }
		public string ModelNameFormatted { get; set; }
		public uint ModelId { get; set; }

		public string VersionName { get; set; }
		public uint VersionId { get; set; }

		public string CityName { get; set; }
		public string CityNameFormatted { get; set; }
		public uint CityId { get; set; }
        public string CityMaskingName { get; set; }

		public string AreaName { get; set; }
		public uint AreaId { get; set; }

		public string RegisterCity { get; set; }
		public string RegistrationNumber { get; set; }

		public string MaskingName { get; set; }
		public string CarAvailbaleAt { get; set; }
		public bool IsPremium { get; set; }

		public string VersionSubSegmentID { get; set; }
		public string BodyStyleId { get; set; }
		public string PriceNumeric { get; set; }
		public string KmNumeric { get; set; }
		public ClassifiedStockSource StockSource { get; set; }

		public string StockRecommendationUrl { get; set; }
		public string SimilarCarsUrl { get; set; }

		public string Comments { get; set; }
		public bool IsNew { get; set; }
		public bool IsNewCarDealerAvailable { get; set; }
		public string EntryDate { get; set; }
		public bool ShareToCT { get; set; }
		public string Warranties { get; set; }
		public CarRegistrationType RegType { get; set; }
		public string CertificationId { get; set; }
		public decimal? CertificationScore { get; set; }
        public bool IsChatAvailable { get; set; }
        public CwBasePackageId CwBasePackageId { get; set; }
        public int CtePackageId { get; set; }
    }
}
