IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllExpertReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllExpertReviews]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 24.10.2013
-- Description:	Gets Road and Comparison Test for any car and also returns generic Road and Comparison Test
-- exec [GetAllExpertReviews] 7,493,'8,2',281,284,3
-- Modified: Optimised query by removing unnecessary joins by amit v 24/05/2014
-- MOdified BY : Satish Sharma, commented featured query 
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- =============================================
CREATE PROCEDURE [dbo].[GetAllExpertReviews] @MakeId INT = 0
	,@ModelId INT = 0
	,@StartIndex INT = 1
	,@EndIndex INT = 10
	,@ApplicationId int 
AS
BEGIN
	WITH ExpertReviews
	AS (
		--SELECT TOP 3 Ma.NAME AS Make
		--	,Mo.NAME AS Model
		--	,Mo.MaskingName
		--	,CEB.ID AS BasicID
		--	,CEB.Url AS URL
		--	,CEB.Title
		--	,CEB.Description
		--	,CEI.HostUrl + CEI.ImagePathThumbnail AS ImagePath
		--	,CEB.AuthorName
		--	,CEB.DisplayDate
		--	,CEB.CategoryId
		--	,CEB.IsFeatured
		--	,ROW_NUMBER() OVER (
		--		PARTITION BY CEB.ID ORDER BY CEB.DisplayDate DESC
		--		) RowNo
		--	,'1' ShowASFeatured
		--FROM Con_EditCms_Basic CEB
		--LEFT JOIN Con_EditCms_Cars CEC ON CEC.BasicId = CEB.Id
		--	AND CEC.IsActive = 1
		--LEFT JOIN CarModels Mo ON Mo.ID = CEC.ModelID
		--LEFT JOIN CarMakes Ma ON Ma.ID = Mo.CarMakeId
		--LEFT JOIN Con_EditCms_Images CEI ON CEI.BasicId = CEB.Id
		--	AND CEI.IsMainImage = 1
		--	AND CEI.IsActive = 1
		--WHERE CEB.CategoryId IN (
		--		8
		--		,2
		--		)
		--	AND CEB.IsActive = 1
		--	AND CEB.IsPublished = 1
		--	AND CEB.IsFeatured = 1
		--	AND @StartIndex = 1
		--	AND @MakeId = 0
		--ORDER BY CEB.DisplayDate DESC
		
		--UNION ALL
		
		SELECT Ma.NAME AS Make
			,Mo.NAME AS Model
			,Mo.MaskingName
			,CEB.ID AS BasicID
			,CEB.Url AS URL
			,CEB.Title
			,CEB.Description
			,CEI.HostUrl + CEI.ImagePathThumbnail AS ImagePath
			,CEB.AuthorName
			,CEB.DisplayDate
			,CEB.CategoryId
			,CEB.IsFeatured
			,ROW_NUMBER() OVER (
				PARTITION BY CEB.ID ORDER BY CEB.DisplayDate DESC
				) RowNo
			,'0' ShowASFeatured
		FROM Con_EditCms_Basic CEB WITH(NOLOCK) 
		LEFT JOIN Con_EditCms_Cars CEC WITH(NOLOCK)  ON CEC.BasicId = CEB.Id
			AND CEC.IsActive = 1
		LEFT JOIN CarModels Mo WITH(NOLOCK)  ON Mo.ID = CEC.ModelID
		LEFT JOIN CarMakes Ma WITH(NOLOCK)  ON Ma.ID = Mo.CarMakeId
		LEFT JOIN Con_EditCms_Images CEI WITH(NOLOCK)  ON CEI.BasicId = CEB.Id
			AND CEI.IsMainImage = 1
			AND CEI.IsActive = 1
		WHERE CEB.CategoryId IN (
				8
				,2
				)
				AND CEB.ApplicationID = @ApplicationId
			AND CEB.IsActive = 1
			AND CEB.IsPublished = 1
			--AND CEB.Id NOT IN (
			--	-- REMOVE Featured records which are already included in above query
			--	SELECT TOP 3 BS.ID
			--	FROM Con_EditCms_Basic BS
			--	WHERE BS.IsFeatured = 1
			--		AND BS.CategoryId IN (
			--			8
			--			,2
			--			)
			--		AND BS.IsActive = 1
			--		AND BS.IsPublished = 1
			--	ORDER BY BS.DisplayDate DESC
			--	)
			AND (
				Ma.Id = @MakeId
				OR @MakeId = 0
				)
			AND (
				Mo.Id = @ModelId
				OR @ModelId = 0
				)
		)
		,CTE
	AS (
		SELECT *
			,ROW_NUMBER() OVER (
				ORDER BY ShowASFeatured DESC
					,DisplayDate DESC
				) ROW_INDEX
		FROM ExpertReviews WITH(NOLOCK) 
		WHERE RowNo = 1
		)

	SELECT *
	FROM CTE
	WHERE ROW_INDEX BETWEEN @StartIndex
			AND @endIndex	

	SELECT COUNT(DISTINCT CEB.ID) AS RecordCount
	FROM Con_EditCms_Basic CEB WITH(NOLOCK) 
	LEFT JOIN Con_EditCms_Cars CEC WITH(NOLOCK)  ON CEC.BasicId = CEB.Id
		AND CEC.IsActive = 1
	--LEFT JOIN CarModels Mo ON Mo.ID = CEC.ModelID
	--LEFT JOIN CarMakes Ma ON Ma.ID = Mo.CarMakeId
	--LEFT JOIN Con_EditCms_Images CEI ON CEI.BasicId = CEB.Id
	--	AND CEI.IsMainImage = 1
	--	AND CEI.IsActive = 1
	WHERE CEB.CategoryId IN (
			8
			,2
			)
		AND CEB.ApplicationID = @ApplicationId
		AND CEB.IsActive = 1
		AND CEB.IsPublished = 1
		AND (
			CEC.MakeId = @MakeId
			OR @MakeId = 0
			)
		AND (
			CEC.ModelId = @ModelId
			OR @ModelId = 0
			)
END


