IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_HoliUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_HoliUser]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 25-Mar-2013
-- Description:	Save the user details(name & email) and return newly generated Id for SKODA Facebook Holi App
-- =============================================
CREATE PROCEDURE [dbo].[OLM_HoliUser]
	-- Add the parameters for the stored procedure here
	@UpId		INT = -1,
	@Name		VARCHAR(100) = NULL,
	@Email		VARCHAR(100) = NULL,
	@ImgHostUrl	VARCHAR(50) = NULL,
	@Id			NUMERIC(18,0) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET @Id = -1
	
	IF @UpId = -1
		BEGIN
			INSERT INTO OLM_HoliData (Name, Email)
			VALUES (@Name, @Email)
			
			SET @Id = SCOPE_IDENTITY()
		END
	ELSE
		BEGIN
			UPDATE OLM_HoliData SET ImgHostUrl = @ImgHostUrl WHERE Id = @UpId
			SET @Id = @UpId
		END
END
