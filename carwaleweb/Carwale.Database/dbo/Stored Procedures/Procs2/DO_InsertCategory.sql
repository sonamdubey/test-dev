IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_InsertCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_InsertCategory]
GO

	

CREATE PROCEDURE [dbo].[DO_InsertCategory]
	@Id			BIGINT,
	@Category		VARCHAR(50), 
	@Status		INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id = -1
		BEGIN

			SELECT ID FROM DealerOffersSourceCategory WHERE Category = @Category
			
			IF @@RowCount = 0
				BEGIN
					INSERT INTO DealerOffersSourceCategory( Category ) 
					VALUES( @Category )
		
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
	ELSE
		BEGIN
			SELECT ID FROM DealerOffersSourceCategory WHERE Category = @Category AND ID <> @Id
			
			IF @@RowCount = 0

				BEGIN
					UPDATE  DealerOffersSourceCategory SET Category = @Category WHERE ID = @Id
				
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
END
