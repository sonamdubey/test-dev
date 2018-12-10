IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddCarToSellInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddCarToSellInquiries]
GO

	-- =============================================
-- Author:		Afrose
-- Create date: <25th July 2016>
-- Description:	This will create an entry on SellInquiries and SellInquiriesDetails
-- =============================================
CREATE PROCEDURE [dbo].[AddCarToSellInquiries]
    @DealerId NUMERIC,
	@SellerType INT,
    @CarVersionId NUMERIC,
	@CarRegNo VARCHAR(30),
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
    @AdditionalFuelType VARCHAR(30),
	@TC_StockId NUMERIC,
	@CertificationId SMALLINT,
	@ModifiedBy INT,
	@ModifiedDate DATETIME,
	@DescRating SMALLINT,
	@AlbumRating SMALLINT,
	@CalculatedEMI INT,
	@CertifiedLogoUrl VARCHAR(50),	
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

				 SELECT
				 @PackageType= P.InqPtCategoryId,
				 @PackageExpiryDate= CWCTMap.PackageEndDate,
				 @IsPremium=I.IsPremium
				 FROM CWCTDealerMapping CWCTMap WITH(NOLOCK) inner join Packages P WITH(NOLOCK) ON CWCTMap.PackageId=P.Id
				 inner join InquiryPointCategory I WITH(NOLOCK) ON P.InqPtCategoryId=I.Id  WHERE CWDealerID=@DealerId

				 SELECT @UsedCarMasterColorsId = UsedCarMasterColorsId FROM UsedCarMasterColors WITH(NOLOCK) WHERE LOWER(ColorName)=LOWER(@Color)

				INSERT INTO SellInquiries
				(
					 DealerId ,CarVersionId ,CarRegNo ,EntryDate ,StatusId ,Price ,MakeYear ,Kilometers ,Color ,ColorCode ,Comments ,IsArchived
					,ImportChecksum ,LastUpdated ,ViewCount ,PackageType ,PackageExpiryDate ,TC_StockId ,CertificationId
					,ModifiedBy	,ModifiedDate ,DescRating ,AlbumRating ,CalculatedEMI ,CertifiedLogoUrl ,IsPremium ,UsedCarMasterColorsId
					,EMI ,SourceId
				 )
				 VALUES
				(
					@DealerId ,@CarVersionId ,@CarRegNo	,@EntryDate ,1 ,@Price ,@MakeYear ,@Kilometers ,@Color ,@ColorCode ,@Comments ,@IsArchived
					,@ImportChecksum ,@LastUpdated ,NULL ,@PackageType,@PackageExpiryDate ,@TC_StockId ,@CertificationId
					,@ModifiedBy ,@ModifiedDate ,@DescRating ,@AlbumRating ,@CalculatedEMI ,@CertifiedLogoUrl ,@IsPremium ,@UsedCarMasterColorsId
					,@EMI ,@SourceId
				)

				DECLARE @RetValue INT
				SELECT @RetValue=SCOPE_IDENTITY();

				INSERT INTO SellInquiriesDetails
				  (SellInquiryId,Accidental,FloodAffected,AdditionalFuel,RegistrationPlace,InteriorColor,OneTimeTax,Insurance,InsuranceExpiry,CityMileage,CarDriven,
				 Owners)
				 VALUES
				 (
					@RetValue,@Accidental,@FloodAffected,@AdditionalFuelType,@RegistrationPlace,@InteriorColor,@OneTimeTax,@Insurance,@InsuranceExpiry,@CityMileage,
					@CarDriven,@OwnerType
				 )
				 -- Manual Update to trigger LiveListings table
				UPDATE SellInquiries SET LastUpdated=@LastUpdated where ID=@RetValue;
			END
	END


	
