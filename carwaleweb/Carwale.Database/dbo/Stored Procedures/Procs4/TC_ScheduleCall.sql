IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ScheduleCall]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ScheduleCall]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 23rd June 2016
-- Description : <Schedule Call>
-- Modified By : Ashwini Dhamankar on July 12,2016 (added @TC_NextActionDate)
--declare @NewCallId INT
--exec [TC_ScheduleCall] 88927,29637,3,'2016-09-18 06:00:00.000',NULL,'11111111',2,NULL,NULL,'2016-09-22 05:00:00.000',6
--Modified By Deepak on 26th Sep 2016 - Fetched BusinessType from  TC_Leads
--Modified By  khushaboo patil on 14th oct 2016 modified tasklistupdate sp call with versioned sp [TC_TaskListUpdate_V16.10.1]
-- =============================================
CREATE PROCEDURE [dbo].[TC_ScheduleCall] @TC_UsersId INT
	,@TC_LeadId INT
	,@CallType INT
	,@ScheduleDate DATETIME
	,@AlertId INT
	,@LastCallComment VARCHAR(MAX)
	,@TC_NextActionId SMALLINT
	,@NextCallTo INT
	,@NewCallId INT OUTPUT
	,@TC_NextActionDate DATETIME = NULL
	,@TC_BusinessTypeId TINYINT = NULL
AS
BEGIN
	SET @NewCallId = - 1

	IF @CallType IS NULL
		SET @CallType = 4

	IF @ScheduleDate IS NULL
		SET @ScheduleDate = GETDATE()

	SELECT @TC_BusinessTypeId = ISNULL(TC_BusinessTypeId, 0)
	FROM TC_Lead WITH (NOLOCK)
	WHERE TC_LeadId = @TC_LeadId

	-- Dispose in case there is already a call on this lead for same user
	DECLARE @ExistingCallId INT

	SELECT @ExistingCallId = TC_CallsId
	FROM TC_ActiveCalls WITH (NOLOCK)
	WHERE TC_LeadId = @TC_LeadId
		AND TC_UsersId = @TC_UsersId
		AND TC_BusinessTypeId = @TC_BusinessTypeId

	IF @@ROWCOUNT > 0
		EXEC TC_DisposeCall @ExistingCallId
			,@LastCallComment
			,NULL
			,@ScheduleDate
			,@TC_UsersId

	IF (
			@TC_UsersId IS NOT NULL
			AND @TC_LeadId IS NOT NULL
			)
	BEGIN
		INSERT INTO TC_Calls (
			TC_LeadId
			,CallType
			,TC_UsersId
			,ScheduledOn
			,CreatedOn
			,IsActionTaken
			,TC_CallActionId
			,ActionTakenOn
			,ActionComments
			,AlertId
			,TC_NextActionId
			,TC_NextActionDate
			,TC_BusinessTypeId
			)
		VALUES (
			@TC_LeadId
			,@CallType
			,@TC_UsersId
			,@ScheduleDate
			,GETDATE()
			,0
			,NULL
			,NULL
			,NULL
			,NULL
			,@TC_NextActionId
			,@TC_NextActionDate
			,@TC_BusinessTypeId
			)

		SET @NewCallId = SCOPE_IDENTITY()

		INSERT INTO TC_ActiveCalls (
			TC_CallsId
			,TC_LeadId
			,CallType
			,TC_UsersId
			,ScheduledOn
			,AlertId
			,LastCallDate
			,LastCallComment
			,TC_NextActionId
			,NextCallTo
			,TC_NextActionDate
			,TC_BusinessTypeId
			)
		VALUES (
			@NewCallId
			,@TC_LeadId
			,@CallType
			,@TC_UsersId
			,@ScheduleDate
			,@AlertId
			,GETDATE()
			,@LastCallComment
			,@TC_NextActionId
			,@NextCallTo
			,@TC_NextActionDate
			,@TC_BusinessTypeId
			)

		EXEC [TC_TaskListUpdate_V16.10.1] 2
			,@NewCallId
			,1
	END
END
