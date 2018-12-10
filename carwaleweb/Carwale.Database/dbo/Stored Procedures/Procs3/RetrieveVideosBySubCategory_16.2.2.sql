IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveVideosBySubCategory_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveVideosBySubCategory_16]
GO

	
-- =============================================
-- Author:		Rohan Sapkal
-- Create date: 31st July 2014
-- Description:	Retrieve List of videos belonging to specific Subcategory(eg mostpopular Videos)
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by: Manish on 02-09-2014 for sub category 1 query urgently since query is taking time. Need to check the data for isFeatured=0 condition.
-- Modified by: Rohan.s on 12-09-2014 for changing start and end index as optional parameters,start/end index implemented for SubCat=1
-- Modified by: Rohan.s on 30-10-2014 , Added new Category(JustLatest)
-- Modified by Rohan.s on 18-12-2014 , Added columns to select thumbnail from CDN --on 24-12-2014 changed CI.ImageCustomPath to CI.VideoPathThumbNail in select 
-- Modified by Ashwini Todkar on 5 Aug 2015 retrieved original image path and host url and versioning
-- Added by Ashwini Todkar  08-09-2015 so that same SP should work for BikeWale
-- Modified By Rakesh Yadav on 14 Dec 2015,Inner Join is replaced with Left Join with con_editcms_cars,BikeMakes,BikeModels to show untagged videos
-- Added by Sumit Kate on 18 Feb 2016 returns the videos for sub categories
-- =============================================
CREATE  PROCEDURE [dbo].[RetrieveVideosBySubCategory_16.2.2] --[dbo].[RetrieveVideosBySubCategory_16.2.2] '48,49',1,2
	-- Add the parameters for the stored procedure here
	@SubCategories VARCHAR(150)
	,@SubCat INT = 1
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
		IF @ApplicationID = 2
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
									,CK.NAME AS Make
									,CM.NAME AS Model
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
								LEFT JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid
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
									AND CC.isactive = 1
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
								LEFT JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid
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
									AND CC.isactive = 1
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

