IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_DeleteCarPhoto]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_DeleteCarPhoto]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 16th Dec 2014
-- Description:	To Delete AbSure CarPhoto
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_DeleteCarPhoto] 
	@Id			INT,
	@ModifiedBy	INT,
	@Status		BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status = 0
    UPDATE	AbSure_CarPhotos 
	SET		IsActive = 0,
			ModifiedBy = @ModifiedBy, 
			ModifiedDate = GETDATE() 
	OUTPUT inserted.AbSure_CarPhotosId
	WHERE	AbSure_CarPhotosId = @Id

	IF @@ROWCOUNT = 1
		SET @Status = 1
END
