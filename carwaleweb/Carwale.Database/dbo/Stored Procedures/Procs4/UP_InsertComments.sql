IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UP_InsertComments]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UP_InsertComments]
GO

	

CREATE PROCEDURE [dbo].[UP_InsertComments]
	@Id			BIGINT,
	@PhotoId		BIGINT, 
	@CustomerId		BIGINT,
	@Comments		VARCHAR(500), 
	@PostDateTime		DATETIME,
	@Status		INT OUTPUT		
AS

BEGIN

	SET @Status = 0

	IF @Id = -1

		BEGIN


			INSERT INTO UP_Comments( PhotoId, CustomerId, Comments, PostDateTime, IsApproved, ReportAbuse, IsActive ) 
			VALUES( @PhotoId, @CustomerId, @Comments, @PostDateTime, 0, 0, 1 )

			SET @Status = 1 
			
			
		END

	ELSE

		BEGIN
			
			UPDATE UP_Comments SET ReportAbuse = 1 WHERE ID= @Id
			
		END
END
