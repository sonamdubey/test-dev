IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetTopRoadTests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetTopRoadTests]
GO

	
-- =============================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching Top 7 Road Tests 
-- =============================================

CREATE PROCEDURE [App].[GetTopRoadTests]

AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		TOP 7
        CB.Id AS BasicId,CB.AuthorName,CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CB.MainImageSet,
        Cmo.Name As ModelName,Cmo.ID AS ModelId,
        Cma.Name As MakeName,Cma.ID AS MakeId,
        SC.Name As SubCategory,
        CEI.HostUrl,CEI.ImagePathThumbnail
        FROM
        Con_EditCms_Basic AS CB
        Join Con_EditCms_Cars CC On CC.BasicId = CB.Id And CC.IsActive = 1
        Left Join Con_EditCms_Images CEI On CEI.BasicId = CB.Id And CEI.IsMainImage = 1
        Join CarModels Cmo On Cmo.ID = CC.ModelId
        Join CarMakes Cma On Cma.ID = CC.MakeId
        Left Join Con_EditCms_BasicSubCategories BSC On BSC.BasicId = CB.Id
        Left Join Con_EditCms_SubCategories SC On SC.Id = BSC.SubCategoryId And SC.IsActive = 1
        WHERE
        CB.CategoryId = 8 AND CB.IsActive = 1 AND CB.IsPublished =1
        ORDER BY DisplayDate Desc
	
END
