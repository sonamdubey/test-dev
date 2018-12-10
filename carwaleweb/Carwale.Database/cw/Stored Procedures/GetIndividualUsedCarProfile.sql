IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetIndividualUsedCarProfile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetIndividualUsedCarProfile]
GO

	
-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <30/07/2012>
-- Description:	<Returns details required in the individual's used car profile>
-- Modified By : Ashish G. Kamble on 24 Apr 2013
-- Modified : CustomerFavouritesUsed CFU. Flag isActive = 1 is checked
--Modified By Reshma Shetty 14/05/2013 [NewCarSpecifications table no longer in use so data fetched from CarVersions table] 
/*Modified By Reshma Shetty 14/05/2013 @StateCode,@LogoUrl has been assigned NULL and 
  data are being fetched from Livelisting table instead of individual master tables.
  Join from Cities,States,Areas,CarModels,CarMakes has been removed*/
  -- Avishkar 17-5-2013 Modified to set getdate() in @LastUpdated
  -- Reshma 20-5-2013 modified  to set  in @LastUpdated
  --Modified By Satish Sharma 20/05/2013 to update view count
  --Modified By Akansha 13.08.2013 Added a new Parameter @IsSpecsExist
  -- Modified By : Ashish Kamble on 21 Nov 2013 -- Added Youtube video source
  -- Modified By : Akansha Srivastava on 12.2.2014
  -- Description : Added MaskingName Column
  -- Modified By : Akansha Srivastava on 9.5.2014
  -- Description : Comments value now getting retrieved from Livelisting table
  --Modified By Akansha 19.06.2014 Added a new Parameter @BodyTypeId
