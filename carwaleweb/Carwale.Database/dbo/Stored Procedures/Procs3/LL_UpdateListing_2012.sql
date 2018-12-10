IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LL_UpdateListing_2012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LL_UpdateListing_2012]
GO

	
CREATE PROCEDURE [dbo].[LL_UpdateListing_2012]
	@ProfileId		AS VARCHAR(50), 
	@SellerType		AS SMALLINT, 
	@Seller			AS VARCHAR(50), 
	@Inquiryid		AS NUMERIC, 
	@MakeId			AS NUMERIC, 
	@MakeName		AS VARCHAR(100), 
	@ModelId		AS NUMERIC, 
	@ModelName		AS VARCHAR(100),
	@VersionId		AS NUMERIC, 
	@VersionName	AS VARCHAR(100),
	@StateId		AS NUMERIC, 
	@StateName		AS VARCHAR(100),
	@CityId			AS NUMERIC, 
	@CityName		AS VARCHAR(100),
	@AreaId			AS NUMERIC, 
	@AreaName		AS VARCHAR(100),
	@Lattitude		AS DECIMAL(18,4), 
	@Longitude		AS DECIMAL(18,4), 
	@MakeYear		AS DATETIME, 
	@Price			AS NUMERIC, 
	@Kilometers		AS NUMERIC, 
	@Color			AS VARCHAR(100),
	@Comments		AS VARCHAR(500),
	@EntryDate		AS DATETIME, 
	@LastUpdated	AS DATETIME, 
	@PackageType	AS SMALLINT, 
	@ShowDetails	AS BIT, 
	@Priority		AS SMALLINT,	
	@CertificationId AS SMALLINT = null,
	@AdditionalFuel AS VARCHAR(50) = null,
	@EMI             AS BIGINT=null
AS
BEGIN



  	
	--first update the listing. if it is not there then insert the data
	-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
	UPDATE LiveListings SET
		MakeId = @MakeId,			MakeName = @MakeName,		ModelId = @ModelId,		ModelName = @ModelName, 
		VersionId = @VersionId,		VersionName = @VersionName, StateId = @StateId,		StateName = @StateName, 
		CityId = @CityId,			CityName = @CityName,		AreaId = @AreaId,		AreaName = @AreaName, 
		Lattitude = @Lattitude,		Longitude = @Longitude,		MakeYear = @MakeYear,	Price = @Price, 
		Kilometers = @Kilometers,	Color = @Color,				Comments = @Comments,	EntryDate = @EntryDate, 
		LastUpdated = @LastUpdated, PackageType = @PackageType, ShowDetails = @ShowDetails, Priority = @Priority,
		CertificationId = @CertificationId,
		AdditionalFuel=@AdditionalFuel,CalculatedEMI=@EMI
	WHERE
		ProfileId = @ProfileId

	IF @@ROWCOUNT = 0
	BEGIN
		--since the record is not there. hence add the data
		-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
		INSERT INTO LiveListings 
		(
			ProfileId,	SellerType,	Seller,		Inquiryid,
			MakeId,		MakeName,	ModelId,	ModelName,	VersionId,		VersionName,	StateId, 
			StateName,	CityId,		CityName,	AreaId,		AreaName,		Lattitude,		Longitude, 
			MakeYear,	Price,		Kilometers, Color,		Comments,		EntryDate,		LastUpdated, 
			PackageType, ShowDetails, Priority, CertificationId, AdditionalFuel,CalculatedEMI
		)
		VALUES
		(
			@ProfileId, @SellerType,@Seller,	@Inquiryid,
			@MakeId,	@MakeName,	@ModelId,	@ModelName,	@VersionId,		@VersionName,	@StateId, 
			@StateName,	@CityId,	@CityName,	@AreaId,	@AreaName,		@Lattitude,		@Longitude, 
			@MakeYear,	@Price,		@Kilometers, @Color,	@Comments,		@EntryDate,		@LastUpdated, 
			@PackageType, @ShowDetails, @Priority, @CertificationId,@AdditionalFuel,@EMI
		)
	END
	
	-- CHECK IF PHOTOS OF JUST INSERTED CAR IS AVAILABLE. ITS POSSIBLE WHEN YOU SUSPEND THE DEALER.
	DECLARE @PhotoCount INT
	SELECT @PhotoCount = COUNT(Id) FROM CarPhotos WHERE InquiryId = @Inquiryid AND IsDealer = (CASE WHEN @SellerType = 1 THEN 1 ELSE 0 END) AND IsActive = 1 AND IsApproved = 1
	
	IF @PhotoCount > 0
	BEGIN
		UPDATE LiveListings 
		SET PhotoCount = @PhotoCount 
		WHERE Inquiryid = @InquiryId and SellerType = SellerType
		
		UPDATE L  SET L.FrontImagePath = C.DirectoryPath + C.ImageUrlThumbSmall FROM LiveListings L INNER JOIN CarPhotos C
		ON L.Inquiryid = C.InquiryId AND C.IsDealer = (CASE WHEN @SellerType = 1 THEN 1 ELSE 0 END)
		WHERE C.IsMain = 1 AND C.InquiryId = @InquiryId AND IsActive =1 and IsApproved = 1		
	END
END



