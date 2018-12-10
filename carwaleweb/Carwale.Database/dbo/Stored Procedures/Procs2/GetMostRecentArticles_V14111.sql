IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMostRecentArticles_V14111]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMostRecentArticles_V14111]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the most recent articles  with the given contentTypes as categoryids , applicationid, totalrecords , makeid and modelid as input parameter
-- modified by natesh kumar for fetching make and model from carwale and bikewale resp.
-- modified by natesh kumar added subcategory name for bikewale too on 17.11.14
-- modified by natesh kumar added ismainimg flag and isdeleted check on 1/12/14
--================================================================
-- exec [dbo].[GetMostRecentArticles_V14111] 1,8,50
CREATE PROCEDURE [dbo].[GetMostRecentArticles_V14111] 
	@ApplicationId INT
	,@contentTypes VARCHAR(50)
	,@totalRecords INT
	,@MakeId INT = NULL
	,@ModelId INT = NULL
AS

BEGIN
	IF(@ApplicationId = 1)
	BEGIN
		WITH cte
		AS (
			SELECT cb.Id AS BasicId
				,cb.CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.Url AS ArticleUrl
				,DisplayDate AS DisplayDate
				,AuthorName AS AuthorName
				,cb.Description AS Description
				,PublishedDate
				,cb.VIEWS AS VIEWS
				,(ISNULL(cb.IsSticky,0)) AS IsSticky
				,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
				,cei.HostURL AS HostUrl
				,cei.ImagePathLarge AS LargePicUrl
				,cei.ImagePathThumbnail AS SmallPicUrl
				,M.Id AS MakeId
				,MO.ID AS ModelId
				,M.NAME AS MakeName
				,MO.NAME AS ModelName
				,MO.MaskingName AS ModelMaskingName
				,SC.Name As SubCategory
				,ROW_NUMBER() OVER (
					PARTITION BY cb.Id ORDER BY cb.DisplayDate DESC
					) rowno
					
			FROM Con_EditCms_Basic cb WITH (NOLOCK) 
			LEFT JOIN Con_EditCms_Images cei WITH (NOLOCK) ON cb.Id = cei.BasicId
						AND cei.IsMainImage = 1
						AND cei.IsActive = 1
			LEFT JOIN SocialPluginsCount spc WITH (NOLOCK) ON cb.Id = spc.TypeId
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
				AND C.IsActive = 1
			LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId 
			LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = C.MakeId 
			LEFT JOIN Con_EditCms_BasicSubCategories BSC 
                 WITH (NOLOCK) ON BSC.BasicId = cb.Id 
                 LEFT JOIN Con_EditCms_SubCategories SC 
                 WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId And SC.IsActive = 1
			WHERE 
				cb.ApplicationID = @ApplicationId
				AND cb.IsActive = 1
				AND cb.IsPublished = 1
				AND ISNULL(MO.IsDeleted, 0) = 0
				AND ISNULL(M.IsDeleted, 0) = 0
				AND cb.CategoryId IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@contentTypes)
					)
				AND (@MakeId IS NULL OR M.Id = @MakeId)
				AND (@ModelId IS NULL OR MO.ID = @ModelId)
			)
		SELECT TOP (@totalRecords) *
		FROM cte
		WHERE rowno = 1
		ORDER BY DisplayDate DESC
	END

	IF(@ApplicationId = 2)
	BEGIN
		WITH cte
		AS (
			SELECT cb.Id AS BasicId
				,cb.CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.Url AS ArticleUrl
				,DisplayDate AS DisplayDate
				,AuthorName AS AuthorName
				,cb.Description AS Description
				,PublishedDate
				,cb.VIEWS AS VIEWS
				,(ISNULL(cb.IsSticky,0)) AS IsSticky
				,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
				,cei.HostURL AS HostUrl
				,cei.ImagePathCustom AS LargePicUrl
				,cei.ImagePathThumbnail AS SmallPicUrl
				,M.Id AS MakeId
				,MO.ID AS ModelId
				,M.NAME AS MakeName
				,MO.NAME AS ModelName
				,MO.MaskingName AS ModelMaskingName
				,NULL AS SubCategory -- null value is stored in subcategory name as default
				,ROW_NUMBER() OVER (
					PARTITION BY cb.Id ORDER BY cb.DisplayDate DESC
					) rowno
			FROM Con_EditCms_Basic cb WITH (NOLOCK) 
			LEFT JOIN Con_EditCms_Images cei WITH (NOLOCK) ON cb.Id = cei.BasicId
					AND cei.IsMainImage = 1
					AND cei.IsActive = 1
			LEFT JOIN SocialPluginsCount spc WITH (NOLOCK) ON cb.Id = spc.TypeId
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
				AND C.IsActive = 1
			LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId 
			LEFT JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId
			
			WHERE 
				cb.ApplicationID = @ApplicationId
				AND cb.IsActive = 1
				AND cb.IsPublished = 1
				AND ISNULL(MO.IsDeleted, 0) = 0
				AND ISNULL(M.IsDeleted, 0) = 0
				AND cb.CategoryId IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@contentTypes)
					)
				AND (@MakeId IS NULL OR M.Id = @MakeId)
				AND (@ModelId IS NULL OR MO.ID = @ModelId)
			)
		SELECT TOP (@totalRecords) *
		FROM cte
		WHERE rowno = 1
		ORDER BY DisplayDate DESC
	END
END

