IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFeaturedArticles_V14111]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFeaturedArticles_V14111]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the featured articles  with the given contentTypes as categoryids as input parameter
-- modified by natesh kumar for fetching make and model from carwale and bikewale resp.
-- modified by natesh kumar checking isdeleted flag on join on 1/12/14
--================================================================
-- exec dbo.GetFeaturedArticles_V14111 1,'8',50
CREATE PROCEDURE [dbo].[GetFeaturedArticles_V14111]

		@ApplicationId INT
       ,@contentTypes VARCHAR(50) 
	   ,@totalRecords INT
AS

BEGIN
	IF(@ApplicationId = 1)
		BEGIN
			 WITH cte
			AS (
				SELECT cb.Id AS BasicId
					,CategoryId AS CategoryId
					,cb.Title AS Title
					,cb.Url AS ArticleUrl
					,DisplayDate AS DisplayDate
					,AuthorName AS AuthorName
					,cb.Description AS Description
					,PublishedDate
					,VIEWS AS VIEWS
					,cei.HostURL AS HostUrl
					,(ISNULL(cb.IsSticky,0))  AS IsSticky
					,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
					,cei.ImagePathLarge AS LargePicUrl
					,cei.ImagePathThumbnail AS SmallPicUrl
					,cmo.MaskingName AS ModelMaskingName
					,Cmo.NAME AS ModelName
					,Cma.NAME AS MakeName
					,ROW_NUMBER() OVER (
						PARTITION BY cb.Id ORDER BY cb.DisplayDate DESC
						) rowno
				FROM con_editcms_basic cb  WITH (NOLOCK)
				LEFT JOIN Con_EditCms_Images cei WITH (NOLOCK) ON cb.Id = cei.BasicId
					AND cei.IsMainImage = 1
					AND cei.IsActive = 1
				LEFT JOIN Con_EditCms_Cars cec WITH (NOLOCK) ON cec.BasicId = cb.Id
				LEFT JOIN CarModels Cmo WITH (NOLOCK) ON Cmo.ID = cec.ModelId
				LEFT JOIN CarMakes Cma WITH (NOLOCK) ON Cma.ID = cec.MakeId
				LEFT JOIN SocialPluginsCount spc WITH (NOLOCK) ON cb.Id = spc.TypeId
				WHERE --cb.IsFeatured = 1  
				     ((ImagePathCustom IS NOT NULL AND @contentTypes ='1') OR ImagePathCustom IS NOT NULL OR ImagePathCustom IS NULL)
					AND cb.ApplicationID = @ApplicationId
					AND cb.IsActive = 1
					AND cb.IsPublished = 1
					AND ISNULL(Cmo.IsDeleted, 0) = 0
					AND ISNULL(Cma.IsDeleted, 0) = 0
					AND cb.CategoryId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@contentTypes)
						)
					
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
					,CategoryId AS CategoryId
					,cb.Title AS Title
					,cb.Url AS ArticleUrl
					,DisplayDate AS DisplayDate
					,AuthorName AS AuthorName
					,cb.Description AS Description
					,PublishedDate
					,VIEWS AS VIEWS
					,cei.HostURL AS HostUrl
					,(ISNULL(cb.IsSticky,0))  AS IsSticky
					,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
					,cei.ImagePathCustom AS LargePicUrl
					,cei.ImagePathThumbnail AS SmallPicUrl
					,cmo.MaskingName AS ModelMaskingName
					,Cmo.NAME AS ModelName
					,Cma.NAME AS MakeName
					,ROW_NUMBER() OVER (
						PARTITION BY cb.Id ORDER BY cb.DisplayDate DESC
						) rowno
				FROM con_editcms_basic cb  WITH (NOLOCK)
				LEFT JOIN Con_EditCms_Images cei WITH (NOLOCK) ON cb.Id = cei.BasicId
					AND cei.IsMainImage = 1
					AND cei.IsActive = 1
				LEFT JOIN Con_EditCms_Cars cec WITH (NOLOCK) ON cec.BasicId = cb.Id
				LEFT JOIN BikeModels Cmo WITH (NOLOCK) ON Cmo.ID = cec.ModelId
				LEFT JOIN BikeMakes Cma WITH (NOLOCK) ON Cma.ID = cec.MakeId 	
				LEFT JOIN SocialPluginsCount spc WITH (NOLOCK) ON cb.Id = spc.TypeId
				WHERE cb.IsFeatured = 1
					AND cb.ApplicationID = @ApplicationId
					AND cb.IsActive = 1
					AND cb.IsPublished = 1
					AND ISNULL(Cmo.IsDeleted, 0) = 0
					AND ISNULL(Cma.IsDeleted, 0) = 0
					AND cb.CategoryId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@contentTypes)
						)
					
				)
			SELECT TOP (@totalRecords) *
			FROM cte
			WHERE rowno = 1
			ORDER BY DisplayDate DESC	
		END							

END





/****** Object:  StoredProcedure [dbo].[GetRelatedArticles]    Script Date: 2/16/2015 5:09:36 PM ******/
-- SET ANSI_NULLS ON