-- =============================================
CREATE PROCEDURE [cw].[GetIndividualUsedCarProfile] 

	-- Add the parameters for the stored procedure here
    @InquiryID			NUMERIC(18, 0)		  ,
    @CustomerId         NUMERIC(18, 0)= NUll  ,
	@MakeID				NUMERIC(18, 0)	OUTPUT, 
	@ModelID			NUMERIC(18, 0)	OUTPUT, 
	@VersionID			NUMERIC(18, 0)	OUTPUT,
	@Make				VARCHAR(30)		OUTPUT, 
	@LogoUrl			VARCHAR(50)	OUTPUT, 
	@Model				VARCHAR(30)		OUTPUT,
	@MaskingName		VARCHAR(30)		OUTPUT,
	@Version			VARCHAR(50)		OUTPUT, 
	@CarLargePicUrl		VARCHAR(10)	OUTPUT,
	@CarFuelType		TINYINT			OUTPUT, 
	@Price				DECIMAL(18, 0)	OUTPUT,
    @Kilometers			NUMERIC(18, 0)	OUTPUT,
    @MakeYear			DATETIME		OUTPUT,
    @Color				VARCHAR(100)	OUTPUT, 
    @ColorCode			VARCHAR(6)		OUTPUT,
	@Comments			VARCHAR(500)	OUTPUT,
	@Area               VARCHAR(50)		OUTPUT,
	@City				VARCHAR(30)		OUTPUT,
	@State				VARCHAR(30)		OUTPUT,
	@StateCode			VARCHAR(2)		OUTPUT,
    @LastUpdated		DATETIME		OUTPUT,
    @Accessories		VARCHAR(500)	OUTPUT,
    @Owners				NUMERIC(10,0)		OUTPUT,
    @Insurance			VARCHAR(50)		OUTPUT,
    @InsuranceExpiry	DATETIME		OUTPUT,
	@Tax				VARCHAR(50)		OUTPUT, 
	@RegistrationPlace	VARCHAR(50)		OUTPUT,
	@StatusID			SMALLINT	OUTPUT, 
	@InteriorColor		VARCHAR(50)		OUTPUT,
	@InteriorColorCode  VARCHAR(6)		OUTPUT, 
	@CityMileage		VARCHAR(50)		OUTPUT, 
	@AdditionalFuel		VARCHAR(50)		OUTPUT, 
	@CarDriven			VARCHAR(50)		OUTPUT,
	@Accidental			BIT				OUTPUT, 
	@FloodAffected		BIT				OUTPUT, 
	@Warranties			VARCHAR(500)	OUTPUT, 
	@Modifications		VARCHAR(500)		OUTPUT, 
	@BatteryCondition	VARCHAR(50)		OUTPUT, 
	@BrakesCondition	VARCHAR(50)		OUTPUT, 
	@ElectricalsCondition VARCHAR(50)	OUTPUT, 
	@EngineCondition	VARCHAR(50)		OUTPUT, 
	@ExteriorCondition	VARCHAR(50)		OUTPUT, 
	@SeatsCondition		VARCHAR(50)		OUTPUT, 
	@SuspensionsCondition VARCHAR(50)	OUTPUT, 
	@TyresCondition		VARCHAR(50)		OUTPUT, 
	@OverallCondition	VARCHAR(50)		OUTPUT, 
	@SellerID			NUMERIC(18, 0)	OUTPUT, 
	@ExpiryDate			DATETIME	    OUTPUT, 
	@CarCityID			NUMERIC(18, 0)	OUTPUT, 
	@IsFake				BIT				OUTPUT,
	@IsCarFake			BIT			    OUTPUT,
	@PackageExpiryDate	DATETIME		OUTPUT,
	@isDealer			BIT			    OUTPUT, 
	@PackageType		SMALLINT		OUTPUT, 
	@NoOfCylinders		SMALLINT		OUTPUT, 
	@ValueMechanism		VARCHAR(200)	OUTPUT, 
	@TransmissionType	VARCHAR(50)		OUTPUT, 
	@FuelType			VARCHAR(50)		OUTPUT, 
    @Features_SafetySecurity VARCHAR(200)OUTPUT, 
    @Features_Comfort	VARCHAR(200)	OUTPUT, 
    @Features_Others	VARCHAR(200)	OUTPUT, 
	@InteriorCondition	VARCHAR(50)		OUTPUT, 
	@ACCondition		VARCHAR(50)		OUTPUT, 
	@CertificationID	SMALLINT		OUTPUT,
	@CalculatedEMI		BIGINT		OUTPUT, -- Its going to be zero for individual sellers
	@ReasonForSelling	VARCHAR(500) OUTPUT,
	@IsBookMarked       BIT OUTPUT,
	@CertifiedLogoUrl   VARCHAR(200) OUTPUT,
	@IsSpecsExist TinyInt OUTPUT,
	@YoutubeVideoSrc VARCHAR(20) OUTPUT,
	@IsYouTubeVideoApproved BIT OUTPUT,
	@IsPremium BIT OUTPUT,
	@BodyTypeId Int Output,
	@RootId Numeric(18,0) Output,
	@RootName varchar(200) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Basic assumption is listing is not premium
	SET @IsPremium = 0

    -- Insert statements for procedure here
