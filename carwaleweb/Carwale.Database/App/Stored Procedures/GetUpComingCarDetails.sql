IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetUpComingCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetUpComingCarDetails]
GO

	
-- ================================================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching details of particular Upcoming Car
-- ================================================================

CREATE PROCEDURE [App].[GetUpComingCarDetails] 
	@ID Integer
	
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT MK.Name MakeName, Mo.Name AS ModelName, ECL.ExpectedLaunch, ECL.EstimatedPriceMin, ECL.EstimatedPriceMax,ECL.CWConfidence,ECL.UpdatedDate,
	Mo.HostUrl, Mo.SmallPic,Mo.LargePic,CS.FullDescription AS Review,Mo.ID AS ModelId	
	FROM ExpectedCarLaunches ECL 
	INNER JOIN CarModels Mo ON ECL.CarModelId = Mo.ID 
	INNER JOIN CarMakes MK ON MK.ID = Mo.CarMakeId 
	INNER JOIN CarSynopsis CS ON MO.ID = CS.ModelId
	WHERE CS.IsActive = 1 AND ECL.Id = @ID 
END

