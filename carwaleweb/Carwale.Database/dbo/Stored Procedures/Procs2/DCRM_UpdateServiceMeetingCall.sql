IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateServiceMeetingCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateServiceMeetingCall]
GO

	-- =============================================
-- Author     :	Sachin Bharti
-- Create date: 11th Jan 2013
-- Description:	This proc update data for change alert regarding Service Meeting
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_UpdateServiceMeetingCall]
	@UserAlertId NUMERIC(18,0),
	@ActionTakenON DATETIME ,
	@ActionTakenBy INT,
	@TextComment  VARCHAR(500)
	
AS
BEGIN
	
	UPDATE DCRM_ServiceMeeting SET ActionTakenOn = @ActionTakenOn , ActionTakenBy = @ActionTakenBy , ActionComments = @TextComment
	WHERE UserAlertId = @UserAlertId 
	
END