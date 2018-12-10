IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveSantaWish]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveSantaWish]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 18 Dec 2012
-- Description:	Save the SKODA Secret Santa wish & User details
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveSantaWish]
	-- Add the parameters for the stored procedure here
	@Id			INT,
	@Name		VARCHAR(50),
	@Contact	VARCHAR(15),
	@Email		VARCHAR(50),
	@Wish		VARCHAR(200),
	@FacebookId	VARCHAR(20),
	@ClientIp	VARCHAR(20),
	@NewId		INT OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET	@NewId = -1

    -- Insert statements for procedure here
    IF @Id = -1
		BEGIN
			INSERT INTO OLM_SantaWish(Name, Contact, Email, Wish, FacebookId, ClientIp)
			VALUES (@Name, @Contact, @Email, @Wish, @FacebookId, @ClientIp)
			
			SET @NewId = SCOPE_IDENTITY()
		END
	ELSE
		BEGIN
			UPDATE OLM_SantaWish
				SET Name = @Name, Contact = @Contact, Email = @Email
			WHERE Id = @Id
			
			SET @NewId = @Id
		END
END

