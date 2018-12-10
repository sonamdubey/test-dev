IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_CheckPhotosStatus_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_CheckPhotosStatus_v15]
GO

	-- Author		:	chetan dev
-- Create date	:	09/10/2012 14:00 PM
-- Description	:	This SP used to maintain record of uploaded images in the Trading Cars Software       
--					image will get upload in three sizes i.e. 640x428|300x225|80x60 ,this record initially will inactive       
-- =============================================    
CREATE PROCEDURE [dbo].[IMG_CheckPhotosStatus_v15.8.1]    
 -- Add the parameters for the stored procedure here    
 @PhotoId   BIGINT,
 @CategoryId INT
 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--IF @CategoryId = 1	--for trading cars
	--BEGIN
		--
	--END
	--ELSE 
	IF @CategoryId = 2	--for editcms
	BEGIN
		SELECT 
			CEI.Id, CEI.HostUrl AS HostUrl, CEI.OriginalImgPath AS OriginalImgPath 
		FROM Con_EditCms_Images AS CEI 
		WHERE CEI.Id = @PhotoId and CEI.StatusId > 2
			
	END
END



