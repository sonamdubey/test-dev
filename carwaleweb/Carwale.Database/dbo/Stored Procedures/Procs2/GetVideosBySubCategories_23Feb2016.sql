IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetVideosBySubCategories_23Feb2016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetVideosBySubCategories_23Feb2016]
GO

	
-- =============================================
-- Author:	Sumit Kate
-- Create On: 18 Feb 2016
-- Description : To get videos for sub categories
-- EXEC [dbo].[GetVideosBySubCategories_23Feb2016] '48,49',1,2
-- =============================================
CREATE PROCEDURE [dbo].[GetVideosBySubCategories_23Feb2016] 
	-- Add the parameters for the stored procedure here
	@SubCategories VARCHAR(150)
	,@ApplicationID INT = 1
	,@startindex INT = NULL
	,@endindex INT = NULL
	,@MakeId INT = NULL
	,@ModelId INT = NULL
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
	IF(@SubCategories IS NOT NULL)	
	BEGIN
		IF @ApplicationID = 1
		BEGIN
			SELECT *
						FROM (
							SELECT *
								,Row_number() OVER (
									ORDER BY displaydate DESC
									) RowNo
							FROM (
								SELECT DISTINCT CV.basicid
									,CV.videourl
									,CV.VideoId
									,CV.likes
									,CV.VIEWS
									,CB.description
									,CV.duration
									,CB.title
									,CB.Url
									,CK.Name AS Make
									,CM.Name AS Model
									,CM.MaskingName
									,dbo.Getgoogletags(CB.id) AS Tag
									,CC.id AS VideoCarId
									,CS.NAME AS SubCatName
									,CB.displaydate
									,CI.HostURL AS HostUrl -- Modified by Ashwini 
									,CI.VideoPathThumbNail AS OriginalImagePath --Modified by Ashwini 
									,NULL AS ThumbnailDirPath
									,Row_number() OVER (
										PARTITION BY cv.basicid ORDER BY CC.id
										) AS row
									,CC.makeid AS MakeId
									,CC.ModelId AS ModelId
									,CB.isfeatured
								FROM con_editcms_category AS C WITH (NOLOCK)
								INNER JOIN con_editcms_subcategories AS SC WITH (NOLOCK) ON SC.categoryid = C.id
								INNER JOIN con_editcms_basicsubcategories AS BSC WITH (NOLOCK) ON BSC.subcategoryid = SC.id
								INNER JOIN con_editcms_videos AS CV WITH (NOLOCK) ON CV.basicid = BSC.basicid
								INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid								
								LEFT JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid AND CC.isactive = 1
								LEFT JOIN CarMakes CK WITH (NOLOCK) ON CC.makeid = CK.id
								LEFT JOIN CarModels CM WITH (NOLOCK) ON CC.modelid = CM.id
								INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
								INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
								LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (
										CI.BasicId = CV.BasicId
										AND CI.ImageCategoryId = @ThumbnailCategory
										)
								WHERE C.id = 13
									AND CV.isactive = 1									
									AND cS.CategoryId = 13
									AND CB.ispublished = 1
									AND CB.ApplicationID = @ApplicationID
									AND CB.isactive = 1
									AND (
										(
											CC.makeid = @MakeId
											OR @MakeId IS NULL
											)
										AND (
											CC.ModelId = @ModelId
											OR @ModelId IS NULL
											)
										)
									AND (
										CI.IsMainImage = 1
										OR CI.IsMainImage IS NULL
										)
									AND (
											BSC.subcategoryid IN 
											(
												SELECT Ids.ListMember FROM dbo.[fnSplitCSV](@SubCategories) AS Ids
											)
										)
								) a
							WHERE row = 1
							) AS T
						WHERE (
								(
									T.RowNo BETWEEN @startindex
										AND @endindex
									)
								OR @startindex IS NULL
								OR @endindex IS NULL
								)
						ORDER BY RowNo;

			SELECT MAX(RowNo) AS TotalRecords
						FROM (
							SELECT Row_number() OVER (
									ORDER BY displaydate DESC
									) RowNo
							FROM (
								SELECT displaydate,Row_number() OVER (
										PARTITION BY cv.basicid ORDER BY CC.id
										) AS row
								FROM con_editcms_category AS C WITH (NOLOCK)
								INNER JOIN con_editcms_subcategories AS SC WITH (NOLOCK) ON SC.categoryid = C.id
								INNER JOIN con_editcms_basicsubcategories AS BSC WITH (NOLOCK) ON BSC.subcategoryid = SC.id
								INNER JOIN con_editcms_videos AS CV WITH (NOLOCK) ON CV.basicid = BSC.basicid
								INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid
								LEFT JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid AND CC.isactive = 1-- Modified by sumit on 15-03-2016 added cc.isactive=1
								LEFT JOIN CarMakes CK WITH (NOLOCK) ON CC.makeid = CK.id
								LEFT JOIN CarModels CM WITH (NOLOCK) ON CC.modelid = CM.id
								INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
								INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
								LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (
										CI.BasicId = CV.BasicId
										AND CI.ImageCategoryId = @ThumbnailCategory
										)
								WHERE C.id = 13
									AND CV.isactive = 1									
									AND cS.CategoryId = 13
									AND CB.ispublished = 1
									AND CB.ApplicationID = @ApplicationID
									AND CB.isactive = 1
									AND (
										(
											CC.makeid = @MakeId
											OR @MakeId IS NULL
											)
										AND (
											CC.ModelId = @ModelId
											OR @ModelId IS NULL
											)
										)
									AND (
										CI.IsMainImage = 1
										OR CI.IsMainImage IS NULL
										)
									AND (
											BSC.subcategoryid IN 
											(
												SELECT Ids.ListMember FROM dbo.[fnSplitCSV](@SubCategories) AS Ids
											)
										)
								) a
							WHERE row = 1
							) AS T;				
		END
		ELSE IF @ApplicationID = 2
		BEGIN
			SELECT *
						FROM (
							SELECT *
								,Row_number() OVER (
									ORDER BY displaydate DESC
									) RowNo
							FROM (
								SELECT DISTINCT CV.basicid
									,CV.videourl
									,CV.VideoId
									,CV.likes
									,CV.VIEWS
									,CB.description
									,CV.duration
									,CB.title
									,CB.Url
									,CK.Name AS Make
									,CM.Name AS Model
									,CM.MaskingName
									,dbo.Getgoogletags(CB.id) AS Tag
									,CC.id AS VideoCarId
									,CS.NAME AS SubCatName
									,CB.displaydate
									,CI.HostURL AS HostUrl -- Modified by Ashwini 
									,CI.VideoPathThumbNail AS OriginalImagePath --Modified by Ashwini 
									,NULL AS ThumbnailDirPath
									,Row_number() OVER (
										PARTITION BY cv.basicid ORDER BY CC.id
										) AS row
									,CC.makeid AS MakeId
									,CC.ModelId AS ModelId
									,CB.isfeatured
								FROM con_editcms_category AS C WITH (NOLOCK)
								INNER JOIN con_editcms_subcategories AS SC WITH (NOLOCK) ON SC.categoryid = C.id
								INNER JOIN con_editcms_basicsubcategories AS BSC WITH (NOLOCK) ON BSC.subcategoryid = SC.id
								INNER JOIN con_editcms_videos AS CV WITH (NOLOCK) ON CV.basicid = BSC.basicid
								INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid								
								LEFT JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid AND CC.isactive = 1							
								LEFT JOIN BikeMakes CK WITH (NOLOCK) ON CC.makeid = CK.id
								LEFT JOIN BikeModels CM WITH (NOLOCK) ON CC.modelid = CM.id
								INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
								INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
								LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (
										CI.BasicId = CV.BasicId
										AND CI.ImageCategoryId = @ThumbnailCategory
										)
								WHERE C.id = 13
									AND CV.isactive = 1									
									AND cS.CategoryId = 13
									AND CB.ispublished = 1
									AND CB.ApplicationID = @ApplicationID
									AND CB.isactive = 1
									AND (
										(
											CC.makeid = @MakeId
											OR @MakeId IS NULL
											)
										AND (
											CC.ModelId = @ModelId
											OR @ModelId IS NULL
											)
										)
									AND (
										CI.IsMainImage = 1
										OR CI.IsMainImage IS NULL
										)
									AND (
											BSC.subcategoryid IN 
											(
												SELECT Ids.ListMember FROM dbo.[fnSplitCSV](@SubCategories) AS Ids
											)
										)
								) a
							WHERE row = 1
							) AS T
						WHERE (
								(
									T.RowNo BETWEEN @startindex
										AND @endindex
									)
								OR @startindex IS NULL
								OR @endindex IS NULL
								)
						ORDER BY RowNo;

			SELECT MAX(RowNo) AS TotalRecords
						FROM (
							SELECT Row_number() OVER (
									ORDER BY displaydate DESC
									) RowNo
							FROM (
								SELECT displaydate,Row_number() OVER (
										PARTITION BY cv.basicid ORDER BY CC.id
										) AS row
								FROM con_editcms_category AS C WITH (NOLOCK)
								INNER JOIN con_editcms_subcategories AS SC WITH (NOLOCK) ON SC.categoryid = C.id
								INNER JOIN con_editcms_basicsubcategories AS BSC WITH (NOLOCK) ON BSC.subcategoryid = SC.id
								INNER JOIN con_editcms_videos AS CV WITH (NOLOCK) ON CV.basicid = BSC.basicid
								INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid
								LEFT JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid AND CC.isactive = 1
								LEFT JOIN BikeMakes CK WITH (NOLOCK) ON CC.makeid = CK.id
								LEFT JOIN BikeModels CM WITH (NOLOCK) ON CC.modelid = CM.id
								INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
								INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
								LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (
										CI.BasicId = CV.BasicId
										AND CI.ImageCategoryId = @ThumbnailCategory
										)
								WHERE C.id = 13
									AND CV.isactive = 1									
									AND cS.CategoryId = 13
									AND CB.ispublished = 1
									AND CB.ApplicationID = @ApplicationID
									AND CB.isactive = 1
									AND (
										(
											CC.makeid = @MakeId
											OR @MakeId IS NULL
											)
										AND (
											CC.ModelId = @ModelId
											OR @ModelId IS NULL
											)
										)
									AND (
										CI.IsMainImage = 1
										OR CI.IsMainImage IS NULL
										)
									AND (
											BSC.subcategoryid IN 
											(
												SELECT Ids.ListMember FROM dbo.[fnSplitCSV](@SubCategories) AS Ids
											)
										)
								) a
							WHERE row = 1
							) AS T;				
		END
	END
END

