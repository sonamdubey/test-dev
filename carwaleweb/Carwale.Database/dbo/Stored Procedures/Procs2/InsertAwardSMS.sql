IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertAwardSMS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertAwardSMS]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Class TABLE

CREATE PROCEDURE [dbo].[InsertAwardSMS]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@MobileNo		VARCHAR(50),
	@Keyword		VARCHAR(50),	
	@SMSText		VARCHAR(500),	
	@SMSDateTime	VARCHAR(50),
	@SubmitDateTime	DATETIME
	
 AS
	
BEGIN
	IF @Id = -1 -- Insertion

		BEGIN
	
			INSERT INTO AwardSMSModule ( MobileNo, Keyword, SMSText, SMSDateTime,  SubmitDateTime )
			 VALUES(  @MobileNo, @Keyword, @SMSText, @SMSDateTime,  @SubmitDateTime )
			
                       	END
	
					
	
END
