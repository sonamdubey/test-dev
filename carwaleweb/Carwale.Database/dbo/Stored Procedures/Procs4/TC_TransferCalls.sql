IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TransferCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TransferCalls]
GO

	-- Created By:	Deepak
-- Create date: 30 June 2016
-- Modified By : Suresh Prajapati on 03rd Aug, 2016
-- Description : Added Parameter @BusinessTypeId
-- =============================================
CREATE PROCEDURE [dbo].[TC_TransferCalls] @OldLeadUserId INT
	,@NewLeadUserId INT
	,@LeadId INT
	,@CallType TINYINT
	,@BusinessTypeId TINYINT = 3
AS
BEGIN
	DECLARE @ActiveCallId AS INT
	DECLARE @ScheduleDate AS DATETIME = GETDATE()

	SELECT @ActiveCallId = TC.TC_CallsId
	FROM TC_Calls TC WITH (NOLOCK)
	WHERE TC.TC_LeadId = @LeadId
		AND TC.TC_UsersId = @OldLeadUserId
		AND IsActionTaken = 0

	IF @ActiveCallId > 0
	BEGIN
		UPDATE TC_Calls
		SET TC_UsersId = @NewLeadUserId
			,TC_BusinessTypeId = @BusinessTypeId
		WHERE TC_CallsId = @ActiveCallId

		UPDATE TC_ActiveCalls
		SET TC_UsersId = @NewLeadUserId
			,TC_BusinessTypeId = @BusinessTypeId
		WHERE TC_CallsId = @ActiveCallId

		EXEC TC_TaskListUpdate 1
			,@ActiveCallId
			,1

		EXEC TC_TaskListUpdate 2
			,@ActiveCallId
			,1
	END
	ELSE
	BEGIN
		EXEC TC_ScheduleCall @NewLeadUserId
			,@LeadId
			,@CallType
			,@ScheduleDate
			,NULL
			,'Inquiry Reassigned'
			,NULL
			,NULL
			,NULL
			,NULL
			,@BusinessTypeId
	END
END

