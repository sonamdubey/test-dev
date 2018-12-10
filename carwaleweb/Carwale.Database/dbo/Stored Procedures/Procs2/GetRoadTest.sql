IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRoadTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetRoadTest]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 29.08.2013
-- Description:	Gets Road Test for any car and also returns generic road test
-- Modified By : Akansha on 4.2.2014
-- Description : Added Masking Name Column
-- Avishkar added WITH RECOMPILE
-- Modified by Manish on 08-10-2014 changed order by from entry date to id 
-- Modified by Manish on 09-10-2014 Removed with Recompile Keyword
-- =============================================
CREATE PROCEDURE [dbo].[GetRoadTest] @Top INT = 0
	,@MakeId INT = 0
	,@ModelId INT = 0
	,@VersionId INT = 0
	,@Category VarChar(10) = '8'
	,@ApplicationId int
--WITH RECOMPILE
AS
BEGIN
	IF (@Top = 0)
		SET @Top = 3

	SELECT TOP (@Top) Ma.NAME AS Make
		,Mo.NAME AS Model
		,Ve.NAME AS Version
		,CEB.ID AS BasicID
		,CEB.Title
		,CEB.Description
		,SC.NAME AS SubCategory
		,CEI.HostUrl + CEI.ImagePathThumbnail AS ImagePath
		,CEB.AuthorName
		,CEB.DisplayDate
		,Mo.MaskingName
	FROM Con_EditCms_Basic CEB WITH(NOLOCK) 
	LEFT JOIN Con_EditCms_BasicSubCategories BSC WITH(NOLOCK) ON BSC.BasicId = CEB.Id
	LEFT JOIN Con_EditCms_SubCategories SC WITH(NOLOCK) ON SC.Id = BSC.SubCategoryId
		AND SC.IsActive = 1
	LEFT JOIN Con_EditCms_Cars CEC WITH(NOLOCK) ON CEC.BasicId = CEB.Id
		AND CEC.IsActive = 1
	LEFT JOIN CarModels Mo WITH(NOLOCK) ON Mo.ID = CEC.ModelID
	LEFT JOIN CarMakes Ma WITH(NOLOCK) ON Ma.ID = Mo.CarMakeId
	LEFT JOIN CarVersions Ve WITH(NOLOCK) ON Ve.CarModelId = Mo.ID
		AND Ve.Id = CEC.VersionId
	LEFT JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CEB.Id
		AND CEI.IsMainImage = 1
		AND CEI.IsActive = 1
	WHERE CEB.CategoryId in (SELECT items FROM dbo.SplitText(@Category,','))
		AND CEB.IsActive = 1
		AND CEB.IsPublished = 1
		AND CEB.ApplicationID = @ApplicationId
		AND (
			Ma.Id = @MakeId
			OR @MakeId = 0
			)
		AND (
			Mo.Id = @ModelId
			OR @ModelId = 0
			)
		AND (
			Ve.Id = @VersionId
			OR @VersionId = 0
			)
	ORDER BY   CEB.ID DESC      /*CEB.EntryDate*/ 
END
