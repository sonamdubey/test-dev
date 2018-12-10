IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AutoExpo2016_FetchImagesByCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AutoExpo2016_FetchImagesByCategory]
GO

	
-- =============================================
-- Author:		MihirA Chheda
-- Create date: 28-12-2015
-- Description:	return all the images based on category id 
--              @CategoryId=To fetch photos
-- =============================================
CREATE PROCEDURE [dbo].[AutoExpo2016_FetchImagesByCategory] 
	@ApplicationId		INT = NULL
AS
BEGIN

	SELECT		CEB.Id AS BasicId,CEB.Title ArticleTitle,CI.IsMainImage,CI.HostUrl,CI.OriginalImgPath,RANK()  over(partition by CEB.Id order by CI.IsMainImage)Rank,CEB.ApplicationId
	FROM		Con_EditCms_Images CI(NOLOCK)
	INNER JOIN  Con_EditCms_Basic CEB(NOLOCK) ON CEB.Id =CI.BasicId 
	INNER JOIN  Con_PhotoCategory CP WITH (NOLOCK) ON CP.Id = CI.ImageCategoryId
	INNER JOIN  Con_EditCms_Category CEC (NOLOCK) ON CEC.Id=CEB.CategoryId
	WHERE       CI.IsActive = 1 AND CEC.Id IN (19,20) AND CEB.ApplicationID=@ApplicationId  --AND ci.BasicId=12327  
	ORDER BY    RANK DESC

END

