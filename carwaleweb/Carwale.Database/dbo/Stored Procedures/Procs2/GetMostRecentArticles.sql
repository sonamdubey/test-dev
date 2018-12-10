IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMostRecentArticles]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMostRecentArticles]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the most recent articles  with the given contentTypes as categoryids , applicationid, totalrecords , makeid and modelid as input parameter
--================================================================
-- exec dbo.GetMostRecentArticles 1,1,50
CREATE PROCEDURE [dbo].[GetMostRecentArticles] 
	@ApplicationId INT
	,@contentTypes VARCHAR(50)
	,@totalRecords INT
	,@MakeId INT = NULL
	,@ModelId INT = NULL
AS

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
				,ROW_NUMBER() OVER (
					PARTITION BY cb.Id ORDER BY cb.DisplayDate DESC
					) rowno
			FROM Con_EditCms_Basic cb WITH (NOLOCK) 
			LEFT JOIN Con_EditCms_Images cei WITH (NOLOCK) ON cb.Id = cei.BasicId
			LEFT JOIN SocialPluginsCount spc WITH (NOLOCK) ON cb.Id = spc.TypeId
			LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
				AND C.IsActive = 1
			LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
			LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = C.MakeId
			WHERE 
				cb.ApplicationID = @ApplicationId
				AND cb.IsActive = 1
				AND cb.IsPublished = 1
				AND MO.IsDeleted = 0
				AND M.IsDeleted = 0
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
