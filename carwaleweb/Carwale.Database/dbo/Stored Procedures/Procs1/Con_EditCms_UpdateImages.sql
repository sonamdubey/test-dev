IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_UpdateImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_UpdateImages]
GO

	-- =============================================
-- Author:		Sanjay Soni
-- Create date: 25 Aug 2015
-- Description:	Proc to update image url bsed on imageId for editcms photos
-- =============================================
CREATE PROCEDURE [dbo].[Con_EditCms_UpdateImages]
	-- Add the parameters for the stored procedure here
	@Id Int,
	@LastUpdatedBy INT,
	@HostUrl VARCHAR(50),
	@OriginalImgPath VARCHAR(250)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    UPDATE Con_EditCms_Images SET HostUrl = @HostUrl, OriginalImgPath = @OriginalImgPath, LastUpdatedTime = GETDATE(), LastUpdatedBy = @LastUpdatedBy WHERE ID = @Id

END

