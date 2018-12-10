IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_CarPhotos_Desc]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_CarPhotos_Desc]
GO

	
-- =============================================
-- Author:		Satish Sharma
-- Create date: DEC 10, 2008
-- Description:	SP to remove selected image, if selected image was main image 
--				then mark next available image as a main image
-- =============================================
CREATE PROCEDURE [dbo].[Classified_CarPhotos_Desc]
	-- Add the parameters for the stored procedure here	
	@PhotoId		NUMERIC,
	@ImgDesc			VARCHAR(200)
AS
BEGIN		
	UPDATE CarPhotos SET Description = @ImgDesc WHERE Id = @PhotoId
END


