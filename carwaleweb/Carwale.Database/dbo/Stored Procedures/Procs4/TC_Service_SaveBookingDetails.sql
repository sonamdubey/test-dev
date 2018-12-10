IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_SaveBookingDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_SaveBookingDetails]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: July 11,2016
-- Description:	To save service booking details
-- exec [TC_Service_SaveBookingDetails] 26436
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_SaveBookingDetails] 
@TC_InquiriesLeadId INT
AS
BEGIN
	UPDATE TC_InquiriesLead
	SET
	TC_LeadDispositionID = 4
	WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId
END
-----------------------
