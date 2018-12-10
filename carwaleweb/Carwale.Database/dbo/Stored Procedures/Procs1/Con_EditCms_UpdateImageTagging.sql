IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_UpdateImageTagging]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_UpdateImageTagging]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 22 Aug 2013
-- Description:	Proc to update the make and model tagging for editcms photos
-- =============================================
CREATE PROCEDURE [dbo].[Con_EditCms_UpdateImageTagging]
	-- Add the parameters for the stored procedure here
	@ImageId VARCHAR(1000),
	@MakeId INT,
	@ModelId INT,
	@AltImageName VARCHAR(100),
	@Title	VARCHAR(100),
	@Description VARCHAR(200)  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
	UPDATE Con_EditCms_Images
	SET MakeId = @MakeId, ModelId = @ModelId ,AltImageName = @AltImageName , Title = @Title ,Description = @Description 
	WHERE Id in(select listmember from fnSplitCSV(@ImageId))
END

