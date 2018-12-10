IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetRoadTestMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetRoadTestMakes]
GO

	
CREATE PROCEDURE [App].[GetRoadTestMakes]
	
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT	DISTINCT CMA.Id,CMA.Name, CMA.LogoUrl 
	FROM 
	carwale_com.dbo.Con_EditCms_Basic AS CB
	Join carwale_com.dbo.Con_EditCms_Cars CC
	On CC.BasicId = CB.Id And CC.IsActive = 1
	Join carwale_com.dbo.CarMakes CMA 
	On CMA.ID = CC.MakeId 
	WHERE CMA.New = 1 and CMA.IsDeleted = 0
	AND CB.CategoryId = 8 AND CB.IsActive = 1 AND CB.IsPublished = 1
	ORDER BY Name
  
END
