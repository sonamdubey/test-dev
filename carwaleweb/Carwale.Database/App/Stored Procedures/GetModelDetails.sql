IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetModelDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetModelDetails]
GO

	
CREATE PROCEDURE [App].[GetModelDetails] 
	 @ID Integer
AS
BEGIN

	SELECT 
	CMO.ID AS ID, CMO.Name AS Model, IsNull(CMO.ReviewRate, 0) AS ReviewRate,
	IsNull(CMO.ReviewCount, 0) AS ReviewCount, CMA.Name AS Make, CMA.ID AS MakeId, CMO.SmallPic, CMO.HostUrl, CMO.LargePic,
	(SELECT TOP 1 CONVERT(VARCHAR,MIN(AvgPrice)) 
	FROM Carwale_com.dbo.Con_NewCarNationalPrices 
	WHERE Carwale_com.dbo.Con_NewCarNationalPrices.VersionId IN	
	(SELECT ID FROM Carwale_com.dbo.CarVersions WHERE Carwale_com.dbo.CarVersions.CarModelId = CMO.ID AND Carwale_com.dbo.CarVersions.New = 1 AND Carwale_com.dbo.CarVersions.IsDeleted = 0) AND AvgPrice > 0) AS Price 
	FROM 
	Carwale_com.dbo.CarModels CMO LEFT JOIN Carwale_com.dbo.CarMakes CMA 
	ON CMO.CarMakeId = CMA.ID 
	WHERE 
	CMO.ID = @ID

END