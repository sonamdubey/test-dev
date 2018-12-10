IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCarToSellInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCarToSellInquiries]
GO

	-- =============================================
-- Author:		Afrose
-- Create date: <25th July 2016>
-- Description:	This will update SellInquiries and SellInquiriesDetails
-- Modified by Sahil Sharma. To change Comments input parameter from length 50 to 500 as it is in Create
-- =============================================

CREATE PROCEDURE [dbo].[UpdateCarToSellInquiries]
    @DealerId NUMERIC,
	@SellerType INT,
    @CarVersionId NUMERIC,
	@CarRegNo VARCHAR(50),
	@EntryDate DATETIME,
	@Price INT,
	@MakeYear DATETIME,
	@Kilometers NUMERIC,
	@Color VARCHAR(100),
	@ColorCode INT,
	@OwnerType INT,
	@Comments VARCHAR(500),
	@IsArchived BIT,
	@ImportChecksum NUMERIC,
	@LastUpdated DATETIME,
	@ViewCount NUMERIC,	
	@TC_StockId NUMERIC,
	@CertificationId SMALLINT,
	@ModifiedBy INT,
	@ModifiedDate DATETIME,
	@DescRating SMALLINT,
	@AlbumRating SMALLINT,
	@CalculatedEMI INT,
	@CertifiedLogoUrl VARCHAR(50),	
	@AdditionalFuelType VARCHAR(30),
	@EMI NUMERIC,
	@SourceId TINYINT,
	--extra
	@RegistrationPlace VARCHAR(50),
	@InteriorColor VARCHAR(50),
	@OneTimeTax VARCHAR(50),
	@Insurance VARCHAR(50),
	@InsuranceExpiry DATETIME,
	@CityMileage INT,
	@CarDriven VARCHAR(50),
	@Accidental BIT,
	@FloodAffected BIT
	
AS			
BEGIN	
	IF(@SellerType=1)
	BEGIN
	     DECLARE @PackageType INT
		 DECLARE @PackageExpiryDate DATETIME
		 DECLARE @IsPremium BIT
	     DECLARE @UsedCarMasterColorsId SMALLINT

		 SELECT TOP 1 
				 @PackageType= P.InqPtCategoryId,
				 @PackageExpiryDate= CWCTMap.PackageEndDate,
				 @IsPremium=I.IsPremium
				 FROM CWCTDealerMapping CWCTMap WITH(NOLOCK) inner join Packages P WITH(NOLOCK) ON CWCTMap.PackageId=P.Id
				 inner join InquiryPointCategory I WITH(NOLOCK) ON P.InqPtCategoryId=I.Id  WHERE CWDealerID=@DealerId

		 SELECT @UsedCarMasterColorsId = UsedCarMasterColorsId FROM UsedCarMasterColors WITH(NOLOCK) WHERE LOWER(ColorName)=LOWER(@Color)

	    UPDATE SellInquiriesDetails
		SET 
		Owners = @OwnerType,
		AdditionalFuel=@AdditionalFuelType,
		RegistrationPlace=@RegistrationPlace,
		InteriorColor=@InteriorColor,
		OneTimeTax=@OneTimeTax,
		Insurance=@Insurance,
		InsuranceExpiry=@InsuranceExpiry,
		CityMileage=@CityMileage,
		CarDriven=@CarDriven,
		Accidental=@Accidental,
		FloodAffected=@FloodAffected
		WHERE SellInquiryId=(SELECT TOP 1 ID FROM SellInquiries WITH(NOLOCK) WHERE TC_StockId=@TC_StockId AND SourceId=@SourceId and DealerId=@DealerId and StatusId=1)

	     UPDATE SellInquiries
		 SET		             
		 CarVersionId=@CarVersionId
		 ,CarRegNo=@CarRegNo		
		 ,Price=@Price
		 ,MakeYear=@MakeYear
		 ,Kilometers=@Kilometers
		 ,Color=@Color
		 ,ColorCode=@ColorCode
		 ,Comments=@Comments
		 ,IsArchived=@IsArchived
		 ,ImportChecksum=@ImportChecksum
		 ,LastUpdated=@LastUpdated		
		 ,CertificationId=@CertificationId
		 ,ModifiedBy=@ModifiedBy
		 ,ModifiedDate=@ModifiedDate
		 ,DescRating=@DescRating
		 ,AlbumRating=@AlbumRating
		 ,CalculatedEMI=@CalculatedEMI
		 ,CertifiedLogoUrl=@CertifiedLogoUrl
		 ,IsPremium=@IsPremium
		 ,UsedCarMasterColorsId=@UsedCarMasterColorsId
		 ,EMI=@EMI
		 WHERE TC_StockId=@TC_StockId AND SourceId=@SourceId and DealerId=@DealerId and StatusId=1
		 		 
	END
   
END

