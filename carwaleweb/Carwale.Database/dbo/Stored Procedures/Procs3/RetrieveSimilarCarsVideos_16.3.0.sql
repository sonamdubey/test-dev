IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveSimilarCarsVideos_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveSimilarCarsVideos_16]
GO

	
-- ============================================= 
-- Author:Prashant Vishe     
-- Create date:12 Dec 2013
-- Description:Stored Procedure for Retrieving Related Videos. 
-- modified On:9 Jan 2013 by Prashant vishe
-- modification:added query for retrieving subcatName
-- Modifed On:3 march 2013 by Prashant
-- Modification:added VideoId in query
-- Modified On:13 Aug 2014 by Rohan Sapkal
-- Modification:changed SELECT @SimilarModels to handle multiple models for one video
-- Modified by Manish on 13-08-2014 changed @SimilarModels datatype length from 500 to 250
-- Modified by Rohan.s On 12-09-2014 Removed TopCount,16-9-2014 commented out redundant rownum()
-- Modified by Rohan.s on 18-12-2014 , Added columns to select thumbnail from CDN -ON 24-12-2014 ,changed join n select for Con_editcms_images
-- Modified by Ashwini Todkar on 5 Aug 2015 retrieved original image path and host url and versioning
-- Modified by Rakesh Yadav on 1 March 2016, retrive DisplayDate
-- ============================================= 
CREATE PROCEDURE [dbo].[RetrieveSimilarCarsVideos_16.3.0] --[dbo].[RetrieveSimilarCars] 13827,1
	-- Add the parameters for the stored procedure here 
	@BasicId NUMERIC
	,@ApplicationID INT = 1
AS
BEGIN
	IF @ApplicationID = 1
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
		DECLARE @SimilarModels VARCHAR(250)

		SELECT @SimilarModels = COALESCE(@SimilarModels + ',', '') + SCM.similarmodels
		FROM similarcarmodels SCM WITH (NOLOCK)
		INNER JOIN con_editcms_cars CC WITH (NOLOCK) ON CC.modelid = SCM.modelid
		WHERE CC.basicid = @BasicId
			AND SCM.isactive = 1

		SELECT *
		--  ,Row_number()
		--    OVER(
		--      partition BY basicid
		--      ORDER BY videocarid) AS row
		FROM (
			SELECT DISTINCT CB.id AS BasicId
				,CC.modelid
				,CV.videourl
				,CV.VideoId
				,CV.VIEWS
				,CV.likes
				,CV.isactive
				,CB.description
				,CB.title
				,CB.Url
				,CV.duration
				,C.NAME AS Make
				,CM.NAME AS Model
				,CM.MaskingName
				,dbo.Getgoogletags(CB.id) AS Tag
				,CS.NAME AS SubCatName
				,CC.id AS VideoCarId
				,CI.HostURL AS HostUrl -- Modified by Ashwini 
				,CI.VideoPathThumbNail AS OriginalImagePath -- Modified by Ashwini 
				,CB.DisplayDate
				,NULL AS ThumbnailDirPath
				,Row_number() OVER (
					PARTITION BY cv.basicid ORDER BY CC.id
					) AS row
			FROM con_editcms_videos CV WITH (NOLOCK)
			INNER JOIN con_editcms_cars CC WITH (NOLOCK) ON CC.basicid = CV.basicid
			INNER JOIN con_editcms_basic CB WITH (NOLOCK) ON CB.id = CV.basicid
			LEFT JOIN con_editcms_basictags CBT WITH (NOLOCK) ON CBT.basicid = CV.basicid
			LEFT JOIN con_editcms_tags CT WITH (NOLOCK) ON CT.id = CBT.tagid
			INNER JOIN carmakes C WITH (NOLOCK) ON CC.makeid = C.id
			INNER JOIN carmodels CM WITH (NOLOCK) ON CC.modelid = Cm.id
			INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
			INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
			LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (
					CI.BasicId = CV.BasicId
					AND CI.ImageCategoryId = @ThumbnailCategory
					)
			WHERE CC.modelid IN (
					SELECT data
					FROM [dbo].[Stringsplit](@SimilarModels, ',')
					)
				AND CV.isactive = 1
				AND cS.CategoryId = 13
				AND CC.isactive = 1
				AND CB.ispublished = 1
				AND CB.ApplicationID = @ApplicationID
				AND CB.isactive = 1
				AND (
					CI.IsMainImage = 1
					OR CI.IsMainImage IS NULL
					)
			) a
		WHERE row = 1
	END
END