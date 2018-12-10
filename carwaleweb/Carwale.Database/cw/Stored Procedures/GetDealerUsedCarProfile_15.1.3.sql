IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetDealerUsedCarProfile_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetDealerUsedCarProfile_15]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <30/07/2012>
-- Description:	<Returns details required in the dealer's used car profile>
--                EXEC cw.GetDealerUsedCarProfile 2
-- Modified By : Ashish G. Kamble on 24 Apr 2013
-- Modified : CustomerFavouritesUsed CFU. Flag isActive = 1 is checked
--Modified By Reshma Shetty 14/05/2013 [NewCarSpecifications table no longer in use so data fetched from CarVersions table]
/*Modified By Reshma Shetty 16/05/2013 @StateCode,@LogoUrl has been assigned NULL and 
  data are being fetched from Livelisting table instead of individual master tables.
  Join from Cities,States,Areas,CarModels,CarMakes has been removed*/ 
  --Modified By Akansha 13.08.2013 Added a new Parameter @IsSpecsExist
  --Modified By Akansha 19.06.2014 Added a new Parameter @BodyTypeId,@RootId,@RootName
  --Modified by Kirtan Shetty 29 October, Added Parameters and code for EMI option
  --Modified by Aditi Dhaybar 15 December 2014, Retrieve CityId from LiveListing 
  --Modified by Aditi Dhaybar on 19/12/14 for new ask the seller functionality
  --Modified by Kirtan on 22-Jan-2014 Added parameter @showemi
  --Modified by Kirtan on 27-Jan-2014 Added parameters  
  --Modified by Shikhar on Feb 2, 2015 remove the EMI condition
