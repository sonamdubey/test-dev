IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelDetails-v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelDetails-v16]
GO

	
-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 10 July 2013
-- Description:	Proc to get model details
-- Modified By Rakesh Yadav on 30 Apr 2014 selecting video count
-- Modified By Rohan Sapkal on 27-11-2014 added OfferExists Output Parameter
-- Mofified By Satish Sharma on 10/Jun/2015, Increases size of input parameter @LargePic, @SmallPic from Varchar 50 to 150
-- Mofified By Vikas J Use New Column for images
-- Modified By Jitendra fetch RootId
-- Modifies By Ajay Singh on 14 may 2016 to fetch pricecity and pricecityname
-- =============================================
CREATE PROCEDURE [dbo].[GetModelDetails-v16.5.4] --Execute [dbo].[GetModelDetails-v16.5.4] 458
	@ModelId INT
	,@MakeId INT OUTPUT
	,@MakeName VARCHAR(30) OUTPUT
	,@ModelName VARCHAR(30) OUTPUT
	,@MaskingName VARCHAR(30) = NULL OUTPUT
	,@OriginalImgPath VARCHAR(150) OUTPUT
	,@HostURL VARCHAR(100) OUTPUT
	,@Looks FLOAT OUTPUT
	,@Performance FLOAT OUTPUT
	,@Comfort FLOAT OUTPUT
	,@ValueForMoney FLOAT OUTPUT
	,@FuelEconomy FLOAT OUTPUT
	,@ReviewRate FLOAT OUTPUT
	,@ReviewCount NUMERIC(18, 2) OUTPUT
	,@Futuristic BIT OUTPUT
	,@New BIT OUTPUT
	,@RootId INT OUTPUT
	,@MinPrice FLOAT OUTPUT
	,@MaxPrice FLOAT OUTPUT
	,@VideoCount INT = NULL OUTPUT
	,@OfferExists INT = NULL OUTPUT
	,@BodyStyleId int OUTPUT  --modified by anchal
	,@SubSegmentId int OUTPUT  -- modified by anchal
	,@PriceCityId INT OUTPUT --added by ajay singh
	,@PriceCityName VARCHAR(100) OUTPUT --added by ajay singh
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @MakeId = Mk.ID
		,@MakeName = Mk.NAME
		,@ModelName = Mo.NAME
		,@MaskingName = Mo.MaskingName
		,@OriginalImgPath = Mo.OriginalImgPath
		,@HostURL = Mo.HostURL
		,@Looks = Mo.Looks
		,@Performance = Mo.Performance
		,@Comfort = Mo.Comfort
		,@ValueForMoney = Mo.ValueForMoney
		,@FuelEconomy = Mo.FuelEconomy
		,@ReviewRate = Mo.ReviewRate
		,@ReviewCount = Mo.ReviewCount
		,@Futuristic = Mo.Futuristic
		,@New = Mo.New
		,@RootId = Mo.RootId
		,@MinPrice = Mo.MinPrice
		,@MaxPrice = Mo.MaxPrice
		,@VideoCount = Mo.VideoCount
		,@BodyStyleId = Mo.ModelBodyStyle
		,@SubSegmentId = Mo.SubSegmentID
		,@PriceCityId = Mo.PriceCityId --addded by ajay singh
		,@PriceCityName = Mo.PriceCityName --added by ajay singh
	FROM CarModels Mo WITH(NOLOCK)
	INNER JOIN CarMakes Mk WITH(NOLOCK) ON Mk.ID = Mo.CarMakeId
	WHERE Mo.ID = @ModelId
		
	SELECT @OfferExists = MO.ModelId 
	FROM ModelOffers MO WITH(NOLOCK)
	WHERE MO.ModelId=@ModelId
END


