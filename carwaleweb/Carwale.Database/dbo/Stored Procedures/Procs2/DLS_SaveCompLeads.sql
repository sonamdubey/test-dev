IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DLS_SaveCompLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DLS_SaveCompLeads]
GO

	


CREATE PROCEDURE [dbo].[DLS_SaveCompLeads]
	@Id				NUMERIC,
	@QuoteId		NUMERIC = NULL,
	@Mobile			VARCHAR(15) = NULL,
	@DealerId		NUMERIC = NULL,
	@CustomerId		NUMERIC = NULL,
	@VersionId		NUMERIC = NULL,
	@UpdateDate		DATETIME = NULL,
	@UpdatedBy		NUMERIC = NULL,
	@IsVerified		BIT		= NULL,
	@IsValid		BIT OUTPUT
 AS
	
BEGIN
	SET @IsValid = 0
	
	IF @Id = -1
		BEGIN
			UPDATE Customers SET Mobile = @Mobile WHERE Id = @CustomerId
			IF @@ROWCOUNT > 0
				BEGIN
					INSERT INTO DLS_CompLeads
					(
						QuoteId, DealerId, CustomerId, VersionId
					) 
					VALUES
					( 
						@QuoteId, @DealerId, @CustomerId, @VersionId
					)
						
					SET @IsValid = 1
				END
		END
	ELSE
		BEGIN
			UPDATE DLS_CompLeads
			SET UpdateDate = @UpdateDate, UpdatedBy = @UpdatedBy,
				IsVerified = @IsVerified
			WHERE Id = @Id
			
			SET @IsValid = 1 
		END
END