-- =============================================
CREATE PROCEDURE  [cw].[GetDealerUsedCarProfile_15.1.3]

	-- Add the parameters for the stored procedure here
	@InquiryID			NUMERIC(18, 0)		  ,
	@CustomerId         NUMERIC(18, 0)= NUll  ,  
	@MakeID				NUMERIC(18, 0)	OUTPUT, 
	@ModelID			NUMERIC(18, 0)	OUTPUT, 
	@VersionID			NUMERIC(18, 0)	OUTPUT,
	@Make				VARCHAR(30)		OUTPUT, 
	@LogoUrl			VARCHAR(50)	OUTPUT, 
	@Model				VARCHAR(30)		OUTPUT,
	@MaskingName		VARCHAR(30)		OUTPUT ,
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
    @Accessories		VARCHAR(2000)	OUTPUT,
    @Owners				VARCHAR(50)		OUTPUT,
    @Insurance			VARCHAR(50)		OUTPUT,
    @InsuranceExpiry	DATETIME		OUTPUT,
	@Tax				VARCHAR(50)		OUTPUT, 
	@RegistrationPlace	VARCHAR(50)		OUTPUT,
	@StatusID			NUMERIC(18, 0)	OUTPUT, 
	@InteriorColor		VARCHAR(50)		OUTPUT,
	@InteriorColorCode  VARCHAR(6)		OUTPUT, 
	@CityMileage		VARCHAR(50)		OUTPUT, 
	@AdditionalFuel		VARCHAR(50)		OUTPUT, 
	@CarDriven			VARCHAR(50)		OUTPUT,
	@Accidental			BIT				OUTPUT, 
	@FloodAffected		BIT				OUTPUT, 
	@Warranties			VARCHAR(500)	OUTPUT, 
	@Modifications		VARCHAR(500)	OUTPUT, 
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
	@CalculatedEMI		BIGINT		OUTPUT,
	@ReasonForSelling	VARCHAR(500) OUTPUT,
	@IsBookMarked       BIT OUTPUT,
	@CertifiedLogoUrl   VARCHAR(200) OUTPUT,
	@IsSpecsExist TinyInt Output,
	@YoutubeVideoSrc VARCHAR(20) OUTPUT,
	@IsYouTubeVideoApproved BIT OUTPUT,
	@IsPremium BIT OUTPUT,
	@BodyTypeId Int Output,
	@RootId Numeric(18,0) Output,
	@RootName varchar(200) OUTPUT,
	-- Added by kirtan shetty 29 October 2014
	@StockId NUMERIC OUTPUT, 
	@EMI INT OUTPUT,
	@LoanAmount INT OUTPUT,
	@InterestRate FLOAT OUTPUT,
	@Tenure SMALLINT OUTPUT,
	@OtherCharges INT OUTPUT,
	@DownPayment INT OUTPUT,
	@LoanToValue INT OUTPUT,
	@DealerCityId NUMERIC(18, 0) OUTPUT,		-- Added By Aditi Dhaybar on 19/12/14
	@ShowEmi BIT OUTPUT,						-- Added by Kirtan Shetty on 23 January 2015
	-- Added by Kirtan Shetty on 27 January 2015
	@AbsureCarScore INT OUTPUT,					
	@AbsureId INT OUTPUT,						
	@AbsureCertificateUrl VARCHAR(100) OUTPUT,			
	@DealerCertificateId INT OUTPUT,					
	@FinalWarrantyType VARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
  
	SELECT @InquiryID=SI.ID, @MakeId=LL.MakeId , @ModelId=LL.ModelId , @VersionId=LL.VersionId ,
		@Make=LL.MakeName , @LogoUrl=NULL , @Model=LL.ModelName ,
		@MaskingName=CMO.MaskingName,
		@Version=CV.Name , @CarLargePicUrl=CV.largePic , @CarFuelType=CV.CarFuelType, @Price=SI.Price ,
		@Kilometers=SI.Kilometers , @MakeYear=SI.MakeYear , @Color=SI.Color, @ColorCode=SI.ColorCode,
		@Comments=LL.Comments ,@Area=ISNULL(LL.AreaName,' '), @City=LL.CityName , @State=LL.StateName , @StateCode=NULL,
		@LastUpdated=SI.LastUpdated, @Accessories=SA.Accessories, @Owners=SD.Owners, @Insurance=SD.Insurance, 
		@InsuranceExpiry=SD.InsuranceExpiry,@Tax=SD.OneTimeTax , @RegistrationPlace=SD.RegistrationPlace, 
		@StatusID=SI.StatusId, @InteriorColor=SD.InteriorColor , @InteriorColorCode=SD.InteriorColorCode , 
		@CityMileage=SD.CityMileage , @AdditionalFuel=SD.AdditionalFuel, @CarDriven=SD.CarDriven, @Accidental=SD.Accidental, 
		@FloodAffected=SD.FloodAffected, 
		@Warranties=SD.Warranties, @Modifications=SD.Modifications, @BatteryCondition=SD.BatteryCondition, 
		@BrakesCondition=SD.BrakesCondition, @ElectricalsCondition=SD.ElectricalsCondition, 
		@EngineCondition=SD.EngineCondition, @ExteriorCondition=SD.ExteriorCondition, 
		@SeatsCondition=SD.SeatsCondition, @SuspensionsCondition=SD.SuspensionsCondition, 
		@TyresCondition=SD.TyresCondition, @OverallCondition=SD.OverallCondition, 
		@SellerID=Ds.ID ,@ExpiryDate=NULL, @CarCityId=LL.CityId , @DealerCityId=Ds.CityId, --Modified by Aditi Dhaybar on 19/12/14 for new ask the seller functionality
		@IsFake=Ds.Status ,@IsCarFake=0, @PackageExpiryDate=SI.PackageExpiryDate,@isDealer=1,  @PackageType=Si.PackageType, 
		@NoOfCylinders=NULL, @ValueMechanism=NULL,--Modified By Reshma Shetty 14/05/2013 [Not being used in the front end so set as NULL]
		@TransmissionType=CR.Descr,@FuelType=CFT.FuelType,--Modified By Reshma Shetty 14/05/2013 [NewCarSpecifications table no longer in use so data fetched from CarVersions table] 
		@Features_SafetySecurity=SD.Features_SafetySecurity, @Features_Comfort=SD.Features_Comfort, @Features_Others=SD.Features_Others, 
		@InteriorCondition=SD.InteriorCondition, @ACCondition=SD.ACCondition, @CertificationID=SI.CertificationId,
		@CalculatedEMI = ISNull(LL.CalculatedEMI, 0), @ReasonForSelling = '',
		@IsBookMarked = case when @CustomerId is not null and COUNT(CFU.Id)Over(PARTITION BY CustomerId)=1 then 1 else 0 end ,
		@CertifiedLogoUrl=LL.CertifiedLogoUrl, @IsSpecsExist = CV.IsSpecsExist,
		@YoutubeVideoSrc  = SD.YoutubeVideo , @IsYouTubeVideoApproved = SD.IsYouTubeVideoApproved,
		@IsPremium = LL.IsPremium,@BodyTypeId = CV.BodyStyleId, @RootId = LL.RootId, @RootName = CMR.RootName, @StockId = SI.TC_StockId
	FROM SellInquiries AS SI WITH(NOLOCK)	 
		INNER JOIN SellInquiriesDetails SD WITH(NOLOCK) 
			ON SI.Id = SD.SellInquiryId
		INNER JOIN LiveListings LL WITH(NOLOCK) 
			ON LL.ProfileId = 'D' + CONVERT(VARCHAR(15), SI.ID)
		INNER JOIN SellInquiryAccessories SA WITH(NOLOCK) 
			ON SI.Id = SA.CarId
		INNER JOIN CarVersions AS CV WITH(NOLOCK) 
			ON SI.CarVersionId = CV.ID
		INNER JOIN Dealers AS Ds WITH(NOLOCK) 
			ON SI.DealerId = Ds.ID
		LEFT jOIN CarFuelType CFT WITH(NOLOCK)  
			ON CFT.FuelTypeId=CV.CarFuelType
		LEFT jOIN CarTransmission CR WITH(NOLOCK) 
			ON CR.Id=CV.CarTransmission 
		LEFT jOIN CustomerFavouritesUsed CFU WITH(NOLOCK) 
			ON CFU.CarProfileId =	'D' + CONVERT(VARCHAR(15), SI.ID) AND CFU.IsActive = 1
			AND (@CustomerId IS NULL OR CFU.CustomerId=@CustomerId) 				
		INNER Join CarModels CMO WITH(NOLOCK) 
			ON CV.CarModelId = CMO.ID
		INNER Join CarModelRoots CMR WITH(NOLOCK) 
			ON CMO.RootId = CMR.RootId
	WHERE 
		SI.ID = @InquiryID 

