IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_LogResolutionCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_LogResolutionCall]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	15th Nov 2013
-- Description	:	Log call with closed status in DCRM_Calls for resolution call
--					and made new entry in DCRM_SalesMeeting with meeting type 3
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_LogResolutionCall] 
	@DealerId	NUMERIC(18,0),
	@UserId		NUMERIC(18,0),
	@ActionComments		VARCHAR(1500),
	@Result				INT = -1 OUTPUT
AS
	BEGIN

		INSERT INTO DCRM_Calls
				(
					DealerId, UserId, ScheduleDate, CreatedOn, Subject, ScheduledBy, Comments,ActionTakenId,CalledDate
				) 
				VALUES
				( 
					@DealerId, @UserId,GETDATE(),GETDATE(),'Resolution Call',@UserId,@ActionComments,1,GETDATE() --For closed status
				)

		INSERT INTO DCRM_SalesMeeting( DealerId,ActionComments,ActionTakenBy,IsActionTaken,ActionTakenOn,MeetingType,MeetingDate,DealerType,MeetingMode)
								VALUES(@DealerID,@ActionComments,@UserId,1,GETDATE(),3,GETDATE(),1,1)
								--1 for Action taken and 3 for Resolution call and 1 for Carwale Dealers , 1 for face to face meeting mode
							SET @Result = SCOPE_IDENTITY()
		
	END

