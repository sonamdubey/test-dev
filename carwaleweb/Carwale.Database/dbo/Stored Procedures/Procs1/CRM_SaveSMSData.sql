IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveSMSData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveSMSData]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveSMSData]

	@LeadId			Numeric,
	@SMSType		Int,
	@EventType		Int,
	@SentBy			Numeric,
	@SMS			VarChar(500),
	@SMSDate		DateTime
				
 AS
	
BEGIN

	INSERT INTO CRM_SentSMS
	(
		LeadId, SMSType, EventType, SentBy, SMS, SMSDate
	)
	VALUES
	(
		@LeadId, @SMSType, @EventType, @SentBy, @SMS, @SMSDate
	)
		
END













