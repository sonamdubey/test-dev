IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetVersions]
GO

	
-- ==========================================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching versions of particular model
-- ==========================================================

CREATE PROCEDURE [App].[GetVersions]
	@ID Integer
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT DISTINCT CV.ID AS ID, CMA.Name AS Make, CMO.Name AS Model, CV.Name AS Version, CV.CarModelId AS ModelId, 
	Displacement, FuelType, TransmissionType, SeatingCapacity, MileageOverall, 
	(SELECT AvgPrice FROM Con_NewCarNationalPrices WHERE 
	Con_NewCarNationalPrices.VersionId=CV.ID) AS MinPrice, CV.New, IsNull(CV.ReviewRate, 0) AS ReviewRate, 
	IsNull(CV.ReviewCount, 0) AS ReviewCount 
	FROM CarMakes AS CMA, CarModels AS CMO, CarVersions AS CV, NewCarSpecifications NS 
	WHERE CV.IsDeleted=0 AND CV.New = 1 AND CV.ID=NS.CarVersionID AND CV.CarModelId=@ID AND CV.CarModelId = CMO.ID AND CMO.CarMakeId = CMA.ID
	ORDER BY CV.New DESC, MinPrice ASC, CV.Name
END

