IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContentPages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContentPages]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the page content with given basicid as input parameter
-- modified by natesh kumar checking imgactive flag on join on 26/11/14
--================================================================
-- exec dbo.GetContentPages 13637

CREATE PROCEDURE [dbo].[GetContentPages]
@BasicId INT
AS 
BEGIN

			SELECT 
			cb.Id AS BasicId
			,CB.CategoryId AS CategoryId
			,cb.Title AS Title
			,cb.Url AS ArticleUrl
			,DisplayDate AS DisplayDate
			,CB.AuthorName AS AuthorName
			,cb.Description AS Description
			,PublishedDate 
			,cb.Views AS Views
			,(ISNULL(cb.IsSticky,0)) AS IsSticky
			,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
			,cei.HostURL AS HostUrl
			,cei.ImagePathLarge AS LargePicUrl
			,cei.ImagePathThumbnail AS SmallPicUrl 
			,CB.MainImgCaption AS MainImgCaption
			,CS.Name AS SubCategoryName
			,CB.ShowGallery As ShowGallery
			,CA.MaskingName AS AuthorMaskingName
			,(ISNULL(CB.MainImageSet,0)) AS IsMainImageSet
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			LEFT JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CB.AuthorId = CA.Authorid
			LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CB.Id = CEI.BasicId AND CEI.IsMainImage = 1 AND CEI.IsActive = 1
			LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON CB.Id = SPC.TypeId
			LEFT JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CB.Id = CBS.BasicId
			LEFT JOIN Con_EditCms_SubCategories CS WITH (NOLOCK) ON CS.Id = CBS.SubCategoryId
			WHERE CB.ApplicationID = (SELECT CB.ApplicationID FROM Con_EditCms_Basic CB WITH (NOLOCK) WHERE CB.Id = @BasicId) AND CB.Id = @BasicId AND CB.IsActive = 1 AND CB.IsPublished = 1

			-- Get Page Data
			SELECT CPC.PageId AS pageId, CP.Priority AS Priority, CP.PageName AS PageName, CPC.Data AS Content
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK) ON CB.Id = CP.BasicId AND CP.IsActive = 1
			INNER JOIN Con_EditCms_PageContent CPC WITH (NOLOCK) ON CP.Id = CPC.PageId
			WHERE CB.ApplicationID = (SELECT CB.ApplicationID FROM Con_EditCms_Basic CB WITH (NOLOCK) WHERE CB.Id = @BasicId) AND CB.Id = @BasicId AND CP.IsActive = 1 AND CB.IsActive = 1 AND CB.IsPublished = 1
			ORDER BY CP.Priority ASC

			----- for tag
			select CET.Tag
			from Con_EditCms_Tags CET WITH (NOLOCK)
			INNER JOIN Con_EditCms_BasicTags BT WITH (NOLOCK) ON BT.TagId = CET.Id
			INNER JOIN Con_EditCms_Basic CB WITH (NOLOCK) ON BT.BasicId = CB.Id
			WHERE CB.Id = @BasicId AND CB.ApplicationID= (SELECT CB.ApplicationID FROM Con_EditCms_Basic CB  WITH (NOLOCK) WHERE CB.Id = @BasicId) AND CB.IsActive = 1 AND CB.IsPublished = 1

			-- Get Vehicles tagged
			IF((SELECT CB.ApplicationID FROM Con_EditCms_Basic CB WITH (NOLOCK) WHERE CB.Id = @BasicId) = 1)
			BEGIN
				-- For Vehicles tags. (CarWale)
				SELECT C.MakeId, C.ModelId, C.VersionId, 
				M.Name AS MakeName, MO.Name AS ModelName, V.Name AS VersionName,
				MO.MaskingName AS ModelMaskName
				FROM Con_EditCms_Cars C WITH (NOLOCK)
				LEFT JOIN CarVersions V WITH (NOLOCK) ON V.ID = C.VersionId
				INNER JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
				INNER JOIN CarMakes M WITH (NOLOCK) ON M.ID = C.MakeId
				INNER JOIN Con_EditCms_Basic B WITH (NOLOCK) ON B.ID = C.BasicId AND B.IsActive = 1 AND B.IsPublished = 1 AND C.IsActive = 1
				WHERE B.Id = @BasicId AND B.ApplicationID= (SELECT CB.ApplicationID FROM Con_EditCms_Basic CB WITH (NOLOCK) WHERE CB.Id = @BasicId)
			END
			ELSE IF((SELECT CB.ApplicationID FROM Con_EditCms_Basic CB WHERE CB.Id = @BasicId) = 2)
			BEGIN
			-- For Vehicles tags. (BikeWale)
			SELECT C.MakeId, C.ModelId, C.VersionId, M.Name AS MakeName, 
					MO.Name AS ModelName, V.Name AS VersionName,
					MO.MaskingName AS ModelMaskName
			FROM Con_EditCms_Cars C WITH (NOLOCK)
			LEFT JOIN BikeVersions V WITH (NOLOCK) ON V.ID = C.VersionId
			INNER JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
			INNER JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId
			INNER JOIN Con_EditCms_Basic B WITH (NOLOCK) ON B.ID = C.BasicId AND B.IsActive = 1 AND B.IsPublished = 1 AND C.IsActive = 1
			WHERE B.Id = @BasicId AND B.ApplicationID= (SELECT CB.ApplicationID FROM Con_EditCms_Basic CB WITH (NOLOCK) WHERE CB.Id = @BasicId)
			END

END





/****** Object:  StoredProcedure [dbo].[GetContentListByCategory_V14111]    Script Date: 2/16/2015 5:07:31 PM ******/
-- SET ANSI_NULLS ON
