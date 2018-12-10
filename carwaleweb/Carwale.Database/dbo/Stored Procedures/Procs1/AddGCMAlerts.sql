IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddGCMAlerts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddGCMAlerts]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <19/5/2014>
-- Description:	<Insert GCM Alerts>
-- =============================================
CREATE PROCEDURE [dbo].[AddGCMAlerts]
	@Title			VARCHAR(200),
	@AlertTypeId		INT,
	@DetailURL		VARCHAR(200),
	@IsFeatured		BIT,
	@SmallPicURL	VARCHAR(200), 
	@LargePicURL	VARCHAR(200),
	@CreatedBy		INT,
	@ID				INT	OUTPUT
AS
BEGIN
	SET @ID = -1

	INSERT INTO Mobile.GCMAlerts(Title,AlertTypeId,DetailURL,IsFeatured,SmallPicURL,LargePicURL,CreatedBy,CreatedOn) 
	VALUES(@Title,@AlertTypeId,@DetailURL,@IsFeatured,@SmallPicURL,@LargePicURL,@CreatedBy,GETDATE())
	
	SET @ID = SCOPE_IDENTITY()
END

