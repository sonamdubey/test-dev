IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertSMSSent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertSMSSent]
GO

	

--THIS PROCEDURE INSERTS THE VALUES FOR THE SMS Sent

CREATE PROCEDURE [dbo].[InsertSMSSent]
	@Number		VARCHAR(50) = NULL,
	@Message		VARCHAR(500) = NULL,
	@ServiceType		INT = NULL,
	@SMSSentDateTime	DATETIME = NULL,
	@Successfull		BIT = NULL,
	@ReturnedMsg		VARCHAR(500) = NULL,
	@SMSPageUrl			VARCHAR(500) = NULL
 AS
	
BEGIN

	
		INSERT INTO 
			SMSSent 
				(
					Number, 	Message, 	ServiceType, 	SMSSentDateTime,	Successfull, 	
					ReturnedMsg, 	SMSPageUrl
				)	
		VALUES
				(
					@Number, 	@Message, 	@ServiceType, 	@SMSSentDateTime, 	@Successfull,	
					@ReturnedMsg, @SMSPageUrl
				)	
		SELECT SCOPE_IDENTITY()
		
END



