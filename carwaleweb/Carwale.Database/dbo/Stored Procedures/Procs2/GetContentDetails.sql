IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContentDetails]
GO

	--================================================================
-- Author: Natesh Kumar on 28/8/14 
--Description: gets the details about the article with given basicid as input parameter
-- modified by natesh kumar checking imgactive flag on join on 26/11/14
--================================================================
-- exec dbo.GetContentDetails 14215
CREATE PROCEDURE [dbo].[GetContentDetails]

@BasicId INT

AS 

BEGIN
		
		--  Get Application id based on basic id
		Declare @PublishedDate DateTime
		Declare @ApplicationId TinyInt 
		SELECT @ApplicationId = CB.ApplicationID, @PublishedDate = CB.PublishedDate FROM Con_EditCms_Basic CB  WITH (NOLOCK) WHERE CB.Id = @BasicId
		

		SELECT  
		CB.Id AS BasicId
		,CategoryId AS CategoryId
		,CB.Title AS Title
		,CB.Url AS ArticleUrl
		,cb.Description AS Description
		,CB.AuthorName AS AuthorName
		,CA.MaskingName AS AuthorMaskName
		,DisplayDate AS DisplayDate
		,PublishedDate 
		,CB.Views AS Views
		,(ISNULL(cb.IsSticky,0)) AS IsSticky
		,CEI.HostURL AS HostUrl
		,(ISNULL(spc.FacebookCommentCount, 0)) AS FacebookCommentCount
		,CEI.ImagePathLarge AS LargePicUrl
		,CEI.ImagePathThumbnail AS SmallPicUrl 
		,CPC.Data AS Content
		FROM Con_EditCms_Basic CB WITH (NOLOCK)
		INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK)  ON CP.BasicId = CB.Id
		INNER JOIN Con_EditCms_PageContent CPC  WITH (NOLOCK) ON CPC.PageId = CP.Id
		LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) on CEI.BasicId = CB.Id AND CEI.IsMainImage = 1  AND CEI.IsActive = 1
		LEFT JOIN SocialPluginsCount SPC WITH (NOLOCK) ON SPC.TypeId = CB.Id
		INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CB.AuthorId = CA.Authorid
		WHERE CB.Id = @BasicId AND CB.ApplicationID = @ApplicationId AND CB.IsActive = 1 AND CB.IsPublished = 1

		
		-- For previous article url
		SELECT TOP 1  CB.Id AS BasicId ,CB.Title AS Title, CB.Url AS ArticleUrl
		FROM CON_EDITCMS_BASIC CB WITH (NOLOCK)
		INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK) ON CP.BasicId = CB.Id
		WHERE CB.PublishedDate >= @PublishedDate AND  CB.ID > @BasicId 	AND CB.ApplicationID = @ApplicationId
			AND CB.IsActive = 1 AND CB.IsPublished = 1
			AND CB.CategoryId = (SELECT cb.CategoryId FROM Con_EditCms_Basic cb WITH (NOLOCK) where cb.Id = @BasicId)
			AND CB.PublishedDate <= GETDATE()
		ORDER BY CB.PublishedDate


		--For next article url 
		SELECT TOP 1 CB.Id AS BasicId, CB.Title AS Title, CB.Url AS ArticleUrl
		FROM CON_EDITCMS_BASIC CB WITH (NOLOCK)
		INNER JOIN Con_EditCms_Pages CP WITH (NOLOCK) ON CP.BasicId = CB.Id
		WHERE CB.PublishedDate <= @PublishedDate AND CB.ID < @BasicId AND CB.ApplicationID = @ApplicationId
			AND CB.IsActive = 1 AND CB.IsPublished = 1
			AND CB.CategoryId = (SELECT cb.CategoryId FROM Con_EditCms_Basic cb WITH (NOLOCK) where cb.Id = @BasicId)
			AND CB.PublishedDate <= GETDATE()
		ORDER BY CB.PublishedDate DESC

		--- for tag
		select CET.Tag
		from Con_EditCms_Tags CET WITH (NOLOCK)
		INNER JOIN Con_EditCms_BasicTags CEBT WITH (NOLOCK) ON CEBT.TagId = CET.Id
		INNER JOIN Con_EditCms_Basic CB WITH (NOLOCK) ON CEBT.BasicId = CB.Id AND CB.IsActive = 1 AND CB.IsPublished = 1
		WHERE CB.Id = @BasicId AND CB.ApplicationID= @ApplicationId

		IF(@ApplicationId = 1)
		BEGIN
			-- For Vehicles tags. (CarWale)
			SELECT C.MakeId, C.ModelId, C.VersionId, 
			M.Name AS MakeName, MO.Name AS ModelName, V.Name AS VersionName,
			MO.MaskingName AS ModelMaskName
			FROM Con_EditCms_Cars C WITH (NOLOCK)
			LEFT JOIN CarVersions V  WITH (NOLOCK) ON V.ID = C.VersionId
			INNER JOIN CarModels MO  WITH (NOLOCK) ON MO.ID = C.ModelId
			INNER JOIN CarMakes M  WITH (NOLOCK) ON M.ID = C.MakeId
			INNER JOIN Con_EditCms_Basic B  WITH (NOLOCK) ON B.ID = C.BasicId AND B.IsActive = 1 AND B.IsPublished = 1 AND C.IsActive = 1
			WHERE B.Id = @BasicId AND B.ApplicationID= @ApplicationId
		END
		ELSE IF(@ApplicationId = 2)
		BEGIN
		-- For Vehicles tags. (BikeWale)
		SELECT C.MakeId, C.ModelId, C.VersionId, M.Name AS MakeName, 
				MO.Name AS ModelName, V.Name AS VersionName,
				MO.MaskingName AS ModelMaskName
		FROM Con_EditCms_Cars C WITH (NOLOCK)
		LEFT JOIN BikeVersions V  WITH (NOLOCK) ON V.ID = C.VersionId
		INNER JOIN BikeModels MO  WITH (NOLOCK) ON MO.ID = C.ModelId
		INNER JOIN BikeMakes M  WITH (NOLOCK) ON M.ID = C.MakeId
		INNER JOIN Con_EditCms_Basic B  WITH (NOLOCK) ON B.ID = C.BasicId AND B.IsActive = 1 AND B.IsPublished = 1 AND C.IsActive = 1
		WHERE B.Id = @BasicId AND B.ApplicationID= @ApplicationId
		END



END


