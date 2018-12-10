IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFeaturedArticles_v15_8_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFeaturedArticles_v15_8_1]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the featured articles  with the given contentTypes as categoryids as input parameter
-- modified by natesh kumar for fetching make and model from carwale and bikewale resp.
-- modified by natesh kumar checking isdeleted flag on join on 1/12/14
-- Modified by Satish Sharma On 23/Jul/2015, Image dynamic resize revamp
--================================================================
-- exec dbo.GetFeaturedArticles_V14111 1,'8',50
CREATE PROCEDURE [dbo].[GetFeaturedArticles_v15_8_1]

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
					,cb.CategoryId AS CategoryId
					,cb.Title AS Title
					,cb.Url AS ArticleUrl
					,cb.DisplayDate AS DisplayDate
					,cb.AuthorName AS AuthorName
					,cb.Description AS Description
					,cb.PublishedDate
					,cb.VIEWS AS VIEWS
					,cei.HostURL AS HostUrl
					,(ISNULL(cb.IsSticky,0))  AS IsSticky
					,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
					--,cei.ImagePathLarge AS LargePicUrl
					--,cei.ImagePathThumbnail AS SmallPicUrl
					,CEI.OriginalImgPath AS OrginalImgPath
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
				WHERE cb.IsFeatured = 1  AND ((ImagePathCustom IS NOT NULL AND @contentTypes ='1') OR ImagePathCustom IS NOT NULL OR ImagePathCustom IS NULL)
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
					,cb.CategoryId AS CategoryId
					,cb.Title AS Title
					,cb.Url AS ArticleUrl
					,cb.DisplayDate AS DisplayDate
					,cb.AuthorName AS AuthorName
					,cb.Description AS Description
					,cb.PublishedDate
					,cb.VIEWS AS VIEWS
					,cei.HostURL AS HostUrl
					,(ISNULL(cb.IsSticky,0))  AS IsSticky
					,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
					--,cei.ImagePathCustom AS LargePicUrl
					--,cei.ImagePathThumbnail AS SmallPicUrl
					,CEI.OriginalImgPath AS OrginalImgPath
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


