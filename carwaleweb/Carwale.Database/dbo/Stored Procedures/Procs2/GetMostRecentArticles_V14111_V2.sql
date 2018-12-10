IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMostRecentArticles_V14111_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMostRecentArticles_V14111_V2]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the most recent articles  with the given contentTypes as categoryids , applicationid, totalrecords , makeid and modelid as input parameter
-- modified by natesh kumar for fetching make and model from carwale and bikewale resp.
-- modified by natesh kumar added subcategory name for bikewale too on 17.11.14
-- modified by natesh kumar added ismainimg flag and isdeleted check on 1/12/14
--================================================================
-- exec [dbo].[GetMostRecentArticles_V14111_V2] 1,8,50
CREATE PROCEDURE [dbo].[GetMostRecentArticles_V14111_V2] 
	@ApplicationId INT
	,@contentTypes VARCHAR(50)
	,@totalRecords INT
	,@MakeId INT = NULL
	,@ModelId INT = NULL
AS

BEGIN
   
	IF(@ApplicationId = 1)
	BEGIN
		
		
		SELECT   cb.Id AS BasicId
		,cb.CategoryId AS CategoryId
		,cb.Title AS Title
		,cb.Url AS ArticleUrl
		,DisplayDate AS DisplayDate
		,AuthorName AS AuthorName
		,cb.Description AS Description
		,PublishedDate
		,cb.VIEWS AS VIEWS
		,(ISNULL(cb.IsSticky,0)) AS IsSticky
		,cei.HostURL AS HostUrl
		,cei.ImagePathLarge AS LargePicUrl
		,cei.ImagePathThumbnail AS SmallPicUrl		
		,ROW_NUMBER() OVER (
			PARTITION BY cb.Id ORDER BY cb.DisplayDate DESC
			) rowno
	INTO #tempCarBasic		
	FROM Con_EditCms_Basic cb WITH (NOLOCK) 
	JOIN Con_EditCms_Images cei WITH (NOLOCK) ON cb.Id = cei.BasicId
				AND cei.IsMainImage = 1
				AND cei.IsActive = 1	
	WHERE 
		cb.ApplicationID = 1
		AND cb.IsActive = 1
		AND cb.IsPublished = 1;
		
		
		
	WITH cte
		AS (
			SELECT top 1000   cb.BasicId
				,cb.CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.ArticleUrl
				,DisplayDate AS DisplayDate
				,AuthorName AS AuthorName,cb.Description AS Description
				,PublishedDate
				,cb.VIEWS AS VIEWS
				,(ISNULL(cb.IsSticky,0)) AS IsSticky
				,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
				,cb.HostURL AS HostUrl
				,cb.LargePicUrl
				,cb.SmallPicUrl
				,M.Id AS MakeId
				,MO.ID AS ModelId
				,M.NAME AS MakeName
				,MO.NAME AS ModelName
				,MO.MaskingName AS ModelMaskingName
				,SC.Name As SubCategory
				,ROW_NUMBER() OVER (
					PARTITION BY cb.BasicId ORDER BY cb.DisplayDate DESC
					) rowno
					
			FROM #tempCarBasic cb
			JOIN SocialPluginsCount spc WITH (NOLOCK) ON cb.BasicId = spc.TypeId
			JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.BasicId
				AND C.IsActive = 1
			JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId 
			JOIN CarMakes M WITH (NOLOCK) ON M.ID = C.MakeId 
			JOIN Con_EditCms_BasicSubCategories BSC 
                 WITH (NOLOCK) ON BSC.BasicId = cb.BasicId 
                 JOIN Con_EditCms_SubCategories SC 
                 WITH (NOLOCK) ON SC.Id = BSC.SubCategoryId And SC.IsActive = 1
			WHERE 
				ISNULL(MO.IsDeleted, 0) = 0
				AND ISNULL(M.IsDeleted, 0) = 0
				AND cb.CategoryId IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@contentTypes)
					)
				AND (@MakeId IS NULL OR M.Id = @MakeId)
				AND (@ModelId IS NULL OR MO.ID = @ModelId)
			ORDER BY cb.DisplayDate DESC
			)
		SELECT TOP (@totalRecords) *
		FROM cte
		WHERE rowno = 1
		ORDER BY DisplayDate DESC
		
		DROP TABLE #tempCarBasic
		
	END

	IF(@ApplicationId = 2)
	BEGIN
		SELECT  cb.Id AS BasicId
				,cb.CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.Url AS ArticleUrl
				,DisplayDate AS DisplayDate
				,AuthorName AS AuthorName
				,cb.Description AS Description
				,PublishedDate
				,cb.VIEWS AS VIEWS
				,(ISNULL(cb.IsSticky,0)) AS IsSticky
				,cei.HostURL AS HostUrl
				,cei.ImagePathLarge AS LargePicUrl
				,cei.ImagePathThumbnail AS SmallPicUrl				
				,NULL AS SubCategory -- null value is stored in subcategory name as default
				,ROW_NUMBER() OVER (
					PARTITION BY cb.Id ORDER BY cb.DisplayDate DESC
					) rowno
			INTO #tempBikeBasic
			FROM Con_EditCms_Basic cb WITH (NOLOCK) 
			LEFT JOIN Con_EditCms_Images cei WITH (NOLOCK) ON cb.Id = cei.BasicId
					AND cei.IsMainImage = 1
					AND cei.IsActive = 1
			WHERE 
				cb.ApplicationID = 2
				AND cb.IsActive = 1
				AND cb.IsPublished = 1;
				
			
		WITH cte
		AS (
			SELECT top 10000  cb.BasicId
				,cb.CategoryId
				 ,cb.Title
				 ,cb.ArticleUrl
				,cb.DisplayDate 
				,cb.AuthorName 
				, cb.Description
				,PublishedDate
				, VIEWS
				,IsSticky
				,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
				, cb.HostUrl
				, LargePicUrl
				, SmallPicUrl
				,M.Id AS MakeId
				,MO.ID AS ModelId
				,M.NAME AS MakeName
				,MO.NAME AS ModelName
				,MO.MaskingName AS ModelMaskingName
				,NULL AS SubCategory -- null value is stored in subcategory name as default
				,ROW_NUMBER() OVER (
					PARTITION BY cb.BasicId ORDER BY cb.DisplayDate DESC
					) rowno
			FROM #tempBikeBasic cb
			LEFT JOIN SocialPluginsCount spc WITH (NOLOCK) ON cb.BasicId = spc.TypeId
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.BasicId
				AND C.IsActive = 1
			LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId 
			LEFT JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId
			
			WHERE 
				ISNULL(MO.IsDeleted, 0) = 0
				AND ISNULL(M.IsDeleted, 0) = 0
				AND cb.CategoryId IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@contentTypes)
					)
				AND (@MakeId IS NULL OR M.Id = @MakeId)
				AND (@ModelId IS NULL OR MO.ID = @ModelId)
				ORDER BY cb.DisplayDate DESC
			)
		SELECT TOP (@totalRecords) *
		FROM cte
		WHERE rowno = 1
		ORDER BY DisplayDate DESC
		
		DROP TABLE #tempBikeBasic
		
	END
END


