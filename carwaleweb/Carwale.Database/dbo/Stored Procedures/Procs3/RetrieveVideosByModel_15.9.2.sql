IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveVideosByModel_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveVideosByModel_15]
GO

	
-- =============================================
-- Author:		Rohan Sapkal	
-- Create date: 31st July 2014
-- Description:	Retrieve Videos of specific car Model
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by Rohan on 12-9-2014 ,default value of index parameters set to NULL
-- Modified by Rohan 26-11-2014 new Join Con_ModelVideos for Thumbnails
-- Modified by Rohan.s on 18-12-2014 , Added columns to select thumbnail from CDN -on 24-12-2014 changed select n join statements for Con_editcms_images
-- Modified by Ashwini Todkar on 5 Aug 2015 retrieved original image path and host url and versioning
-- Modified by Ashwini Todkar on 8 Sep 2015 retrieved bikewale videos depends on application id and also added order basic id desc in retrival
-- =============================================
CREATE PROCEDURE [dbo].[RetrieveVideosByModel_15.9.2] --EXEC [dbo].[RetrieveVideosByModel_14.11.3] 126,1
	-- Add the parameters for the stored procedure here
	@ModelId INT = 0
	,@ApplicationID INT
	,@startindex INT = NULL
	,@endindex INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ThumbnailCategory NUMERIC = (
			SELECT Id
			FROM Con_PhotoCategory WITH (NOLOCK)
			WHERE NAME = 'VideoThumbNail'
				AND ApplicationId = @ApplicationID
			)

	-- Insert statements for procedure here
	IF @ApplicationID = 1
	BEGIN
		SELECT *
		FROM (
			SELECT DISTINCT TOP (1000) CV.basicid
				,CV.videourl
				,CV.VideoId
				,CV.likes
				,CV.VIEWS
				,CB.description
				,CV.duration
				,CB.title
				,CB.Url
				,C.NAME AS Make
				,CM.NAME AS Model
				,CM.MaskingName
				,dbo.Getgoogletags(CB.id) AS Tag
				,CC.id AS VideoCarId
				,CS.NAME AS SubCatName
				,Row_number() OVER (
					ORDER BY CV.basicid DESC
					) AS RowNo
				,CC.MakeId AS MakeId
				,CC.ModelId AS ModelId
				,CI.HostURL AS HostUrl --Modified by Ashwini Todkar
				,CI.VideoPathThumbNail AS OriginalImgPath
				,NULL AS ThumbnailDirPath
			FROM con_editcms_videos AS CV WITH (NOLOCK)
			INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid
			INNER JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid
			INNER JOIN carmakes C WITH (NOLOCK) ON CC.makeid = C.id
			INNER JOIN carmodels CM WITH (NOLOCK) ON CC.modelid = CM.id
			INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
			INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
			LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (
					CI.BasicId = CV.BasicId
					AND CI.ImageCategoryId = @ThumbnailCategory
					)
			WHERE CV.isactive = 1
				AND CC.isactive = 1
				AND cS.CategoryId = 13
				AND CB.ispublished = 1
				AND CB.isactive = 1
				AND CB.ApplicationID = @ApplicationID
				AND
				--( 
				--CC.makeid = @MakeId
				--       OR @MakeId IS NULL )
				--  AND
				--(
				CC.modelid = @ModelId
				--         OR @ModelId IS NULL )
				AND (
					CI.IsMainImage = 1
					OR CI.IsMainImage IS NULL
					)
			ORDER BY CV.BasicId DESC
			) AS T
		WHERE (
				(
					T.RowNo BETWEEN @startindex
						AND @endindex
					)
				OR @startindex IS NULL
				OR @endindex IS NULL
				)
	END
	ELSE
	BEGIN
		IF @ApplicationID = 2 --added by Ashwini Todkar on 8 Sep 2015 retrieved bikewale videos depends on application id and also added order basic id desc in retrival
		BEGIN
			SELECT *
			FROM (
				SELECT DISTINCT TOP (1000) CV.basicid
					,CV.videourl
					,CV.VideoId
					,CV.likes
					,CV.VIEWS
					,CB.description
					,CV.duration
					,CB.title
					,CB.Url
					,C.NAME AS Make
					,CM.NAME AS Model
					,CM.MaskingName
					,dbo.Getgoogletags(CB.id) AS Tag
					,CC.id AS VideoCarId
					,CS.NAME AS SubCatName
					,Row_number() OVER (
						ORDER BY CV.basicid DESC
						) AS RowNo
					,CC.MakeId AS MakeId
					,CC.ModelId AS ModelId
					,CI.HostURL AS HostUrl --Modified by Ashwini Todkar
					,CI.VideoPathThumbNail AS OriginalImgPath
					,NULL AS ThumbnailDirPath
				FROM con_editcms_videos AS CV WITH (NOLOCK)
				INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid
				INNER JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid
				INNER JOIN BikeMakes C WITH (NOLOCK) ON CC.makeid = C.id
				INNER JOIN BikeModels CM WITH (NOLOCK) ON CC.modelid = CM.id
				INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
				INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
				LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (
						CI.BasicId = CV.BasicId
						AND CI.ImageCategoryId = @ThumbnailCategory
						)
				WHERE CV.isactive = 1
					AND CC.isactive = 1
					AND cS.CategoryId = 13
					AND CB.ispublished = 1
					AND CB.isactive = 1
					AND CB.ApplicationID = @ApplicationID
					AND
					--( 
					--CC.makeid = @MakeId
					--       OR @MakeId IS NULL )
					--  AND
					--(
					CC.modelid = @ModelId
					--         OR @ModelId IS NULL )
					AND (
						CI.IsMainImage = 1
						OR CI.IsMainImage IS NULL
						)
				ORDER BY CV.BasicId DESC
				) AS T
			WHERE (
					(
						T.RowNo BETWEEN @startindex
							AND @endindex
						)
					OR @startindex IS NULL
					OR @endindex IS NULL
					)
		END
				----------------------------------
	END
			--ORDER BY basicid DESC
END



