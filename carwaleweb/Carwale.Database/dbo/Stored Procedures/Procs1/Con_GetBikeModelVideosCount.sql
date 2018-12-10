IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_GetBikeModelVideosCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_GetBikeModelVideosCount]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 19-08-2016
-- Description:	bike video count
-- =============================================
CREATE PROCEDURE [dbo].[Con_GetBikeModelVideosCount] 
@BasicId INT
AS
BEGIN
	DECLARE @ModelId INT;
	SELECT @ModelId = ModelId FROM Con_EditCms_Cars WITH (NOLOCK) WHERE BasicId = @BasicId
	
	SELECT COUNT(1) AS VideoCount, @ModelId AS ModelId
	FROM(
		SELECT distinct c.ModelId AS ModelId, c.BasicId
		FROM Con_EditCms_Videos v WITH (NOLOCK)
		INNER JOIN Con_EditCms_Basic AS b WITH (NOLOCK) ON b.Id = v.BasicId AND b.IsPublished = 1 AND b.ApplicationID = 2 AND b.IsActive = 1 AND b.CategoryId = 13 -- for videos categogry only
		INNER JOIN Con_EditCms_Cars AS c WITH(NOLOCK) ON c.BasicId = b.Id AND c.IsActive = 1 AND c.ModelId = @ModelId
		WHERE v.IsActive = 1
	) AS a 
END
