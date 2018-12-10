IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetRoadTestCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetRoadTestCount]
GO

	
CREATE PROCEDURE [App].[GetRoadTestCount]

AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT COUNT(CB.ID) AS RoadTestsCount
	FROM
	carwale_com.dbo.Con_EditCms_Basic AS CB 
	Join carwale_com.dbo.Con_EditCms_Cars CC 
	On CC.BasicId = CB.Id And CC.IsActive = 1 
	Left Join carwale_com.dbo.Con_EditCms_Images CEI
	On CEI.BasicId=CB.Id And CEI.IsMainImage=1
	Join carwale_com.dbo.CarModels Cmo 
	On Cmo.ID = CC.ModelId 
	Join carwale_com.dbo.CarMakes Cma 
	On Cma.ID = CC.MakeId 
	Left Join carwale_com.dbo.Con_EditCms_BasicSubCategories BSC 
	On BSC.BasicId = CB.Id 
	Left Join carwale_com.dbo.Con_EditCms_SubCategories SC 
	On SC.Id = BSC.SubCategoryId And SC.IsActive = 1
	WHERE
	CB.CategoryId = 8 AND CB.IsActive = 1 AND CB.IsPublished = 1
   
END
