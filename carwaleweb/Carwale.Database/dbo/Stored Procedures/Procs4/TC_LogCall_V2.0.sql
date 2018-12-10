IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LogCall_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LogCall_V2]
GO

	

-- Created By:	Deepak Tripathi
-- Create date: 11th July 2016
-- Description:	Adding New Lead

-- =============================================
create  PROCEDURE [dbo].[TC_LogCall_V2.0]
	@LeadId			INT,
	@CallType		TINYINT,
	@LeadOwnerId	INT,
	@ScheduledOn	DATETIME,
	@FollowupComments VARCHAR(500),
	@TC_BusinessTypeId	TINYINT
AS
	BEGIN
		SET NOCOUNT ON;

		INSERT INTO TC_Calls (TC_LeadId ,CallType ,TC_UsersId ,ScheduledOn ,IsActionTaken ,
								TC_CallActionId,ActionTakenOn ,ActionComments,CreatedOn, TC_BusinessTypeId)
				VALUES (@LeadId,@CallType,@LeadOwnerId,@ScheduledOn,1,2,@ScheduledOn,@FollowupComments,@ScheduledOn, @TC_BusinessTypeId)
END


