IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SendSMSToSpecialUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SendSMSToSpecialUsers]
GO

	-- =============================================
-- Author	:	Sachin Bharti		
-- Create date	:	<30th Sep 2013>
-- Description	:	First insert SMS data in the table and then fetch it form 
--					table to send SMS
-- =============================================
CREATE PROCEDURE [dbo].[TC_SendSMSToSpecialUsers]

AS
BEGIN
		-- SP called to insert all special users SMS related data in the TC_SMSDetail table
		EXECUTE [dbo].[TC_InsertSMSDetail] 

		-- SP called to fetch all the details from TC_SMSDetail table and return table
		EXECUTE  [dbo].[TC_SMSDetailsFetch]
END
