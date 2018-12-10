IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_SMSAddressSent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_SMSAddressSent]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE SMS Sent into NCD_Address on 11/8/2011 bu umesh

Create PROCEDURE [dbo].[NCD_SMSAddressSent]
	@MobileNo		Varchar(12),
	@SmsAddress		Varchar(140)
 AS
	
BEGIN
	Insert Into 
		NCD_SmsAddress 
			(
				MobileNo, 	SmsAddress
			)	
	Values
			(
				@MobileNo, 	@SmsAddress
			)	
		
END

