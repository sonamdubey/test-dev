IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LogSendLeadsSMSData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LogSendLeadsSMSData]
GO

	-- Author	:	Sachin Bharti(1st oct 2013)
-- Purpose	:	To log the record of SMS sended to the specail users
CREATE PROCEDURE [dbo].[TC_LogSendLeadsSMSData]( @TC_LeadsSMSDataLog TC_LeadsSMSDataLog READONLY )

AS
BEGIN
	INSERT INTO TC_LeadsSMSLog
					(MobileNo,
					 Message,
					 SendedOn
					 )
			SELECT	SM.Mobile,
					SM.Message,
					SendedOn
		   FROM @TC_LeadsSMSDataLog SM 
 END



