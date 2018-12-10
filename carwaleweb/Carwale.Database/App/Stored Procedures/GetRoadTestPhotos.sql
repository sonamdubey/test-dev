IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetRoadTestPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetRoadTestPhotos]
GO

	
-- ============================================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching photos for particular roadtest
-- ============================================================

CREATE PROCEDURE [App].[GetRoadTestPhotos]
	@BasicId Integer
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT 
		ROW_NUMBER() OVER (ORDER BY Sequence ASC) AS Row, * FROM Con_EditCms_Images CI 
		Inner Join Con_PhotoCategory CP On CP.Id = CI.ImageCategoryId 
        WHERE BasicId = @BasicId AND IsActive = 1 AND CI.IsMainImage=0
	
END

