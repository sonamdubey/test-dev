IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsites_AddImageTag]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsites_AddImageTag]
GO

	-- Created By: Nilesh Utture on 28th Oct, 2013 Adds Tag to uploaded image.
CREATE PROCEDURE [dbo].[Microsites_AddImageTag] 
	-- Add the parameters for the stored procedure here
	(
		@BranchId INT,
		@ModelId INT,
		@ImageId INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE 
			Microsite_Images
	SET		ModelId = @ModelId
	WHERE	Id = @ImageId AND
			DealerId = @BranchId
END

