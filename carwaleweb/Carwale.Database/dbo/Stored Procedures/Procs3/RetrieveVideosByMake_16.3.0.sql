IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveVideosByMake_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveVideosByMake_16]
GO

	-- =============================================
-- Author:		Rohan Sapkal	
-- Create date: 31st July 2014
-- Description:	Retrieve Videos of Specific car Make
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by: Rohan S on 16-09-2014 , Handled start/end index as NULL default
-- Modified by Rohan.s on 18-12-2014 , Added columns to select thumbnail from CDN -on 24-12-2014 changed select n join statement for Con_editcms_images
-- Modified by Ashwini Todkar on 5 Aug 2015 retrieved original image path and host url and versioning
-- Modified by Ashwini Todkar on 8 Sep 2015 retrieved bikewale videos depends on application id
-- Modified by Rakesh Yadav on 1 March 2016, retrive DisplayDate
-- Modified by Ajay Singh on 27-june-2016,order by display date as the replacement of order by basic id
-- =============================================
CREATE PROCEDURE [dbo].[RetrieveVideosByMake_16.3.0]
	-- Add the parameters for the stored procedure here
	@MakeId INT
	,@ApplicationID INT
	,@StartIndex INT = NULL
	,@EndIndex INT = NULL
AS
BEGIN
	DECLARE @ThumbnailCategory NUMERIC = (
			SELECT Id
			FROM Con_PhotoCategory WITH (NOLOCK)
			WHERE NAME = 'VideoThumbNail'
				AND ApplicationId = @ApplicationID
			)

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
				,CI.HostURL AS HostUrl --Modified by Ashwini
				,CI.VideoPathThumbNail AS OriginalImgPath --Modified by Ashwini
				,NULL AS ThumbnailDirPath
				,CB.DisplayDate
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
				AND CB.ApplicationID = @ApplicationID
				AND CB.isactive = 1
				AND
				--( 
				CC.makeid = @MakeId
				--       OR @MakeId IS NULL )
				--  AND ( CC.modelid = @ModelId
				--         OR @ModelId IS NULL )
				AND (
					CI.IsMainImage = 1
					OR CI.IsMainImage IS NULL
					)
			ORDER BY CB.DisplayDate DESC --Changed by Ajay singh on 27 -06-2016
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
		IF @ApplicationID = 2
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
					,CI.HostURL AS HostUrl --Modified by Ashwini
					,CI.VideoPathThumbNail AS OriginalImgPath --Modified by Ashwini
					,NULL AS ThumbnailDirPath
					,CB.DisplayDate
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
					AND CB.ApplicationID = @ApplicationID
					AND CB.isactive = 1
					AND
					--( 
					CC.makeid = @MakeId
					--       OR @MakeId IS NULL )
					--  AND ( CC.modelid = @ModelId
					--         OR @ModelId IS NULL )
					AND (
						CI.IsMainImage = 1
						OR CI.IsMainImage IS NULL
						)
				ORDER BY CB.DisplayDate DESC--Changed by Ajay singh on 27 -06-2016
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
	END
END

