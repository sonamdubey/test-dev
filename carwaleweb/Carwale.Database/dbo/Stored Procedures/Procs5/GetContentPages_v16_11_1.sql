USE [CarWale]
GO
/****** Object:  StoredProcedure [dbo].[GetContentPages_v16_11_1]    Script Date: 11/29/2016 11:03:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--================================================================
-- Author: Natesh Kumar on 28/8/14 
-- Description: gets the page content with given basicid as input parameter
-- modified by natesh kumar checking imgactive flag on join on 26/11/14
-- Modified by Satish Sharma On 23/Jul/2015, Image dynamic resize revamp
--================================================================
-- exec dbo.GetContentPages_15_8_1 17936
-- Modified by Satish Sharma On 23/Jul/2015, Image dynamic resize revamp
-- Modified by Sachin Bharti On 5/10/2015, Added categorymaskingname
-- Modified by Rakesh Yadav On 20 jun 2016,added makeMasking Name and removed join with Con_EditCms_Images table
-- Modified by Meet Shah to get Prev and Next Article 
CREATE PROCEDURE [dbo].[GetContentPages_v16_11_1] @BasicId INT
AS
BEGIN
	--  Get Application id based on basic id
	DECLARE @PublishedDate DATETIME
	DECLARE @ApplicationId TINYINT

	SELECT @ApplicationId = CB.ApplicationID
		,@PublishedDate = CB.PublishedDate
	FROM Con_EditCms_Basic CB WITH (NOLOCK)
	WHERE CB.Id = @BasicId

	SELECT cb.Id AS BasicId
		,CB.CategoryId AS CategoryId
		,cb.Title AS Title
		,cb.Url AS ArticleUrl
		,DisplayDate AS DisplayDate
		,CB.AuthorName AS AuthorName
		,cb.Description AS Description
		,CB.PublishedDate
		,cb.VIEWS AS VIEWS
		,(ISNULL(cb.IsSticky, 0)) AS IsSticky
		,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
		,CB.HostURL AS HostUrl
		--,cei.ImagePathLarge AS LargePicUrl
		--,cei.ImagePathThumbnail AS SmallPicUrl 
		,CB.MainImagePath AS OrginalImgPath
		,CB.MainImgCaption AS MainImgCaption
		,CS.NAME AS SubCategoryName
		,CB.ShowGallery AS ShowGallery
		,CA.MaskingName AS AuthorMaskingName
		,(ISNULL(CB.MainImageSet, 0)) AS IsMainImageSet
		,CC.CategoryMaskingName
	FROM Con_EditCms_Basic CB WITH (NOLOCK)
	LEFT JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CB.AuthorId = CA.Authorid
	--LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CB.Id = CEI.BasicId
		--AND CEI.IsMainImage = 1
		--AND CEI.IsActive = 1
	LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON CB.Id = SPC.TypeId
	LEFT JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CB.Id = CBS.BasicId
	LEFT JOIN Con_EditCms_SubCategories CS WITH (NOLOCK) ON CS.Id = CBS.SubCategoryId
	LEFT JOIN Con_EditCms_Category CC WITH (NOLOCK) ON CC.Id = CB.CategoryId
	WHERE CB.ApplicationID = (
			SELECT CB.ApplicationID
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			WHERE CB.Id = @BasicId
			)
		AND CB.Id = @BasicId
		AND CB.IsActive = 1
		AND CB.IsPublished = 1

	-- Get Page Data
	SELECT CPC.PageId AS pageId
		,CP.Priority AS Priority
		,CP.PageName AS PageName
		,CPC.Data AS Content
	FROM Con_EditCms_Basic CB WITH (NOLOCK)
	INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK) ON CB.Id = CP.BasicId
		AND CP.IsActive = 1
	INNER JOIN Con_EditCms_PageContent CPC WITH (NOLOCK) ON CP.Id = CPC.PageId
	WHERE CB.ApplicationID = (
			SELECT CB.ApplicationID
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			WHERE CB.Id = @BasicId
			)
		AND CB.Id = @BasicId
		AND CP.IsActive = 1
		AND CB.IsActive = 1
		AND CB.IsPublished = 1
	ORDER BY CP.Priority ASC

	----- for tag
	SELECT CET.Tag
	FROM Con_EditCms_Tags CET WITH (NOLOCK)
	INNER JOIN Con_EditCms_BasicTags BT WITH (NOLOCK) ON BT.TagId = CET.Id
	INNER JOIN Con_EditCms_Basic CB WITH (NOLOCK) ON BT.BasicId = CB.Id
	WHERE CB.Id = @BasicId
		AND CB.ApplicationID = (
			SELECT CB.ApplicationID
			FROM Con_EditCms_Basic CB WITH (NOLOCK)
			WHERE CB.Id = @BasicId
			)
		AND CB.IsActive = 1
		AND CB.IsPublished = 1

	-- Get Vehicles tagged
	IF (
			(
				SELECT CB.ApplicationID
				FROM Con_EditCms_Basic CB WITH (NOLOCK)
				WHERE CB.Id = @BasicId
				) = 1
			)
	BEGIN
		-- For Vehicles tags. (CarWale)
		SELECT C.MakeId
			,C.ModelId
			,C.VersionId
			,M.NAME AS MakeName
			,MO.NAME AS ModelName
			,V.NAME AS VersionName
			,MO.MaskingName AS ModelMaskName
			,null AS MakeMaskingName
		FROM Con_EditCms_Cars C WITH (NOLOCK)
		LEFT JOIN CarVersions V WITH (NOLOCK) ON V.ID = C.VersionId
		INNER JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
		INNER JOIN CarMakes M WITH (NOLOCK) ON M.ID = C.MakeId
		INNER JOIN Con_EditCms_Basic B WITH (NOLOCK) ON B.ID = C.BasicId
			AND B.IsActive = 1
			AND B.IsPublished = 1
			AND C.IsActive = 1
		WHERE B.Id = @BasicId
			AND B.ApplicationID = (
				SELECT CB.ApplicationID
				FROM Con_EditCms_Basic CB WITH (NOLOCK)
				WHERE CB.Id = @BasicId
				)
	END
	ELSE
		IF (
				(
					SELECT CB.ApplicationID
					FROM Con_EditCms_Basic CB WITH (NOLOCK)
					WHERE CB.Id = @BasicId
					) = 2
				)
		BEGIN
			-- For Vehicles tags. (BikeWale)
			SELECT C.MakeId
				,C.ModelId
				,C.VersionId
				,M.NAME AS MakeName
				,MO.NAME AS ModelName
				,V.NAME AS VersionName
				,MO.MaskingName AS ModelMaskName
				,M.MaskingName As MakeMaskingName
			FROM Con_EditCms_Cars C WITH (NOLOCK)
			LEFT JOIN BikeVersions V WITH (NOLOCK) ON V.ID = C.VersionId
			INNER JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
			INNER JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId
			INNER JOIN Con_EditCms_Basic B WITH (NOLOCK) ON B.ID = C.BasicId
				AND B.IsActive = 1
				AND B.IsPublished = 1
				AND C.IsActive = 1
			WHERE B.Id = @BasicId
				AND B.ApplicationID = (
					SELECT CB.ApplicationID
					FROM Con_EditCms_Basic CB WITH (NOLOCK)
					WHERE CB.Id = @BasicId
					)
		END

	-- For previous article url
	SELECT TOP 1 CB.Id AS BasicId
		,CB.Title AS Title
		,CB.Url AS ArticleUrl
	FROM CON_EDITCMS_BASIC CB WITH (NOLOCK)
	INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK) ON CP.BasicId = CB.Id
	WHERE CB.PublishedDate >= @PublishedDate
		AND CB.ID > @BasicId
		AND CB.ApplicationID = @ApplicationId
		AND CB.IsActive = 1
		AND CB.IsPublished = 1
		AND CB.CategoryId = (
			SELECT cb.CategoryId
			FROM Con_EditCms_Basic cb WITH (NOLOCK)
			WHERE cb.Id = @BasicId
			)
		AND CB.PublishedDate <= GETDATE()
	ORDER BY CB.PublishedDate

	--For next article url 
	SELECT TOP 1 CB.Id AS BasicId
		,CB.Title AS Title
		,CB.Url AS ArticleUrl -- added by jitendra
	FROM CON_EDITCMS_BASIC CB WITH (NOLOCK)
	INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK) ON CP.BasicId = CB.Id
	WHERE CB.PublishedDate <= @PublishedDate
		AND CB.ID < @BasicId
		AND CB.ApplicationID = @ApplicationId
		AND CB.IsActive = 1
		AND CB.IsPublished = 1
		AND CB.CategoryId = (
			SELECT cb.CategoryId
			FROM Con_EditCms_Basic cb WITH (NOLOCK)
			WHERE cb.Id = @BasicId
			)
	--AND CB.PublishedDate <= GETDATE()
	ORDER BY CB.PublishedDate DESC
END
