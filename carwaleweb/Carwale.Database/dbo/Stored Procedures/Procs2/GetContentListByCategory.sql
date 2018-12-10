IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContentListByCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContentListByCategory]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the article list based on categoryid/categoryids with applicationid, categoryids list , startindex, endindex, makeid and modelid as input parameter
--================================================================

CREATE PROCEDURE [dbo].[GetContentListByCategory]  -- exec dbo.GetContentListByCategory 1,'1,2,3',1,100,null,null

	  @ApplicationId TINYINT
	, @CategoryList VARCHAR(50) 
	, @StartIndex INT
	, @EndIndex INT
	, @MakeId INT = NULL
	, @ModelId INT = NULL

AS

BEGIN

		
			

			IF (@ApplicationId = 1)
			BEGIN
			-- Sticky Query.
			(	
			SELECT DISTINCT
				cb.Id AS BasicId
				,CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.Url AS ArticleUrl
				,DisplayDate AS DisplayDate
				,CB.AuthorName AS AuthorName
				,CA.MaskingName AS AuthorMaskingName
				,cb.Description AS Description
				,PublishedDate 
				,cb.Views AS Views
				,(ISNULL(spc.FacebookCommentCount, 0))  AS FacebookCommentCount
				,(ISNULL(cb.IsSticky,0)) AS IsSticky
				,cei.HostURL AS HostUrl	
				,cei.ImagePathLarge AS LargePicUrl
				,cei.ImagePathThumbnail AS SmallPicUrl
				--,'' AS MakeId, '' AS ModelId, '' AS MakeName, '' AS ModelName, '' AS ModelMaskingName 	
				,M.Id AS MakeId, MO.ID AS ModelId, M.Name AS MakeName, MO.Name AS ModelName, MO.MaskingName AS ModelMaskingName
				,'1' AS Row_No
				,'1' as Row_Num
			FROM Con_EditCms_Basic AS CB WITH(NOLOCK)
			INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
			LEFT JOIN Con_EditCms_Images CEI  WITH(NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1
			LEFT JOIN SocialPluginsCount SPC WITH(NOLOCK) ON SPC.TypeId = CB.Id
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id AND C.IsActive = 1
			LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
			LEFT JOIN CarMakes M WITH(NOLOCK) ON M.ID = C.MakeId
			WHERE CB.ApplicationID = @ApplicationId AND CB.CategoryId IN (select ListMember from fnSplitCSVValuesWithIdentity(@CategoryList)) 
				AND CB.IsActive = 1 AND CB.IsPublished = 1 AND IsSticky = 1
				AND (CAST(GETDATE() AS DATE) BETWEEN CAST(StickyFromDate AS DATE)AND CAST(StickyToDate AS DATE))
				AND MO.IsDeleted = 0  AND M.IsDeleted = 0
			)
			UNION ALL
			(-- Normal Query
			SELECT  * FROM
			(
				SELECT *, ROW_NUMBER() OVER (ORDER BY DisplayDate DESC) AS Row_Num FROM(
				SELECT
					cb.Id AS BasicId
					,CategoryId AS CategoryId
					,cb.Title AS Title
					,cb.Url AS ArticleUrl
					,DisplayDate AS DisplayDate
					,CB.AuthorName AS AuthorName
					,CA.MaskingName AS AuthorMaskingName
					,cb.Description AS Description
					,PublishedDate 
					,cb.Views AS Views
					,(ISNULL(spc.FacebookCommentCount, 0))  AS FacebookCommentCount
					,(ISNULL(cb.IsSticky,0)) AS IsSticky
					,cei.HostURL AS HostUrl		
					,cei.ImagePathLarge AS LargePicUrl
					,cei.ImagePathThumbnail AS SmallPicUrl
					,M.Id AS MakeId, MO.ID AS ModelId, M.Name AS MakeName, MO.Name AS ModelName, MO.MaskingName AS ModelMaskingName
					,ROW_NUMBER() OVER (PARTITION BY C.BasicId ORDER BY DisplayDate DESC) AS Row_No		
				FROM Con_EditCms_Basic CB WITH (NOLOCK)
				INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId	
				LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1
				LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id		
				LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id AND C.IsActive = 1
				LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
				LEFT JOIN CarMakes M WITH(NOLOCK) ON M.ID = C.MakeId
				WHERE CB.CategoryId IN (select ListMember from fnSplitCSVValuesWithIdentity(@CategoryList))
				AND CB.ApplicationID = @ApplicationId AND CB.IsActive = 1 AND CB.IsPublished = 1
				AND CB.DisplayDate <= GETDATE()
				AND (IsSticky IS NULL OR IsSticky = 0 OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE))
				AND (@MakeId IS NULL OR M.Id = @MakeId)
				AND (@ModelId IS NULL OR MO.ID = @ModelId)
				AND MO.IsDeleted = 0  AND M.IsDeleted = 0
				) AS CTE1 WHERE Row_No = 1
			) CTE WHERE Row_Num BETWEEN @StartIndex AND @EndIndex
			) ORDER BY Row_Num
			END
			ELSE IF(@ApplicationId = 2)
			BEGIN
			-- Sticky Query.
			(	
			SELECT DISTINCT
				cb.Id AS BasicId
				,CategoryId AS CategoryId
				,cb.Title AS Title
				,cb.Url AS ArticleUrl
				,DisplayDate AS DisplayDate
				,CB.AuthorName AS AuthorName
				,CA.MaskingName AS AuthorMaskingName
				,cb.Description AS Description
				,PublishedDate 
				,cb.Views AS Views
				,(ISNULL(spc.FacebookCommentCount, 0))  AS FacebookCommentCount
				,(ISNULL(cb.IsSticky,0)) AS IsSticky
				,cei.HostURL AS HostUrl	
				,cei.ImagePathLarge AS LargePicUrl
				,cei.ImagePathThumbnail AS SmallPicUrl
				--,'' AS MakeId, '' AS ModelId, '' AS MakeName, '' AS ModelName, '' AS ModelMaskingName 	
				,M.Id AS MakeId, MO.ID AS ModelId, M.Name AS MakeName, MO.Name AS ModelName, MO.MaskingName AS ModelMaskingName
				,'1' AS Row_No
				,'1' as Row_Num
			FROM Con_EditCms_Basic AS CB WITH(NOLOCK)
			INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
			LEFT JOIN Con_EditCms_Images CEI  WITH(NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1
			LEFT JOIN SocialPluginsCount SPC WITH(NOLOCK) ON SPC.TypeId = CB.Id
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id AND C.IsActive = 1
			LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
			LEFT JOIN BikeMakes M WITH(NOLOCK) ON M.ID = C.MakeId
			WHERE CB.ApplicationID = @ApplicationId AND CB.CategoryId IN (select ListMember from fnSplitCSVValuesWithIdentity(@CategoryList)) 
				AND CB.IsActive = 1 AND CB.IsPublished = 1 AND IsSticky = 1
				AND (CAST(GETDATE() AS DATE) BETWEEN CAST(StickyFromDate AS DATE)AND CAST(StickyToDate AS DATE))
				AND MO.IsDeleted = 0  AND M.IsDeleted = 0
			)
			UNION ALL
			(-- Normal Query
			SELECT  * FROM
			(
				SELECT *, ROW_NUMBER() OVER (ORDER BY DisplayDate DESC) AS Row_Num FROM(
				SELECT
					cb.Id AS BasicId
					,CategoryId AS CategoryId
					,cb.Title AS Title
					,cb.Url AS ArticleUrl
					,DisplayDate AS DisplayDate
					,CB.AuthorName AS AuthorName
					,CA.MaskingName AS AuthorMaskingName
					,cb.Description AS Description
					,PublishedDate 
					,cb.Views AS Views
					,(ISNULL(spc.FacebookCommentCount, 0))  AS FacebookCommentCount
					,(ISNULL(cb.IsSticky,0)) AS IsSticky
					,cei.HostURL AS HostUrl		
					,cei.ImagePathLarge AS LargePicUrl
					,cei.ImagePathThumbnail AS SmallPicUrl
					,M.Id AS MakeId, MO.ID AS ModelId, M.Name AS MakeName, MO.Name AS ModelName, MO.MaskingName AS ModelMaskingName
					,ROW_NUMBER() OVER (PARTITION BY C.BasicId ORDER BY DisplayDate DESC) AS Row_No		
				FROM Con_EditCms_Basic CB WITH (NOLOCK)
				INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId	
				LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id AND CEI.IsMainImage = 1 AND CEI.IsActive = 1
				LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id		
				LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id AND C.IsActive = 1
				LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
				LEFT JOIN BikeMakes M WITH(NOLOCK) ON M.ID = C.MakeId
				WHERE CB.CategoryId IN (select ListMember from fnSplitCSVValuesWithIdentity(@CategoryList))
				AND CB.ApplicationID = @ApplicationId AND CB.IsActive = 1 AND CB.IsPublished = 1
				AND CB.DisplayDate <= GETDATE()
				AND (IsSticky IS NULL OR IsSticky = 0 OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE))
				AND (@MakeId IS NULL OR M.Id = @MakeId)
				AND (@ModelId IS NULL OR MO.ID = @ModelId)
				AND MO.IsDeleted = 0  AND M.IsDeleted = 0
				) AS CTE1 WHERE Row_No = 1
			) CTE WHERE Row_Num BETWEEN @StartIndex AND @EndIndex
			) ORDER BY Row_Num
			END

			-- Record Count Query
			SELECT COUNT(DISTINCT CB.Id) AS RecordCount
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id AND C.IsActive = 1
			LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
			LEFT JOIN CarMakes M WITH(NOLOCK) ON M.ID = C.MakeId
			WHERE CB.CategoryId IN (select ListMember from fnSplitCSVValuesWithIdentity(@CategoryList))
			AND CB.IsPublished = 1 AND CB.ApplicationID = @ApplicationId AND CB.IsActive = 1
			AND (@MakeId IS NULL OR M.Id = @MakeId)
			AND (@ModelId IS NULL OR MO.ID = @ModelId)
			AND MO.IsDeleted = 0 AND M.IsDeleted = 0 

END
