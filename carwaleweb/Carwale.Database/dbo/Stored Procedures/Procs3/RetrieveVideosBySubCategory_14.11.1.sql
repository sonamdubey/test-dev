IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveVideosBySubCategory_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveVideosBySubCategory_14]
GO

	
-- =============================================
-- Author:		Rohan Sapkal
-- Create date: 31st July 2014
-- Description:	Retrieve List of videos belonging to specific Subcategory(eg mostpopular Videos)
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by: Manish on 02-09-2014 for sub category 1 query urgently since query is taking time. Need to check the data for isFeatured=0 condition.
-- Modified by: Rohan.s on 12-09-2014 for changing start and end index as optional parameters,start/end index implemented for SubCat=1
-- Modified by: Rohan.s on 30-10-2014 , Added new Category(JustLatest)
-- =============================================
CREATE PROCEDURE [dbo].[RetrieveVideosBySubCategory_14.11.1] --[dbo].[RetrieveVideosBySubCategory_14.11.1] 6,1
	-- Add the parameters for the stored procedure here
	@SubCat INT
	,@ApplicationID INT 
	,@startindex INT =NULL
	,@endindex INT = NULL
	,@MakeId INT =NULL
	,@ModelId INT =NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF @SubCat =6
BEGIN
	SELECT * FROM (SELECT Row_number() OVER ( ORDER BY basicRowNum DESC ) RowNo,*
 FROM (
						SELECT CV.basicid
							,Row_number() OVER (
								PARTITION BY CV.basicid ORDER BY CB.id
								) AS basicRowNum
							,CB.isfeatured
							,CV.videourl
							,CV.VideoId
							,CV.likes
							,CV.VIEWS
							,CB.description
							,CB.title
							,CB.Url
							,CV.duration
							,C.NAME AS Make
							,CM.NAME AS Model
							,CM.MaskingName
							,dbo.Getgoogletags(CB.id) AS Tag
							,CC.id AS VideoCarId
							,CB.displaydate
							,CS.NAME AS SubCatName
							,CC.makeid as MakeId 
							,CC.ModelId as ModelId
						FROM con_editcms_videos AS CV WITH (NOLOCK)
						INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid
						INNER JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid
						INNER JOIN carmakes C WITH (NOLOCK) ON CC.makeid = C.id
						INNER JOIN carmodels CM WITH (NOLOCK) ON CC.modelid = CM.id
						INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
						INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
						WHERE CV.isactive = 1
							AND CC.isactive = 1
							AND cS.CategoryId = 13
							AND CB.ispublished = 1
							AND CB.ApplicationID = @ApplicationID
							AND CB.isactive = 1
							AND (( CC.makeid = @MakeId OR @MakeId IS NULL ) AND ( CC.ModelId = @ModelId OR @ModelId IS NULL))
	) a
		WHERE basicRowNum = 1
		)T 
	WHERE ((T.RowNo BETWEEN @startindex AND @endindex) OR @startindex IS NULL OR @endindex IS NULL) 
	ORDER BY DisplayDate DESC;
END
ELSE
	IF @SubCat = 1
		--------------------------------------------latest and featured videos
		--DECLARE @basicIds varchar(500);
		--SET @basicIds=NULL;
	BEGIN