-- added by Kirtan Shetty 29 October 2014
-- For Getting the Data Related to EMI options
	SELECT 
		@EMI = TCST.EMI
		,@LoanAmount = TCST.LoanAmount
		,@InterestRate = TCST.InterestRate
		,@Tenure = TCST.Tenure
		,@OtherCharges = TCST.OtherCharges 
		,@LoanToValue = TCST.LoanToValue
		,@DownPayment = (@Price - TCST.LoanAmount)
		,@ShowEmi = TCST.ShowEMIOnCarwale
		-- Added by Kirtan Shetty on Jan 27
		,@AbsureCarScore = ABC.CarScore																
		,@AbsureId = ABC.Id																			
		,@AbsureCertificateUrl = 'http://www.autobiz.in/absure/CarCertificate.aspx?carId=' + CONVERT(VARCHAR, ABC.Id)
		,@DealerCertificateId = TCST.CertificationId												
		,@FinalWarrantyType = ABW.Warranty											
	FROM TC_Stock TCST WITH(NOLOCK) 
		LEFT JOIN AbSure_CarDetails ABC 
			ON ABC.StockId = TCST.Id 
		LEFT JOIN AbSure_WarrantyTypes ABW 
			ON ABC.FinalWarrantyTypeId = ABW.AbSure_WarrantyTypesId
	WHERE -- Removed the condition to show EMI value by Shikhar on Feb 2, 2015
		TCST.Id = @StockId

-- Update car view count
IF @MakeId > 0
BEGIN 
	UPDATE SellInquiries SET ViewCount = ISNULL(ViewCount, 0)+1 WHERE ID = @InquiryID 
END

END


