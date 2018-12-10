IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetGalleryImages_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetGalleryImages_v15]
GO

	
-- =============================================    
-- Author:  Vikas    
-- Create date: 20/04/2012    
-- Description: To get the details of the images that need to be shown in the Image Gallery    
-- 01-10-2012 Modified by Vikas Replace(CEI.Caption, '''', '&rsquo;') As Caption    
-- exec [dbo].[GetGalleryImages] 358,0
-- Modified By:prashant Vishe On 12 March 2014
-- modification:records sorted in ascending order of sequence
-- Modified By : Supriya Khartode On 30/7/2014 to add ApplicationId filter
-- Modified By : Sanjay Soni On 08/08/2015 to add OriginalImgPath
-- Modified By : Sanjay Soni On 21/08/2015 to change order BY sequence Desc
-- =============================================    
CREATE PROCEDURE [dbo].[GetGalleryImages_v15.8.1]
	-- Add the parameters for the stored procedure here    
	@ModelId INT = 0
	,@MainCategory INT = 0
	,@ApplicationId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from    
	-- interfering with SELECT statements.    
	SET NOCOUNT ON;

	IF @MainCategory = 0
	BEGIN
		SELECT *
		FROM (
			SELECT ROW_NUMBER() OVER (
					ORDER BY CategoryId DESC
						,IsMainImage DESC
						,Id
					) AS RowN
				,*
			FROM (
				SELECT CEI.Id
					,CB.CategoryId
					,CEI.IsMainImage
					,CEI.BasicId
					,CEI.ImageCategoryId
					,CEI.Sequence
					,CP.NAME AS CategoryName
					,Replace(CEI.Caption, '''', '&rsquo;') AS Caption
					,CEI.HostURL
					,ISNULL(CEI.ImageName,'' ) as ImageName
					,CEI.OriginalImgPath
					--,CEI.ImagePathThumbnail
					--,CEI.ImagePathLarge
					,CASE CP.MainCategory
						WHEN 1
							THEN 'Interior'
						WHEN 2
							THEN 'Exterior'
						END AS MainCategory
					,CASE CB.CategoryId
						WHEN 8
							THEN ('Road Test: ' + CMa.NAME + ' ' + CMo.NAME)
						WHEN 1
							THEN CB.Title
						WHEN 3
							THEN CB.Title
						WHEN 10
							THEN ''
						ELSE CB.Title
						END AS ArticleTitle
					,CB.Description
					,CASE CB.CategoryId
						WHEN 1
							THEN ('/news/' + CONVERT(VARCHAR, CB.Id) + '-' + CB.Url + '.html')
						WHEN 2
							THEN ('/research/comparos/' + CB.Url + '-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 3
							THEN ('/research/' + [dbo].[Parseurl](CMa.NAME) + '-cars/' + [dbo].[Parseurl](CMo.NAME) + '/buying-used-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 5
							THEN ('/research/tipsadvice/' + CB.Url + '-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 6
							THEN ('/research/features/' + CB.Url + '-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 8
							THEN ('/research/' + [dbo].[Parseurl](CMa.NAME) + '-cars/' + [dbo].[Parseurl](CMo.NAME) + '/roadtest-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 10
							THEN ''
						END AS ArticleUrl
					--,ISNULL(CEIS.ImageWidth, 160) AS ImageWidth
					--,ISNULL(CEIS.ImageHeight, 120) AS ImageHeight
					,ROW_NUMBER() OVER (
						PARTITION BY CEI.ID --CEIS.ImageId     
						ORDER BY CEI.LastUpdatedTime DESC
						) AS WidthRank
				FROM Con_EditCms_Basic CB WITH (NOLOCK)
				INNER JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
					AND CEI.IsActive = 1
				INNER JOIN CarModels CMo WITH (NOLOCK) ON CMo.ID = CEI.ModelId
				INNER JOIN CarMakes CMa WITH (NOLOCK) ON CMa.ID = CEI.MakeId
				INNER JOIN Con_PhotoCategory CP WITH (NOLOCK) ON CP.Id = CEI.ImageCategoryId
				--LEFT JOIN Con_EditCms_ImageSizes CEIS WITH (NOLOCK) ON CEIS.ImageId = CEI.Id
				WHERE CEI.ModelId = @ModelId
					AND CB.CategoryId IN (
						8
						,10
						)
					AND CB.IsPublished = 1
					AND CB.ApplicationID = @ApplicationId
				) AS tbl
			WHERE WidthRank = 1
			) AS TopRecords
		ORDER BY CASE 
				WHEN Sequence IS NULL
					THEN 1
				ELSE 0
				END
			,Sequence DESC
	END
	ELSE
	BEGIN
		SELECT *
		FROM (
			SELECT ROW_NUMBER() OVER (
					ORDER BY CategoryId DESC
						,IsMainImage DESC
						,Id
					) AS RowN
				,*
			FROM (
				SELECT CEI.Id
					,CB.CategoryId
					,CEI.IsMainImage
					,CEI.BasicId
					,CEI.ImageCategoryId
					,CEI.Sequence
					,CP.NAME AS CategoryName
					,Replace(CEI.Caption, '''', '&rsquo;') AS Caption
					,CEI.HostURL
					,ISNULL(CEI.ImageName,'') as ImageName
					,CEI.OriginalImgPath
					--CEI.ImagePathThumbnail,     
					--CEI.ImagePathLarge,     
					,CASE CP.MainCategory
						WHEN 1
							THEN 'Interior'
						WHEN 2
							THEN 'Exterior'
						END AS MainCategory
					,CASE CB.CategoryId
						WHEN 8
							THEN ('Road Test: ' + CMa.NAME + ' ' + CMo.NAME)
						WHEN 1
							THEN CB.Title
						WHEN 3
							THEN CB.Title
						WHEN 10
							THEN ''
						ELSE CB.Title
						END AS ArticleTitle
					,CB.Description
					,CASE CB.CategoryId
						WHEN 1
							THEN ('/news/' + CONVERT(VARCHAR, CB.Id) + '-' + CB.Url + '.html')
						WHEN 2
							THEN ('/research/comparos/' + CB.Url + '-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 3
							THEN ('/research/' + [dbo].[Parseurl](CMa.NAME) + '-cars/' + [dbo].[Parseurl](CMo.NAME) + '/buying-used-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 5
							THEN ('/research/tipsadvice/' + CB.Url + '-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 6
							THEN ('/research/features/' + CB.Url + '-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 8
							THEN ('/research/' + [dbo].[Parseurl](CMa.NAME) + '-cars/' + [dbo].[Parseurl](CMo.NAME) + '/roadtest-' + CONVERT(VARCHAR, CB.Id) + '/')
						WHEN 10
							THEN ''
						END AS ArticleUrl
					--,ISNULL(CEIS.ImageWidth, 160) AS ImageWidth
					--,ISNULL(CEIS.ImageHeight, 120) AS ImageHeight
					,ROW_NUMBER() OVER (
						PARTITION BY CEI.ID --CEIS.ImageId     
						ORDER BY CEI.LastUpdatedTime DESC
						) AS WidthRank
				FROM Con_EditCms_Basic CB WITH (NOLOCK)
				INNER JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id
					AND CEI.IsActive = 1
				INNER JOIN CarModels CMo WITH (NOLOCK) ON CMo.ID = CEI.ModelId
				INNER JOIN CarMakes CMa WITH (NOLOCK) ON CMa.ID = CEI.MakeId
				INNER JOIN Con_PhotoCategory CP WITH (NOLOCK) ON CP.Id = CEI.ImageCategoryId
				--LEFT JOIN Con_EditCms_ImageSizes CEIS ON CEIS.ImageId = CEI.Id
				WHERE CEI.ModelId = @ModelId
					AND CB.CategoryId IN (
						8
						,10
						)
					AND CB.IsPublished = 1
					AND CP.MainCategory = @MainCategory
					AND CB.ApplicationID = @ApplicationId
				) AS tbl
			WHERE WidthRank = 1
			) AS TopRecords
		ORDER BY CASE 
				WHEN Sequence IS NULL
					THEN 1
				ELSE 0
				END
			,Sequence DESC
	END
END
