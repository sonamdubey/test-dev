IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetUpcomingCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetUpcomingCars]
GO

	
-- ===============================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching all Upcoming Cars
-- ===============================================

CREATE PROCEDURE [App].[GetUpcomingCars]
	
AS
BEGIN
	
	SET NOCOUNT ON;
		SELECT ROW_NUMBER() OVER (ORDER BY ECL.LaunchDate) AS Row, 
		ECL.ID, MK.Name MakeName, Mo.Name AS ModelName, ECL.ExpectedLaunch,ECL.CWConfidence,ECL.UpdatedDate,
		ECL.EstimatedPriceMin,ECL.EstimatedPriceMax, 
		Mo.HostUrl, Mo.SmallPic ,Mo.LargePic
		FROM ExpectedCarLaunches ECL 
		INNER JOIN CarModels Mo ON ECL.CarModelId = Mo.ID 
		INNER JOIN CarMakes MK ON MK.ID = Mo.CarMakeId 
		WHERE Mo.Futuristic = 1 AND ECL.IsLaunched = 0 
	
END

