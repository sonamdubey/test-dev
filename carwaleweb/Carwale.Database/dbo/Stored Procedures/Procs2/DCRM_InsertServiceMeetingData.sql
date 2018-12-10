IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InsertServiceMeetingData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InsertServiceMeetingData]
GO

	
-- =============================================
-- Author     :	Sachin Bharti
-- Create date: 11th Jan 2013
-- Description:	This proc saves the data for schedule Service Meeting 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_InsertServiceMeetingData]
	@DealerId			NUMERIC(18,0),
	@ScheduledBy		INT,
	@ScheduledTo		INT,
	@ScheduledDate		DATETIME,
	@CreatedOn			DATETIME,
	@UserAlertId		NUMERIC(18,0)
AS
BEGIN
	
	INSERT INTO DCRM_ServiceMeeting
			(DealerId, ScheduledBy, ScheduledTo, ScheduledDate,  CreatedOn,UserAlertId)
	VALUES
			(@DealerId, @ScheduledBy, @ScheduledTo, @ScheduledDate, @CreatedOn, @UserAlertId)
	
END