SELECT * FROM (SELECT Row_number() OVER ( ORDER BY basicRowNum DESC ) RowNo,*
 FROM (
						SELECT CV.basicid
							,Row_number() OVER (
								PARTITION BY CV.basicid ORDER BY CB.id
								) AS basicRowNum
							,CB.isfeatured
							,CV.videourl
							,CV.VideoId
							,CV.likes
							,CV.VIEWS
							,CB.description
							,CB.title
							,CB.Url
							,CV.duration
							,C.NAME AS Make
							,CM.NAME AS Model
							,CM.MaskingName
							,dbo.Getgoogletags(CB.id) AS Tag
							,CC.id AS VideoCarId
							,CB.displaydate
							,CS.NAME AS SubCatName
							,CC.makeid as MakeId 
							,CC.ModelId as ModelId
						FROM con_editcms_videos AS CV WITH (NOLOCK)
						INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid
						INNER JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid
						INNER JOIN carmakes C WITH (NOLOCK) ON CC.makeid = C.id
						INNER JOIN carmodels CM WITH (NOLOCK) ON CC.modelid = CM.id
						INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
						INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
						WHERE CV.isactive = 1
							AND CC.isactive = 1
							AND cS.CategoryId = 13
							AND CB.ispublished = 1
							AND CB.ApplicationID = @ApplicationID
							AND CB.isactive = 1
							AND (( CC.makeid = @MakeId OR @MakeId IS NULL ) AND ( CC.ModelId = @ModelId OR @ModelId IS NULL))
	) a
		WHERE basicRowNum = 1
		)T 
	WHERE ((T.RowNo BETWEEN @startindex AND @endindex) OR @startindex IS NULL OR @endindex IS NULL) 
	ORDER BY IsFeatured DESC,DisplayDate DESC;
	END
	ELSE
		IF @SubCat = 2
			--------------------------------------------most popular
			--DECLARE @basicIds varchar(500);
			--DECLARE @startindex INT;
			--DECLARE @endindex INT;
			--SET @basicIds=NULL;
			--SET @endindex=0;
			--SET @startindex=0;
		BEGIN
			SELECT *
			FROM (
				SELECT *
					,Row_number() OVER (
						ORDER BY VIEWS DESC
						) RowNo
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
							PARTITION BY cv.basicid ORDER BY CC.id
							) AS row
							,CC.makeid as MakeId 
							,CC.ModelId as ModelId
					FROM con_editcms_videos AS CV WITH (NOLOCK)
					INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid
					INNER JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid
					INNER JOIN carmakes C WITH (NOLOCK) ON CC.makeid = C.id
					INNER JOIN carmodels CM WITH (NOLOCK) ON CC.modelid = CM.id
					INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
					INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
					WHERE CV.isactive = 1
						AND CC.isactive = 1
						AND cS.CategoryId = 13
						AND CB.ispublished = 1
						AND CB.ApplicationID = @ApplicationID
						AND CB.isactive = 1
						AND (( CC.makeid = @MakeId OR @MakeId IS NULL ) AND ( CC.ModelId = @ModelId OR @ModelId IS NULL))
					ORDER BY CV.VIEWS DESC
					) a
				WHERE row = 1
				) AS T
			WHERE ((T.RowNo BETWEEN @startindex AND @endindex) OR @startindex IS NULL OR @endindex IS NULL)
			ORDER BY RowNo;
		END
				-------------------------------------------expert reviews(roadtest,comparison test,first drive),interio show and miscelleneous categories
		ELSE
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
						,Row_number() OVER (
							PARTITION BY cv.basicid ORDER BY CC.id
							) AS row
							,CC.makeid as MakeId 
							,CC.ModelId as ModelId
					FROM con_editcms_category AS C WITH (NOLOCK)
					INNER JOIN con_editcms_subcategories AS SC WITH (NOLOCK) ON SC.categoryid = C.id
					INNER JOIN con_editcms_basicsubcategories AS BSC WITH (NOLOCK) ON BSC.subcategoryid = SC.id
					INNER JOIN con_editcms_videos AS CV WITH (NOLOCK) ON CV.basicid = BSC.basicid
					INNER JOIN con_editcms_basic AS CB WITH (NOLOCK) ON CB.id = CV.basicid
					INNER JOIN con_editcms_cars AS CC WITH (NOLOCK) ON CC.basicid = CV.basicid
					INNER JOIN carmakes CK WITH (NOLOCK) ON CC.makeid = CK.id
					INNER JOIN carmodels CM WITH (NOLOCK) ON CC.modelid = CM.id
					INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
					INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
					WHERE C.id = 13
						AND CV.isactive = 1
						AND CC.isactive = 1
						AND cS.CategoryId = 13
						AND CB.ispublished = 1
						AND CB.ApplicationID = @ApplicationID
						AND CB.isactive = 1
						AND (( CC.makeid = @MakeId OR @MakeId IS NULL ) AND ( CC.ModelId = @ModelId OR @ModelId IS NULL))
						AND (
							(
								@SubCat = 3
								AND BSC.subcategoryid IN (
									47
									,48
									,50
									)
								)
							OR (
								@SubCat = 4
								AND BSC.subcategoryid IN (
									51
									,52
									,53
									)
								)
							OR (
								@SubCat = 5
								AND BSC.subcategoryid = 49
								)
							)
					) a
				WHERE row = 1
				) AS T
			WHERE ((T.RowNo BETWEEN @startindex AND @endindex) OR @startindex IS NULL OR @endindex IS NULL)
			ORDER BY RowNo;
		END
END


