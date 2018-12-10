IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CarDetailsInitialize_V_16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CarDetailsInitialize_V_16_11_1]
GO

	-- =============================================  
-- Author:  <Nilesh Utture>  
-- Create date: <01st November, 2012>  
-- Description: <Gives All Details of cars in stock> 
-- Modified By : Tejashree Patil on 3 Jan 2012 at 6.30pm, Fetched CertificationId and CustomerBenefits 
-- Modified By Vivek Gupta on 17-12-2013, Added a a variable @YouTubeVideoLink and fetched it.
-- Modified By vivek Gupta on 23-12-2013, added queries to get youtube link
-- Modified By vivek Gupta on 6th jan,2014, commented previous query and added new query for youtubelink.
-- Modified By : Vivek Gupta on 17-12-2014, added join with CarfuelType to fetch FuelType
-- Modified By : Vivek Gupta on 12-08-2015 added originalimgpath to return in table
-- Modified By : Chetan Navin on 14-10-2016 added query to fetch photos
-- =============================================  
CREATE PROCEDURE [dbo].[CarDetailsInitialize_V_16_11_1]
	-- Add the parameters for the stored procedure here  
	@StockId BIGINT
	,@BranchId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT ON;

	-- Modified By : Tejashree Patil on 3 Jan 2012 at 6.30pm
	DECLARE @ValueAdditions VARCHAR(500)
	-- Modified By Vivek Gupta on 17-12-2013
	-- Modified By vivek Gupta on 23-12-2013
	DECLARE @YouTubeVideoLink VARCHAR(200)

	-- Modified By vivek Gupta on 6th jan,2014, commented previous query and added new query for youtubelink.
	--SELECT @YouTubeVideoLink = YoutubeVideo FROM SellInquiriesDetails WITH(NOLOCK) WHERE SellInquiryId =  (SELECT SI.ID FROM SellInquiries SI WITH(NOLOCK) WHERE SI.TC_StockId = @StockId)
	SELECT @YouTubeVideoLink = VideoUrl
	FROM TC_CarVideos WITH (NOLOCK)
	WHERE StockId = @StockId
		AND IsActive = 1

	IF EXISTS (
			SELECT TOP 1 *
			FROM TC_StockCarValueAdditions WITH (NOLOCK)
			WHERE TC_StockId = @StockId
			)
	BEGIN
		SELECT @ValueAdditions = COALESCE(@ValueAdditions + '|', '') + CVA.ValueAddName
		FROM TC_StockCarValueAdditions SCVA WITH (NOLOCK)
		INNER JOIN TC_CarValueAdditions CVA WITH (NOLOCK) ON CVA.TC_CarValueAdditionsId = SCVA.TC_CarValueAdditionsId
		WHERE SCVA.TC_StockId = @StockId
	END

	-- Insert statements for procedure here  
	SELECT TSt.ID
		,TSt.IsBooked
		,CM.ID AS MakeId
		,CMO.ID AS ModelId
		,CV.ID AS VersionId
		,CM.NAME AS Make
		,CM.LogoUrl AS LogoUrl
		,CMO.NAME AS Model
		,CV.NAME AS Version
		,CV.largePic AS CarLargePicUrl
		,TSt.Price AS Price
		,TSt.StatusId
		,TSt.Kms AS Kilometers
		,TSt.MakeYear AS MakeYear
		,TSt.Colour
		,TSt.LastUpdatedDate
		,TSt.RegNo
		,TS.STATUS
		,IsNull(CP.ImageUrlThumb, '') AS ImageUrlThumb
		,IsNull(CP.ImageUrlFull, '') AS ImageUrlFull
		,TSt.IsParkNSale
		--,SI.Id AS ProfileId
		,TCC.Owners
		,TCC.Insurance
		,TCC.InsuranceExpiry
		,TCC.OneTimeTax Tax
		,TCC.RegistrationPlace
		,TCC.InteriorColor
		,TCC.InteriorColorCode
		,TCC.CityMileage
		,TCC.AdditionalFuel
		,TCC.CarDriven
		,TCC.Accidental
		,TCC.FloodAffected
		,TCC.Warranties
		,TCC.Modifications
		,TCC.Comments
		,TCC.BatteryCondition
		,TCC.BrakesCondition
		,TCC.ElectricalsCondition
		,TCC.EngineCondition
		,TCC.ExteriorCondition
		,TCC.SeatsCondition
		,TCC.SuspensionsCondition
		,TCC.TyresCondition
		,TCC.OverallCondition
		,TCC.Features_SafetySecurity
		,TCC.Features_Comfort
		,TCC.Features_Others
		,TCC.InteriorCondition
		,TCC.ACCondition
		,D.ContactEmail
		,D.Organization
		,D.MobileNo
		,D.FaxNo
		,D.WebsiteUrl
		,CP.DirectoryPath
		,TSt.IsSychronizedCW
		,TSt.IsFeatured
		,CB.BookingDate
		,CP.HostUrl
		,CP.OriginalImgPath
		,
		-- Modified By : Tejashree Patil on 3 Jan 2012 at 6.30pm
		@ValueAdditions AS CarValueAdditions
		,TSt.CertificationId
		,
		-- Modified By Vivek Gupta on 17-12-2013
		@YouTubeVideoLink AS YoutubeVideoLink
		,CFT.FuelType
	FROM TC_Stock TSt WITH (NOLOCK)
	LEFT JOIN TC_CarCondition TCC WITH (NOLOCK) ON TSt.Id = TCC.StockId
	LEFT JOIN TC_StockStatus TS WITH (NOLOCK) ON TS.Id = TSt.StatusId
	LEFT JOIN Dealers D WITH (NOLOCK) ON TSt.BranchId = D.Id
	LEFT JOIN TC_CarPhotos CP WITH (NOLOCK) ON CP.StockId = TSt.Id
		AND CP.IsMain = 1
		AND CP.IsActive = 1
	LEFT JOIN CarVersions CV WITH (NOLOCK) ON CV.Id = TSt.VersionId
		AND CV.IsDeleted = 0
	LEFT JOIN CarFuelType CFT WITH (NOLOCK) ON CV.CarFuelType = CFT.FuelTypeId
	LEFT JOIN CarModels CMO WITH (NOLOCK) ON CMO.Id = CV.CarModelId
		AND CMO.IsDeleted = 0
	LEFT JOIN CarMakes CM WITH (NOLOCK) ON CM.Id = CMO.CarMakeId
		AND CM.IsDeleted = 0
	LEFT JOIN TC_CarBooking CB WITH (NOLOCK) ON TSt.Id = CB.StockId
	-- LEFT JOIN SellInquiries SI WITH (NOLOCK) ON SI.TC_StockId = TSt.Id AND TSt.IsSychronizedCW = 1
	WHERE TSt.ID = @StockId
		AND TSt.BranchId = @BranchId
		AND TSt.IsActive = 1
		AND TSt.IsApproved = 1

	SELECT TC.Id
		,ImageUrlFull
		,ImageUrlThumb
		,ImageUrlThumbSmall
		,TC.IsMain
		,DirectoryPath
		,HostUrl
		,TC.StatusId
		,OriginalImgPath
	FROM TC_CarPhotos TC WITH (NOLOCK)
	WHERE TC.IsActive = 1
		AND TC.StockId = @StockId
	ORDER BY TC.IsMain DESC
		,TC.Id
END

