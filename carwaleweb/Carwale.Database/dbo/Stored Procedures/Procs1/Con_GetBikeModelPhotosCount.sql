IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_GetBikeModelPhotosCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_GetBikeModelPhotosCount]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 19-08-2016
-- Description:	photo count for bike photos
--8 roadtest, 2 comaprision tests,10 photo gallery
-- =============================================
CREATE PROCEDURE [dbo].[Con_GetBikeModelPhotosCount]
	@BasicId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;    

	DECLARE @ModelId INT
	SELECT @ModelId = ModelId FROM Con_EditCms_Cars WITH (NOLOCK) WHERE BasicId = @BasicId

	SELECT COUNT(i.Id) AS PhotoCount , @ModelId AS ModelId
	FROM Con_EditCms_Images i WITH (NOLOCK)
	INNER JOIN Con_EditCms_Basic b WITH (NOLOCK)
	 ON b.Id = i.BasicId AND b.IsPublished = 1 
	 AND b.ApplicationID = 2 AND b.IsActive = 1
	WHERE i.ModelId = @ModelId
		AND i.IsActive = 1    
		AND b.CategoryId IN (2,8,10)	
END

