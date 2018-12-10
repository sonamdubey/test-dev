IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRoadTestCount_v16_6_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetRoadTestCount_v16_6_1]
GO

	
-- =============================================
-- Author:		Kumar Vikram
-- Create date: 20.05.2014
-- Description:	Gets Road Test count for any car
-- Avishkar added WITH RECOMPILE
-- Modified: Optimised query by removing unnecessary joins by amit v 24/05/2014
-- Modified: Optimised query by removing unnecessary joins by jitendra 21/06/2016
-- =============================================
create PROCEDURE [dbo].[GetRoadTestCount_v16_6_1] @Top INT = 0
	,@MakeId INT = 0
	,@ModelId INT = 0
	,@VersionId INT = 0
	,@Category VarChar(10) = '8'
	,@ApplicationId int 
--WITH RECOMPILE
AS
BEGIN


	--SELECT count(*) AS Count
	--FROM Con_EditCms_Basic CEB WITH(NOLOCK) 
	--LEFT JOIN Con_EditCms_BasicSubCategories BSC WITH(NOLOCK) ON BSC.BasicId = CEB.Id
	--LEFT JOIN Con_EditCms_SubCategories SC WITH(NOLOCK) ON SC.Id = BSC.SubCategoryId
	--	AND SC.IsActive = 1
	--LEFT JOIN Con_EditCms_Cars CEC WITH(NOLOCK) ON CEC.BasicId = CEB.Id
	--	AND CEC.IsActive = 1
	--LEFT JOIN CarModels Mo WITH(NOLOCK) ON Mo.ID = CEC.ModelID
	--LEFT JOIN CarMakes Ma WITH(NOLOCK) ON Ma.ID = Mo.CarMakeId
	--LEFT JOIN CarVersions Ve WITH(NOLOCK) ON Ve.CarModelId = Mo.ID
	--	AND Ve.Id = CEC.VersionId
	--LEFT JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CEB.Id
	--	AND CEI.IsMainImage = 1
	--	AND CEI.IsActive = 1
	--WHERE CEB.CategoryId in (SELECT items FROM dbo.SplitText(@Category,','))
	--	AND CEB.IsActive = 1
	--	AND CEB.IsPublished = 1
	--	AND (
	--		Ma.Id = @MakeId
	--		OR @MakeId = 0
	--		)
	--	AND (
	--		Mo.Id = @ModelId
	--		OR @ModelId = 0
	--		)
	--	AND (
	--		Ve.Id = @VersionId
	--		OR @VersionId = 0
	--		)
		
	SELECT count(DISTINCT CEB.Id) AS Count
	FROM Con_EditCms_Basic CEB WITH(NOLOCK) 
	LEFT JOIN Con_EditCms_Cars CEC WITH(NOLOCK) ON CEC.BasicId = CEB.Id
		AND CEC.IsActive = 1
	WHERE CEB.CategoryId in (SELECT items FROM dbo.SplitText(@Category,','))
		AND CEB.IsActive = 1
		AND CEB.IsPublished = 1
		AND CEB.ApplicationID = @ApplicationId
		AND (
			CEC.MakeId = @MakeId
			OR @MakeId = 0
			)
		AND (
			CEC.MakeId = @ModelId
			OR @ModelId = 0
			)
		AND (
			CEC.VersionId = @VersionId
			OR @VersionId = 0
			)	
END