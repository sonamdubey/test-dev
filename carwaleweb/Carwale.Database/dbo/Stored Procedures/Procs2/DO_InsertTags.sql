IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_InsertTags]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_InsertTags]
GO

	

CREATE PROCEDURE [dbo].[DO_InsertTags]
	@Id			BIGINT,
	@Tag			VARCHAR(50), 
	@Status		INT OUTPUT		
 AS
	
BEGIN
	SET @Status = 0

	IF @Id = -1
		BEGIN

			SELECT ID FROM DealerOffersTags WHERE Tag = @Tag
			
			IF @@RowCount = 0
				BEGIN
					INSERT INTO DealerOffersTags( Tag ) 
					VALUES( @Tag )
		
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
	ELSE
		BEGIN
			SELECT ID FROM DealerOffersTags WHERE Tag = @Tag AND ID <> @Id
			
			IF @@RowCount = 0

				BEGIN
					UPDATE  DealerOffersTags SET Tag = @Tag WHERE ID = @Id
				
					SET @Status = 1 
				END
			ELSE
				SET @Status = 0
		END
END
