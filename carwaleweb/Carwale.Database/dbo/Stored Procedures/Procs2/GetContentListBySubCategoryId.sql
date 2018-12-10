IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContentListBySubCategoryId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContentListBySubCategoryId]
GO

	--===================================================================================
--Created By Natesh Kumar on 1-01-14
--description: get article list on subcategory id
--
--======================================================================================

CREATE PROCEDURE [dbo].[GetContentListBySubCategoryId]

		@ApplicationId INT
		,@SubCategory VARCHAR(30)
		,@StartIndex INT
		,@EndIndex INT
		,@MakeId INT = NULL
		,@ModelId INT = NULL

AS

BEGIN
		
		-- Sticky Query.
		(	
		SELECT DISTINCT
			cb.Id AS BasicId
			,CB.CategoryId AS CategoryId
			,cb.Title AS Title
			,cb.Url AS ArticleUrl
			,DisplayDate AS DisplayDate
			,CB.AuthorName AS AuthorName
			,CA.MaskingName AS AuthorMaskingName
			,cb.Description AS Description
			,PublishedDate 
			,cb.Views AS Views
			,(ISNULL(cb.IsSticky,0)) AS IsSticky
			,cei.HostURL AS HostUrl	
			,cei.ImagePathLarge AS LargePicUrl
			,cei.ImagePathThumbnail AS SmallPicUrl
			,'1' AS Row_No
			,'1' as Row_Num
		FROM Con_EditCms_Basic AS CB WITH(NOLOCK)
		INNER JOIN Con_EditCms_BasicSubCategories BSC  WITH (NOLOCK) ON BSC.BasicId = CB.Id
		INNER JOIN Con_EditCms_SubCategories SC WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId AND SC.IsActive = 1
		INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
		LEFT JOIN Con_EditCms_Images CEI  WITH(NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1
		WHERE CB.ApplicationID = @ApplicationId AND SC.Id = @SubCategory
			AND CB.IsActive = 1 AND CB.IsPublished = 1 AND IsSticky = 1
			AND (CAST(GETDATE() AS DATE) BETWEEN CAST(StickyFromDate AS DATE)AND CAST(StickyToDate AS DATE))
		)
		UNION ALL
		(-- Normal Query
		SELECT  * FROM
		(
			SELECT *, ROW_NUMBER() OVER (ORDER BY DisplayDate DESC) AS Row_Num FROM(
			SELECT
				cb.Id AS BasicId
				,CB.CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.Url AS ArticleUrl
				,DisplayDate AS DisplayDate
				,CB.AuthorName AS AuthorName
				,CA.MaskingName AS AuthorMaskingName
				,cb.Description AS Description
				,PublishedDate 
				,cb.Views AS Views
				,(ISNULL(cb.IsSticky,0)) AS IsSticky
				,cei.HostURL AS HostUrl		
				,cei.ImagePathLarge AS LargePicUrl
				,cei.ImagePathThumbnail AS SmallPicUrl
				,ROW_NUMBER() OVER (PARTITION BY CB.Id ORDER BY DisplayDate DESC) AS Row_No		
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			INNER JOIN Con_EditCms_BasicSubCategories BSC  WITH (NOLOCK) ON BSC.BasicId = CB.Id
			INNER JOIN Con_EditCms_SubCategories SC WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId AND SC.IsActive = 1
			INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId	
			LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1
			WHERE SC.Id = @SubCategory
			AND CB.ApplicationID = @ApplicationId AND CB.IsActive = 1 AND CB.IsPublished = 1
			AND CB.DisplayDate <= GETDATE()
			AND (IsSticky IS NULL OR IsSticky = 0 OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE))
			) AS CTE1 WHERE Row_No = 1
		) CTE WHERE Row_Num BETWEEN @StartIndex AND @EndIndex
) ORDER BY Row_Num
		
				-- Record Count Query
		SELECT COUNT(DISTINCT CB.Id) AS RecordCount
		FROM Con_EditCms_Basic CB WITH (NOLOCK)
		INNER JOIN Con_EditCms_BasicSubCategories BSC  WITH (NOLOCK) ON BSC.BasicId = CB.Id
		INNER JOIN Con_EditCms_SubCategories SC WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId AND SC.IsActive = 1
		WHERE SC.Id = @SubCategory
		AND CB.IsPublished = 1 AND CB.ApplicationID = @ApplicationId AND CB.IsActive = 1

END
