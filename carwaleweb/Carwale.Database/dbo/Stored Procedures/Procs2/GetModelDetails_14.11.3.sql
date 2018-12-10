IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelDetails_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelDetails_14]
GO

	
-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 10 July 2013
-- Description:	Proc to get model details
--Modified By Rakesh Yadav on 30 Apr 2014 selecting video count
--Modified By Rohan Sapkal on 27-11-2014 added OfferExists Output Parameter
-- Mofified By Satish Sharma on 10/Jun/2015, Increases size of input parameter @LargePic, @SmallPic from Varchar 50 to 150
-- =============================================
CREATE PROCEDURE [dbo].[GetModelDetails_14.11.3] --Execute [dbo].[GetModelDetails_14.11.3] 458
	@ModelId BIGINT
	,@MakeId BIGINT OUTPUT
	,@MakeName VARCHAR(30) OUTPUT
	,@ModelName VARCHAR(30) OUTPUT
	,@MaskingName VARCHAR(30) = NULL OUTPUT
	,@LargePic VARCHAR(150) OUTPUT
	,@SmallPic VARCHAR(150) OUTPUT
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
	,@MinPrice FLOAT OUTPUT
	,@MaxPrice FLOAT OUTPUT
	,@VideoCount INT = NULL OUTPUT
	,@OfferExists INT = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @MakeId = Mk.ID
		,@MakeName = Mk.NAME
		,@ModelName = Mo.NAME
		,@MaskingName = Mo.MaskingName
		,@LargePic = Mo.LargePic
		,@SmallPic = Mo.SmallPic
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
		,@MinPrice = Mo.MinPrice
		,@MaxPrice = Mo.MaxPrice
		,@VideoCount = Mo.VideoCount
	FROM CarModels Mo WITH(NOLOCK)
	INNER JOIN CarMakes Mk WITH(NOLOCK) ON Mk.ID = Mo.CarMakeId
	WHERE Mo.CarMakeId = Mk.Id
		AND Mo.ID = @ModelId
		
	SELECT @OfferExists = MO.ModelId 
	FROM ModelOffers MO WITH(NOLOCK)
	WHERE MO.ModelId=@ModelId
END

