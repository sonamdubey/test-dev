IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetVersionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetVersionDetails]
GO

	
-- ==========================================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching details of particular Version 
-- ==========================================================

CREATE PROCEDURE [App].[GetVersionDetails]
	@ID Integer
AS
BEGIN

	SET NOCOUNT ON;
	SELECT	
	CMA.ID AS MakeId, CMO.ID as ModelId, CV.ID AS VersionId, CMA.Name AS Make, CMO.Name AS Model,CV.Name AS Version,
	NCSF.*, NCS.*, NCNP.AvgPrice, CMO.LargePic AS ThumbUrl, CMO.HostUrl AS HostingUrl, 
	ISNULL(CMO.ReviewCount,0) AS Reviews, 
	ISNULL(CMO.ReviewRate, 0) AS ReviewRate 
	FROM	
	CarVersions CV, CarModels CMO, CarMakes CMA, NewCarStandardFeatures NCSF, NewCarSpecifications NCS, Con_NewCarNationalPrices NCNP
	WHERE	
	CV.CarModelId = CMO.ID
	AND CMO.CarMakeId = CMA.ID	
	AND CV.ID = NCSF.CarVersionId
	AND CV.ID = NCS.CarVersionId
	AND CV.ID = NCNP.VersionId
	AND CV.ID = @ID
END

