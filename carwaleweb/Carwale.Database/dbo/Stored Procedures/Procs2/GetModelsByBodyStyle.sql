IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelsByBodyStyle]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelsByBodyStyle]
GO

	
-- =============================================
-- Author:		Chetan Thambad	
-- Create date: 17/03/2016
-- Description:	This SP will get model ids for showing recommendation with or without body Style
-- modified by : Chetan T. on <23/03/2016> Retrieved ModelPopularity from CarModels Table
-- =============================================
CREATE PROCEDURE [dbo].[GetModelsByBodyStyle]
	-- Add the parameters for the stored procedure here
	@SimilarBodyStyle BIT
	,@BodyStyle INT
AS
BEGIN
	SELECT CMO.ID AS ModelId
		,CMK.ID AS MakeId
		,CMK.NAME AS MakeName
		,CMO.NAME AS ModelName
		,CMO.MaskingName
		,CMO.OriginalImgPath
		,CMO.HostURL
		,CMO.Looks
		,CMO.Performance
		,CMO.Comfort
		,CMO.ValueForMoney
		,CMO.FuelEconomy
		,CMO.ReviewRate
		,CMO.ReviewCount
		,CMO.Futuristic
		,CMO.New
		,CMO.RootId
		,CMO.MinPrice
		,CMO.MaxPrice
		,CMO.VideoCount
		,CMO.ModelBodyStyle
		,CMO.SubSegmentID
		,CMO.ModelPopularity
	FROM CarModels CMO WITH (NOLOCK)
	INNER JOIN CarMakes CMK WITH (NOLOCK) ON CMO.CarMakeId = CMK.ID
	WHERE (
			(
				ModelBodyStyle = @BodyStyle
				AND @SimilarBodyStyle = 1
				)
			OR (
				ModelBodyStyle != @BodyStyle
				AND @SimilarBodyStyle = 0
				)
			)
		AND CMO.New = 1
		AND CMO.IsDeleted = 0
END
