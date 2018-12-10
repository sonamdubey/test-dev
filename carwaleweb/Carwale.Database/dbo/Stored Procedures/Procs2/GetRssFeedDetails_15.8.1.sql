IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRssFeedDetails_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetRssFeedDetails_15]
GO

	--================================================
-- Ceated by Natesh Kumar on 9/12/14
-- for getting rss related details with appid , categoryidlist and noofdays as input parameters
--Modified By : Shalini Nair on 12/08/15 retrieving OriginalImgPath
--================================================
-- exec [dbo].[GetRssFeedDetails] 1,'1',80
CREATE PROCEDURE [dbo].[GetRssFeedDetails_15.8.1]
@ApplicationId TINYINT
	,@CategoryList VARCHAR(50)
	,@NoOfDays INT
	,@MakeId INT = NULL
	,@ModelId INT = NULL
AS
BEGIN
	IF (@ApplicationId = 1)
	BEGIN
		
			-- Normal Query
			SELECT *
			FROM (
				SELECT *
					,ROW_NUMBER() OVER (
						ORDER BY DisplayDate DESC
						) AS Row_Num
				FROM (
					SELECT cb.Id AS BasicId
						,CategoryId AS CategoryId
						,cb.Title AS Title
						,cb.Url AS ArticleUrl
						,DisplayDate AS DisplayDate
						,cb.Description AS Description
						,CB.AuthorName AS AuthorName
						,CB.Views AS Views
						,CPC.Data AS Content
						,CEI.ImagePathLarge AS LargePicUrl
						,CEI.ImagePathThumbnail As SmallPicUrl 
						,CEI.HostURL AS HostUrl
						,CEI.OriginalImgPath 
						,CB.MainImgCaption As ImgCaption
						,ROW_NUMBER() OVER (
							PARTITION BY CB.Id ORDER BY DisplayDate DESC
							) AS Row_No
					FROM Con_EditCms_Basic CB WITH (NOLOCK)
					INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
					LEFT JOIN Con_EditCms_Pages CP WITH (NOLOCK)  ON CP.BasicId = CB.Id
					LEFT JOIN Con_EditCms_PageContent CPC  WITH (NOLOCK) ON CPC.PageId = CP.Id
					LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
						AND CEI.IsMainImage = 1
						AND CEI.IsActive = 1
					LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
						AND C.IsActive = 1
					LEFT JOIN CarModels MO WITH (NOLOCK) ON MO.ID = C.ModelId
					LEFT JOIN CarMakes M WITH (NOLOCK) ON M.ID = C.MakeId
					WHERE CB.CategoryId IN (
							SELECT ListMember
							FROM fnSplitCSVValuesWithIdentity(@CategoryList)
							)
						AND CB.ApplicationID = @ApplicationId
						AND CB.IsActive = 1
						AND CB.IsPublished = 1
						AND ISNULL(MO.IsDeleted, 0) = 0
						AND ISNULL(M.IsDeleted, 0) = 0
						AND CB.DisplayDate <= GETDATE()
						AND CB.DisplayDate >= DATEADD(DAY, -@NoOfDays, GETDATE())
						AND (
							IsSticky IS NULL
							OR IsSticky = 0
							OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE)
							)
						AND (
							@MakeId IS NULL
							OR M.Id = @MakeId
							)
						AND (
							@ModelId IS NULL
							OR MO.ID = @ModelId
							)
					) AS CTE1
				WHERE Row_No = 1
				)AS CTE
					ORDER BY Row_Num

	END
	ELSE
		IF (@ApplicationId = 2)
		BEGIN
		-- Normal Query
				SELECT *
				FROM (
					SELECT *
						,ROW_NUMBER() OVER (
							ORDER BY DisplayDate DESC
							) AS Row_Num
					FROM (
						SELECT cb.Id AS BasicId
								,CategoryId AS CategoryId
								,cb.Title AS Title
								,cb.Url AS ArticleUrl
								,DisplayDate AS DisplayDate
								,cb.Description AS Description
								,CB.AuthorName AS AuthorName
								,CB.Views AS Views
								,CPC.Data AS Content
								,CEI.ImagePathLarge AS LargePicUrl
								,CEI.ImagePathThumbnail As SmallPicUrl 
								,CEI.HostURL AS HostUrl
								,CEI.OriginalImgPath
								,CB.MainImgCaption As ImgCaption
								,ROW_NUMBER() OVER (
								PARTITION BY CB.Id ORDER BY DisplayDate DESC
								) AS Row_No
						FROM Con_EditCms_Basic CB WITH (NOLOCK)
						INNER JOIN Con_EditCms_Author CA WITH (NOLOCK) ON CA.Authorid = CB.AuthorId
						LEFT JOIN Con_EditCms_Pages CP WITH (NOLOCK)  ON CP.BasicId = CB.Id
						LEFT JOIN Con_EditCms_PageContent CPC  WITH (NOLOCK) ON CPC.PageId = CP.Id
						LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
							AND CEI.IsMainImage = 1
							AND CEI.IsActive = 1
						LEFT JOIN Con_EditCms_Cars C WITH (NOLOCK) ON C.BasicId = CB.Id
							AND C.IsActive = 1
						LEFT JOIN BikeModels MO WITH (NOLOCK) ON MO.ID = C.ModelId 
						LEFT JOIN BikeMakes M WITH (NOLOCK) ON M.ID = C.MakeId 
						WHERE CB.CategoryId IN (
								SELECT ListMember
								FROM fnSplitCSVValuesWithIdentity(@CategoryList)
								)
							AND CB.ApplicationID = @ApplicationId
							AND CB.IsActive = 1
							AND CB.IsPublished = 1
							AND ISNULL(MO.IsDeleted, 0) = 0
							AND ISNULL(M.IsDeleted, 0) = 0
							AND CB.DisplayDate <= GETDATE()
							AND CB.DisplayDate >= DATEADD(DAY, -@NoOfDays, GETDATE())
							AND (
								IsSticky IS NULL
								OR IsSticky = 0
								OR CAST(StickyToDate AS DATE) < CAST(GETDATE() AS DATE)
								)
							AND (
								@MakeId IS NULL
								OR M.Id = @MakeId
								)
							AND (
								@ModelId IS NULL
								OR MO.ID = @ModelId
								)
						) AS CTE1
					WHERE Row_No = 1
					)AS CTE
						ORDER BY Row_Num

	
		END

END


