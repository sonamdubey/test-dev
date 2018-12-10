IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRelatedModelVideos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetRelatedModelVideos]
GO

	
-- =============================================    
-- Author:  Ravi Koshal
-- Create date: 7/7/2014
-- Description: To get videos based on tags and popularity.
-- Approved by Manish on 10-07-2014 05:30 checked index.
-- Modified by Natesh on 22-7-2014 Added check on divide by zero error. and search based on multiple tags
-- Modified by Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by Natesh on 22-8-2014 added rowno for distinct basic id videos
-- =============================================    
CREATE PROCEDURE [dbo].[GetRelatedModelVideos]  -- execute dbo.GetRelatedModelVideos 'Gl,Mercedes-Benz,Mercedes-Benz GL',1
	-- Add the parameters for the stored procedure here
	@Tags VARCHAR(MAX) 
	,@ApplicationId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @RowCount INT;

	with cte as (
			SELECT   ((cv.VIEWS) / CASE WHEN  DATEDIFF(DAY,PublishedDate,(GETDATE()-1))=0  THEN 1 
                                                        ELSE  DATEDIFF(DAY,PublishedDate,(GETDATE()-1))  END) AS Popularity
		,CB.Title AS VideoTitle
		,CB.Id AS BasicId
		,C.NAME AS MakeName
		,CM.NAME AS ModelName
		,CM.MaskingName
		,CBS.SubCategoryId AS SubCatId
		,CS.NAME AS SubCatName
		,CB.ApplicationID
		,CV.VideoUrl AS VideoSrc
		,ISNULL(CV.Likes, 0) AS Likes
		,ISNULL(CV.VIEWS, 0) AS VIEWS
		,CV.VideoId
		,left(CB.Description, 50) AS Description
		,CB.PublishedDate
		,ROW_NUMBER() OVER( partition by  cb.Id order by cb.Id )rowno -- rowno added to get distinct basic id 
	FROM Con_EditCms_Videos AS cv WITH (NOLOCK)
	INNER JOIN Con_EditCms_Basic AS cb WITH (NOLOCK) ON cb.Id = CV.BasicId
	INNER JOIN Con_EditCms_BasicTags AS ceb WITH (NOLOCK) ON ceb.BasicId = CB.Id
	INNER JOIN Con_EditCms_Tags AS cet WITH (NOLOCK) ON cet.Id = ceb.TagId
	INNER JOIN Con_EditCms_Cars CC WITH (NOLOCK) ON CC.BasicId = cv.BasicId
	INNER JOIN CarMakes C WITH (NOLOCK) ON CC.MakeId = C.Id
	INNER JOIN CarModels CM WITH (NOLOCK) ON CC.ModelId = Cm.ID
	INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
	INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
	WHERE cet.Tag IN (
			SELECT ListMember
			FROM fnSplitCSVValuesWithIdentity(@Tags)
			)
			
		AND CB.IsActive = 1
		AND CB.IsPublished = 1
		AND CV.IsActive = 1
		AND CB.ApplicationID = 1 
	) select TOP 2 * from cte 
	  where rowno = 1 
	  ORDER BY Popularity desc    

	SET @RowCount = @@ROWCOUNT

	IF (@RowCount < 2)
	BEGIN
		SELECT TOP 1 ((cv.VIEWS) / CASE WHEN  DATEDIFF(DAY,PublishedDate,(GETDATE()-1))=0  THEN 1 
                                                        ELSE  DATEDIFF(DAY,PublishedDate,(GETDATE()-1))  END) AS Popularity
			,CB.Title AS VideoTitle
			,CV.VideoUrl AS VideoSrc
			,ISNULL(CV.Likes, 0) AS Likes
			,ISNULL(CV.VIEWS, 0) AS VIEWS
			,CV.VideoId
			,left(CB.Description, 50) AS Description
			,CB.PublishedDate
			,CB.Id AS BasicId
			,C.NAME AS MakeName
			,CM.NAME AS ModelName
			,CM.MaskingName
			,CB.ApplicationID
			,CBS.SubCategoryId AS SubCatId
			,CS.NAME AS SubCatName
		FROM Con_EditCms_Basic AS CB WITH (NOLOCK)
		INNER JOIN Con_EditCms_Videos AS CV WITH (NOLOCK) ON CB.Id = CV.BasicId
			AND CV.IsActive = 1
		INNER JOIN Con_EditCms_Cars AS CC WITH (NOLOCK) ON CC.BasicId = CV.BasicId
			AND CC.IsActive = 1
		INNER JOIN CarMakes C WITH (NOLOCK) ON CC.MakeId = C.Id
		INNER JOIN CarModels CM WITH (NOLOCK) ON CC.ModelId = Cm.ID
		INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
		INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
		WHERE CB.IsPublished = 1
			AND CB.ApplicationID = @ApplicationId
			
	END
END



