IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelDetails]
GO

	-- =============================================

-- Author:		Ashish G. Kamble

-- Create date: 10 July 2013

-- Description:	Proc to get model details
--Modified By Rakesh Yadav on 30 Apr 2014 selecting video count

-- =============================================

CREATE PROCEDURE [dbo].[GetModelDetails] --Execute GetModelDetails 458

	@ModelId BIGINT,

	@MakeId BIGINT OUTPUT,

	@MakeName VARCHAR(30) OUTPUT,	

	@ModelName VARCHAR(30) OUTPUT,

	@MaskingName VARCHAR(30) = null OUTPUT,

	@LargePic VARCHAR(50) OUTPUT,

	@SmallPic VARCHAR(50) OUTPUT,

	@HostURL VARCHAR(100) OUTPUT,

	@Looks FLOAT OUTPUT,

	@Performance FLOAT OUTPUT,

	@Comfort FLOAT OUTPUT,

	@ValueForMoney FLOAT OUTPUT,

	@FuelEconomy FLOAT OUTPUT,

	@ReviewRate FLOAT OUTPUT,

	@ReviewCount NUMERIC(18,2) OUTPUT,

	@Futuristic BIT OUTPUT,

	@New BIT OUTPUT,

	@MinPrice FLOAT OUTPUT,

	@MaxPrice FLOAT OUTPUT,

	@VideoCount INT=NULL OUTPUT

AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;



    SELECT 

		@MakeId = Mk.ID, @MakeName = Mk.Name, @ModelName = Mo.Name, @MaskingName = Mo.MaskingName,

		@LargePic = Mo.LargePic, @SmallPic = Mo.SmallPic, @HostURL = Mo.HostURL,

		@Looks = Mo.Looks, @Performance = Mo.Performance, @Comfort = Mo.Comfort, @ValueForMoney = Mo.ValueForMoney, @FuelEconomy = Mo.FuelEconomy, 

		@ReviewRate = Mo.ReviewRate, @ReviewCount = Mo.ReviewCount,

		@Futuristic = Mo.Futuristic, @New = Mo.New,

		@MinPrice = Mo.MinPrice, @MaxPrice = Mo.MaxPrice, @VideoCount=Mo.VideoCount

	FROM CarModels Mo 

	INNER JOIN CarMakes Mk ON Mk.ID = Mo.CarMakeId

	WHERE Mo.CarMakeId = Mk.Id AND Mo.ID = @ModelId

END



