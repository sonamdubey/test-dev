IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertConsumerCreditPoints]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertConsumerCreditPoints]
GO
	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Class Packages TABLE Packages

CREATE PROCEDURE [dbo].[InsertConsumerCreditPoints]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@ConsumerType	SMALLINT,	
	@ConsumerId		NUMERIC,	--validaty in days
	@ExpiryDate		DATETIME,	
	@Points		NUMERIC,
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	
BEGIN
	SET @Status = 0
	
	IF @Id = -1 

		BEGIN
		
			INSERT INTO ConsumerCreditPoints (ConsumerType, ConsumerId, ExpiryDate, Points)
		
			VALUES (@ConsumerType, @ConsumerId, @ExpiryDate, @Points)
		
			SET @Status = 1
		END
	ELSE

		BEGIN
			UPDATE ConsumerCreditPoints SET ExpiryDate=@ExpiryDate, Points=@Points
			WHERE ConsumerType = @ConsumerType AND ConsumerId=@ConsumerId
			
			SET @Status=2
		END
	
END