SELECT @InquiryID=CSI.ID, @MakeId=LL.MakeId , @ModelId=LL.ModelId , @VersionId=CV.ID ,
	@Make=LL.MakeName , @LogoUrl=NULL , @Model=LL.ModelName ,
	@MaskingName= CMO.MaskingName,
	@Version=CV.Name , @CarLargePicUrl=CV.largePic , @CarFuelType=CV.CarFuelType, @Price=CSI.Price ,
	@Kilometers=CSI.Kilometers , @MakeYear=CSI.MakeYear , @Color=CSI.Color, @ColorCode=CSI.ColorCode,
	@Comments=LL.Comments ,@Area=ISNULL(LL.AreaName,' '), @City=LL.CityName , @State=LL.StateName , @StateCode=NULL,
	--@LastUpdated=GETDATE(),   -- Avishkar 17-5-2013 Modified to set getdate() in @LastUpdated
	@LastUpdated=LL.lastupdated,  -- Reshma 20-5-2013 modified  to set  in @LastUpdated
	--@LastUpdated=DateAdd(D, -30, CSI.ClassifiedExpiryDate),
	@Accessories=SD.Accessories, @Owners=SD.Owners, @Insurance=SD.Insurance, 
	@InsuranceExpiry=SD.InsuranceExpiry,@Tax=SD.Tax , @RegistrationPlace=SD.RegistrationPlace, 
	@StatusID=CSI.StatusId, @InteriorColor=SD.InteriorColor , @InteriorColorCode=SD.InteriorColorCode , 
	@CityMileage=SD.CityMileage , @AdditionalFuel=SD.AdditionalFuel, @CarDriven=SD.CarDriven, @Accidental=SD.Accidental, 
	@FloodAffected=SD.FloodAffected, 
	@Warranties=SD.Warranties, @Modifications=SD.Modifications, @BatteryCondition=SD.BatteryCondition, 
	@BrakesCondition=SD.BrakesCondition, @ElectricalsCondition=SD.ElectricalsCondition, 
	@EngineCondition=SD.EngineCondition, @ExteriorCondition=SD.ExteriorCondition, 
	@SeatsCondition=SD.SeatsCondition, @SuspensionsCondition=SD.SuspensionsCondition, 
	@TyresCondition=SD.TyresCondition, @OverallCondition=SD.OverallCondition, 
	@SellerID=Ds.ID ,@ExpiryDate= CSI.ClassifiedExpiryDate, @CarCityId=CSI.CityId , 
	@IsFake=Ds.IsFake ,@IsCarFake=CSI.IsFake, @PackageExpiryDate=CSI.PackageExpiryDate,@isDealer=0,  @PackageType=CSI.PackageType, 
	@NoOfCylinders=1, @ValueMechanism=NULL,--Modified By Reshma Shetty 14/05/2013 [Not being used in the front end so set as NULL] 
	@TransmissionType=CR.Descr,@FuelType=CFT.FuelType, --Modified By Reshma Shetty 14/05/2013 [NewCarSpecifications table no longer in use so data fetched from CarVersions table] 
	@Features_SafetySecurity=SD.Features_SafetySecurity, @Features_Comfort=SD.Features_Comfort, @Features_Others=SD.Features_Others, 
	@InteriorCondition=SD.InteriorCondition, @ACCondition=SD.ACCondition, @CertificationID=null,@CalculatedEMI = 0,
	@ReasonForSelling = CSI.ReasonForSelling,
	@IsBookMarked = case when @CustomerId is not null and COUNT(CFU.Id)Over(PARTITION BY CFU.CustomerId)=1 then 1 else 0 end,
	@CertifiedLogoUrl=LL.CertifiedLogoUrl,@IsSpecsExist=CV.IsSpecsExist, @YoutubeVideoSrc = SD.YoutubeVideo, @IsYouTubeVideoApproved = SD.IsYouTubeVideoApproved, 
	@IsPremium = LL.IsPremium,@BodyTypeId = CV.BodyStyleId, @RootId = LL.RootId, @RootName = CMR.RootName
FROM CustomerSellInquiries AS CSI WITH(NOLOCK)
 INNER JOIN LiveListings LL WITH(NOLOCK) ON LL.ProfileId = 'S' + CONVERT(VARCHAR(15), CSI.ID)
 INNER JOIN CustomerSellInquiryDetails SD WITH(NOLOCK) ON CSI.ID = SD.InquiryId
 INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CSI.CarVersionId = CV.ID
 INNER JOIN Customers AS Ds WITH(NOLOCK) ON CSI.CustomerId = Ds.Id
 LEFT JOIN CarFuelType CFT WITH(NOLOCK) ON CFT.FuelTypeId=CV.CarFuelType
 LEFT JOIN CarTransmission CR WITH(NOLOCK)  ON CR.Id=CV.CarTransmission
 LEFT jOIN CustomerFavouritesUsed CFU  WITH(NOLOCK) ON CFU.CarProfileId=	LL.ProfileId AND CFU.IsActive = 1
 AND (@CustomerId IS NULL OR CFU.CustomerId=@CustomerId)
 Inner Join CarModels CMO on CV.CarModelId = CMO.ID
 Inner Join CarModelRoots CMR on CMO.RootId = CMR.RootId
WHERE CSI.ID = @InquiryID

--Modified By Satish Sharma 20/05/2013 to update view count
-- Update car view count
IF @MakeId > 0
BEGIN 
	UPDATE CustomerSellInquiries SET ViewCount = IsNull(ViewCount, 0)+1 WHERE ID = @InquiryID 
END

END






