IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AutoExpo2016_FetchImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AutoExpo2016_FetchImages]
GO

	-- =============================================
-- Author:		Mihir Chheda
-- Create date: 16-12-2015 
-- Description:	return photos based on category and application type
-- =============================================
CREATE PROCEDURE [dbo].[AutoExpo2016_FetchImages] ---1,3,-1
	@ApplicationId		INT = NULL,
	@MakeId				INT = NULL,
	@CategoryId         INT = NULL,
	@PlatformType		INT = NULL
AS
BEGIN
   SELECT    CAST(SUBSTRING(AI.ImageUrl,0,26) AS VARCHAR(50)) HostUrl, CAST(SUBSTRING(AI.ImageUrl,29,100) AS VARCHAR(100)) OriginalImgPath	
             ,AI.Id,AI.Title,AI.ImageUrl,AI.ImageLink,AI.Description,AI.MakeId,AI.ImageCategoryId,AI.Priority,AI.ThumbImageUrl,AI.isVideo,CM.Name MakeName,AI.CssClass,AI.AndroidLink,AI.IsVideo
   FROM		AutoExpo_ImageDetails AI(NOLOCK)
   INNER JOIN CarMakes CM (NOLOCK) ON Cm.ID=AI.MakeId
   WHERE	(ISNULL(@ApplicationId,-1) = -1 OR ApplicationId = @ApplicationId) 
            AND (ISNULL(@MakeId,-1) = -1 OR MakeId = @MakeId)
			AND (ISNULL(@CategoryId,-1) = -1 OR ImageCategoryId = @CategoryId) 
            AND IsActive=1 AND PlatformType=@PlatformType
   ORDER BY Priority
END


