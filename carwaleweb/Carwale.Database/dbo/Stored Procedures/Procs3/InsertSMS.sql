IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertSMS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertSMS]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Class TABLE

CREATE PROCEDURE [dbo].[InsertSMS]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@MobileNo		VARCHAR(50),
	@Keyword		VARCHAR(50),	
	@SMSFor		VARCHAR(100),
	@SMSText		VARCHAR(500),	
	@SMSDateTime	VARCHAR(50),
	@Status		VARCHAR(50),
	@SubmitDateTime	DATETIME
	
 AS
	
BEGIN
	IF @Id = -1 -- Insertion

		BEGIN
	
			INSERT INTO SMSModule ( MobileNo, Keyword, SMSFor, SMSText, SMSDateTime, Status, SubmitDateTime )
			 VALUES(  @MobileNo, @Keyword, @SMSFor, @SMSText, @SMSDateTime, @Status, @SubmitDateTime )
			
                       	END
	
					
	
END
